using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.SSM;
using Amazon.CDK.AWS.S3.Assets;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.Logs;

namespace SharedInfrastructure.Production;

public class EC2ComputeStackProps : StackProps
{
    public IVpc Vpc { get; set; }

    public DatabaseInstance Database { get; set; }

    public Bucket ImageBucket { get; set; }

    public UserPool WebAppUserPool { get; set; }
}

public class EC2ComputeStack : Stack
{
    private Role Ec2Role;
    private Asset ServerConfigScriptAsset;
    private Asset WebAppAsset;
    private Asset SslConfigAsset;
    private Asset WebAppVirtualHostConfigAsset;
    private Asset KestrelServiceAsset;
    private Instance_ Instance;

    internal EC2ComputeStack(Construct scope, string id, EC2ComputeStackProps props) : base(scope, id, props)
    {
        CreateEc2Role(props);

        UploadAssetsToS3();

        CreateEc2Instance(props);

        ConfigureUserData();

        CreateCognitoUserPoolClient(props);

        _ = new CfnOutput(this, "EC2Url", new CfnOutputProps { Description = "The application URL", Value = $"https://{Instance.InstancePublicDnsName}" });
    }

    internal void CreateEc2Role(EC2ComputeStackProps props)
    {
        //=========================================================================================
        // Create an application role for the website, seeded with the Systems Manager
        // permissions allowing future management from Systems Manager and remote access
        // from the console. Also add the CodeDeploy service role allowing deployments through
        // CodeDeploy if we wish. The trust relationship to EC2 enables the running application
        // to obtain temporary, auto-rotating credentials for calls to service APIs made by the
        // AWS SDK for .NET, without needing to place credentials onto the compute host.
        Ec2Role = new Role(this, "EC2Role", new RoleProps
        {
            AssumedBy = new CompositePrincipal(new ServicePrincipal("ec2.amazonaws.com")),
            ManagedPolicies = new[]
            {
                ManagedPolicy.FromAwsManagedPolicyName("AmazonSSMManagedInstanceCore"),
                ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSCodeDeployRole")
            }
        });

        // Access to read parameters by path is not in the AmazonSSMManagedInstanceCore
        // managed policy
        Ec2Role.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] { "ssm:GetParametersByPath" },
            Resources = new[]
            {
                Arn.Format(new ArnComponents
                {
                    Service = "ssm",
                    Resource = "parameter",
                    ResourceName = $"{Constants.AppName}/*"
                }, this)
            }
        }));
        
        // Provide permission to allow access to Amazon Rekognition for processing uploaded
        // book images. Credentials for the calls will be provided courtesy of the application
        // role defined above, and the trust relationship with EC2.
        Ec2Role.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] { "rekognition:DetectModerationLabels" },
            Resources = new[] { "*" }
        }));

        // Add permissions for the app to retrieve the database password in Secrets Manager
        Ec2Role.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[]
            {
                "secretsmanager:DescribeSecret",
                "secretsmanager:GetSecretValue"
            },
            Resources = new[]
            {
                "*"
            }
        }));

        // Add permissions to the app to access the S3 image bucket
        props.ImageBucket.GrantReadWrite(Ec2Role);

        // Create an Amazon CloudWatch log group for the website
        _ = new LogGroup(this, $"{Constants.AppName}LogGroup", new LogGroupProps
        {
            LogGroupName = Constants.AppName,
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        // Add permissions to write logs
        Ec2Role.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[]
            {
                "logs:DescribeLogGroups",
                "logs:CreateLogGroup",
                "logs:CreateLogStream",
                "logs:PutLogEvents"
            },
            Resources = new[]
            {
                "arn:aws:logs:*:*:log-group:*:log-stream:*"
            }
        }));        
    }

    internal void UploadAssetsToS3()
    {
        ServerConfigScriptAsset = new Asset(this, "ServerConfigScriptAsset", new AssetProps
        {
            Path = "app/Bookstore.Cdk/EC2Artifacts/configure_ec2_web_app.sh"
        });
        ServerConfigScriptAsset.GrantRead(Ec2Role);

        WebAppAsset = new Asset(this, "WebAppAsset", new AssetProps
        {
            Path = "app/Bookstore.Web/bin/Release/net6.0/publish"
        });
        WebAppAsset.GrantRead(Ec2Role);

        SslConfigAsset = new Asset(this, "ApacheSSLConfigAsset", new AssetProps
        {
            Path = "app/Bookstore.Cdk/EC2Artifacts/ssl.conf"
        });
        SslConfigAsset.GrantRead(Ec2Role);

        WebAppVirtualHostConfigAsset = new Asset(this, "WebAppVirtualHostConfigAsset", new AssetProps
        {
            Path = "app/Bookstore.Cdk/EC2Artifacts/bobsbookstore.conf"
        });
        WebAppVirtualHostConfigAsset.GrantRead(Ec2Role);

        KestrelServiceAsset = new Asset(this, "KestrelServiceAsset", new AssetProps
        {
            Path = "app/Bookstore.Cdk/EC2Artifacts/bobsbookstore.service"
        });
        KestrelServiceAsset.GrantRead(Ec2Role);
    }

    internal void CreateEc2Instance(EC2ComputeStackProps props)
    {
        var ami = MachineImage.Lookup(new LookupMachineImageProps
        {
            Name = "amzn2-x86_64-MATEDE_DOTNET-*",
            Owners = new[] { "amazon" }
        });

        var webAppSecurityGroup = new SecurityGroup(this, $"{Constants.AppName}AppSecurityGroup", new SecurityGroupProps
        {
            Vpc = props.Vpc,
            Description = "Allow HTTP(S) access to Bobs Bookstore website",
            AllowAllOutbound = true
        });
        webAppSecurityGroup.AddIngressRule(Peer.AnyIpv4(), Port.Tcp(80), "HTTP access");
        webAppSecurityGroup.AddIngressRule(Peer.AnyIpv4(), Port.Tcp(443), "HTTPS access");
        webAppSecurityGroup.Connections.AllowTo(props.Database, Port.Tcp(1433), "Database");

        Instance = new Instance_(this, "WebServer", new Amazon.CDK.AWS.EC2.InstanceProps
        {
            Vpc = props.Vpc,
            VpcSubnets = new SubnetSelection { SubnetType = SubnetType.PUBLIC },
            SecurityGroup = webAppSecurityGroup,
            InstanceType = Amazon.CDK.AWS.EC2.InstanceType.Of(InstanceClass.BURSTABLE3, InstanceSize.SMALL),
            MachineImage = ami,
            Role = Ec2Role,
            RequireImdsv2 = true,
            UserDataCausesReplacement = true
        });
    }

    internal void ConfigureUserData()
    {
        var serverConfigScriptFilePath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = ServerConfigScriptAsset.Bucket,
            BucketKey = ServerConfigScriptAsset.S3ObjectKey
        });

        var webAppFilePath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = WebAppAsset.Bucket,
            BucketKey = WebAppAsset.S3ObjectKey
        });

        var sslConfigFilePath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = SslConfigAsset.Bucket,
            BucketKey = SslConfigAsset.S3ObjectKey
        });

        var webAppConfigVirtualHostFilePath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = WebAppVirtualHostConfigAsset.Bucket,
            BucketKey = WebAppVirtualHostConfigAsset.S3ObjectKey
        });

        var kestrelServiceFilPath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = KestrelServiceAsset.Bucket,
            BucketKey = KestrelServiceAsset.S3ObjectKey
        });

        Instance.UserData.AddExecuteFileCommand(new ExecuteFileOptions
        {
            FilePath = serverConfigScriptFilePath,
            Arguments = $"{webAppFilePath} {sslConfigFilePath} {webAppConfigVirtualHostFilePath} {kestrelServiceFilPath}"
        });
    }

    internal void CreateCognitoUserPoolClient(EC2ComputeStackProps props)
    {
        var Ec2UserPoolClient = new UserPoolClient(this, "EC2Client", new UserPoolClientProps
        {
            UserPool = props.WebAppUserPool,
            GenerateSecret = false,
            PreventUserExistenceErrors = true,
            SupportedIdentityProviders = new[]
            {
                UserPoolClientIdentityProvider.COGNITO
            },
            AuthFlows = new AuthFlow
            {
                UserPassword = true
            },
            OAuth = new OAuthSettings
            {
                Flows = new OAuthFlows
                {
                    AuthorizationCodeGrant = true
                },
                Scopes = new[]
                {
                    OAuthScope.OPENID,
                    OAuthScope.EMAIL,
                    OAuthScope.COGNITO_ADMIN,
                    OAuthScope.PROFILE
                },
                CallbackUrls = new[]
                {
                    $"https://{Instance.InstancePublicDnsName}/signin-oidc"
                },
                LogoutUrls = new[]
                {
                    $"https://{Instance.InstancePublicDnsName}/"
                }
            }
        });

        _ = new[]
        {
            new StringParameter(this, "EC2ClientId", new StringParameterProps
            {
                ParameterName = $"/{Constants.AppName}/Authentication/Cognito/EC2ClientId",
                StringValue = Ec2UserPoolClient.UserPoolClientId
            })
        };
    }
}
