namespace Bookstore.Cdk;

using Amazon.CDK;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.S3.Assets;
using Amazon.CDK.AWS.SSM;

using Constructs;

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
        this.CreateEc2Role(props);

        this.UploadAssetsToS3();

        this.CreateEc2Instance(props);

        this.ConfigureUserData();

        this.CreateCognitoUserPoolClient(props);

        _ = new CfnOutput(this, "EC2Url", new CfnOutputProps { Description = "The application URL", Value = $"https://{this.Instance.InstancePublicDnsName}" });
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
        this.Ec2Role = new Role(this, "EC2Role", new RoleProps
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
        this.Ec2Role.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
        this.Ec2Role.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] { "rekognition:DetectModerationLabels" },
            Resources = new[] { "*" }
        }));

        // Add permissions for the app to retrieve the database password in Secrets Manager
        this.Ec2Role.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
        props.ImageBucket.GrantReadWrite(this.Ec2Role);

        // Create an Amazon CloudWatch log group for the website
        _ = new LogGroup(this, $"{Constants.AppName}LogGroup", new LogGroupProps
        {
            LogGroupName = Constants.AppName,
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        // Add permissions to write logs
        this.Ec2Role.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
        this.ServerConfigScriptAsset = new Asset(this, "ServerConfigScriptAsset", new AssetProps
        {
            Path = "app/Bookstore.Cdk/EC2Artifacts/configure_ec2_web_app.sh"
        });
        this.ServerConfigScriptAsset.GrantRead(this.Ec2Role);

        this.WebAppAsset = new Asset(this, "WebAppAsset", new AssetProps
        {
            Path = "app/Bookstore.Web/bin/Release/net6.0/publish"
        });
        this.WebAppAsset.GrantRead(this.Ec2Role);

        this.SslConfigAsset = new Asset(this, "ApacheSSLConfigAsset", new AssetProps
        {
            Path = "app/Bookstore.Cdk/EC2Artifacts/ssl.conf"
        });
        this.SslConfigAsset.GrantRead(this.Ec2Role);

        this.WebAppVirtualHostConfigAsset = new Asset(this, "WebAppVirtualHostConfigAsset", new AssetProps
        {
            Path = "app/Bookstore.Cdk/EC2Artifacts/bobsbookstore.conf"
        });
        this.WebAppVirtualHostConfigAsset.GrantRead(this.Ec2Role);

        this.KestrelServiceAsset = new Asset(this, "KestrelServiceAsset", new AssetProps
        {
            Path = "app/Bookstore.Cdk/EC2Artifacts/bobsbookstore.service"
        });
        this.KestrelServiceAsset.GrantRead(this.Ec2Role);
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

        this.Instance = new Instance_(this, "WebServer", new Amazon.CDK.AWS.EC2.InstanceProps
        {
            Vpc = props.Vpc,
            VpcSubnets = new SubnetSelection { SubnetType = SubnetType.PUBLIC },
            SecurityGroup = webAppSecurityGroup,
            InstanceType = Amazon.CDK.AWS.EC2.InstanceType.Of(InstanceClass.BURSTABLE3, InstanceSize.SMALL),
            MachineImage = ami,
            Role = this.Ec2Role,
            RequireImdsv2 = true,
            UserDataCausesReplacement = true
        });
    }

    internal void ConfigureUserData()
    {
        var serverConfigScriptFilePath = this.Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = this.ServerConfigScriptAsset.Bucket,
            BucketKey = this.ServerConfigScriptAsset.S3ObjectKey
        });

        var webAppFilePath = this.Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = this.WebAppAsset.Bucket,
            BucketKey = this.WebAppAsset.S3ObjectKey
        });

        var sslConfigFilePath = this.Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = this.SslConfigAsset.Bucket,
            BucketKey = this.SslConfigAsset.S3ObjectKey
        });

        var webAppConfigVirtualHostFilePath = this.Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = this.WebAppVirtualHostConfigAsset.Bucket,
            BucketKey = this.WebAppVirtualHostConfigAsset.S3ObjectKey
        });

        var kestrelServiceFilPath = this.Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = this.KestrelServiceAsset.Bucket,
            BucketKey = this.KestrelServiceAsset.S3ObjectKey
        });

        this.Instance.UserData.AddExecuteFileCommand(new ExecuteFileOptions
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
                    $"https://{this.Instance.InstancePublicDnsName}/signin-oidc"
                },
                LogoutUrls = new[]
                {
                    $"https://{this.Instance.InstancePublicDnsName}/"
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
