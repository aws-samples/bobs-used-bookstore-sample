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

// Defines the minimal AWS Cloud resources for the bookstore's admin and customer-facing
// applications so that when using the "AdminSite Integrated" and "" launch profiles, the
// application code uses some cloud resources to illustrate how to use some AWS services.
//
// The stack creates a private Amazon S3 bucket to hold the book cover images, a CloudFront
// distribution that will be used to access the bucket (allowing the bucket to remain private),
// Amazon Cognito User Pools, and parameters in Systems Manager Parameter Store.
//
// Note that these are not enough resources to support a full "production" deployment to AWS.
// An additional stack, defining all resources including an Amazon VPC, and SQL Server database in
// Amazon RDS, will be added subsequently.
//
// Settings such as the CloudFront distribution domain, S3 bucket name, and user pools
// for the web apps are placed into Systems Manager Parameter Store.
// The sample applications read all the parameters, in one pass, using the
// AWS-DotNet-Extensions-Configuration package (https://github.com/aws/aws-dotnet-extensions-configuration).
// This package reads all parameters beneath a specified parameter key root, injecting them into the
// application configurations at runtime, just as if they were statically held in appsettings.json.
// This is why the account used when debugging the applications, or the application roles when
// the applications are deployed to compute hosts on AWS, need permissions to call the
// ssm:GetParametersByPath action.
//
public class EC2ComputeStackProps : StackProps
{
    public IVpc Vpc { get; set; }

    public DatabaseInstance Database { get; set; }

    public Bucket ImageBucket { get; set; }

    public UserPool AdminAppUserPool { get; set; }
}

public class EC2ComputeStack : Stack
{
    private Role AdminAppRole;
    private Asset ServerConfigScriptAsset;
    private Asset AdminAppAsset;
    private Asset SslConfigAsset;
    private Asset AdminAppVirtualHostConfigAsset;
    private Asset KestrelServiceAsset;
    private Instance_ Instance;

    internal EC2ComputeStack(Construct scope, string id, EC2ComputeStackProps props) : base(scope, id, props)
    {
        CreateAdminAppRole(props);

        UploadAssetsToS3();

        CreateEc2Instance(props);

        ConfigureUserData();

        CreateAdminAppCognitoUserPoolClient(props);
    }

    internal void CreateAdminAppRole(EC2ComputeStackProps props)
    {
        //=========================================================================================
        // Create an application role for the admin website, seeded with the Systems Manager
        // permissions allowing future management from Systems Manager and remote access
        // from the console. Also add the CodeDeploy service role allowing deployments through
        // CodeDeploy if we wish. The trust relationship to EC2 enables the running application
        // to obtain temporary, auto-rotating credentials for calls to service APIs made by the
        // AWS SDK for .NET, without needing to place credentials onto the compute host.
        AdminAppRole = new Role(this, "AdminApplicationRole", new RoleProps
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
        AdminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] { "ssm:GetParametersByPath" },
            Resources = new[]
            {
                //Arn.Format(new ArnComponents
                //{
                //    Service = "ssm",
                //    Resource = "parameter",
                //    ResourceName = $"{bookstoreDbCredentialsParameter}/*"
                //}, this),

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
        AdminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] { "rekognition:DetectModerationLabels" },
            Resources = new[] { "*" }
        }));

        // Add permissions for the app to retrieve the database password in Secrets Manager
        //db.Secret.GrantRead(adminAppRole);
        AdminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
                //$"arn:aws:secretsmanager:{Region}:{Account}:secret:{db.Secret.SecretName}-??????"
            }
        }));

        // Add permissions to the app to access the S3 image bucket
        props.ImageBucket.GrantReadWrite(AdminAppRole);

        // Create an Amazon CloudWatch log group for the admin website
        _ = new LogGroup(this, $"{Constants.AppName}LogGroup", new LogGroupProps
        {
            LogGroupName = Constants.AppName,
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        // Add permissions to write logs
        AdminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[]
            {
                "logs:DescribeLogGroups",
                "logs:CreateLogGroup",
                "logs:CreateLogStream"
            },
            Resources = new[]
            {
                "arn:aws:logs:*:*:log-group:*"
            }
        }));

        AdminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[]
            {
                "logs:PutLogEvents",
            },
            Resources = new[]
            {
                "arn:aws:logs:*:*:log-group:*:log-stream:*",
            }
        }));
    }

    internal void UploadAssetsToS3()
    {
        ServerConfigScriptAsset = new Asset(this, "ServerConfigScriptAsset", new AssetProps
        {
            Path = "src/SharedInfrastructure/EC2Artifacts/configure_ec2_admin_app.sh"
        });
        ServerConfigScriptAsset.GrantRead(AdminAppRole);

        AdminAppAsset = new Asset(this, "AdminAppAsset", new AssetProps
        {
            Path = "../../app/Bookstore.Admin/bin/Release/net6.0/publish"
        });
        AdminAppAsset.GrantRead(AdminAppRole);

        SslConfigAsset = new Asset(this, "ApacheSSLConfigAsset", new AssetProps
        {
            Path = "src/SharedInfrastructure/EC2Artifacts/ssl.conf"
        });
        SslConfigAsset.GrantRead(AdminAppRole);

        AdminAppVirtualHostConfigAsset = new Asset(this, "AdminAppVirtualHostConfigAsset", new AssetProps
        {
            Path = "src/SharedInfrastructure/EC2Artifacts/bookstoreadmin.conf"
        });
        AdminAppVirtualHostConfigAsset.GrantRead(AdminAppRole);

        KestrelServiceAsset = new Asset(this, "KestrelServiceAsset", new AssetProps
        {
            Path = "src/SharedInfrastructure/EC2Artifacts/bookstoreadmin.service"
        });
        KestrelServiceAsset.GrantRead(AdminAppRole);
    }

    internal void CreateEc2Instance(EC2ComputeStackProps props)
    {
        var ami = MachineImage.Lookup(new LookupMachineImageProps
        {
            Name = "amzn2-x86_64-MATEDE_DOTNET-*",
            Owners = new[] { "amazon" }
        });

        var adminAppSG = new SecurityGroup(this, $"{Constants.AppName}AppSecurityGroup", new SecurityGroupProps
        {
            Vpc = props.Vpc,
            Description = "Allow HTTP access to sample app admin website",
            AllowAllOutbound = true
        });
        adminAppSG.AddIngressRule(Peer.AnyIpv4(), Port.Tcp(80), "HTTP access");
        adminAppSG.AddIngressRule(Peer.AnyIpv4(), Port.Tcp(443), "HTTPS access");
        adminAppSG.Connections.AllowTo(props.Database, Port.Tcp(1433), "Database");

        Instance = new Instance_(this, "WebServer", new Amazon.CDK.AWS.EC2.InstanceProps
        {
            Vpc = props.Vpc,
            VpcSubnets = new SubnetSelection { SubnetType = SubnetType.PUBLIC },
            SecurityGroup = adminAppSG,
            InstanceType = InstanceType.Of(InstanceClass.BURSTABLE3, InstanceSize.SMALL),
            MachineImage = ami,
            Role = AdminAppRole
        });
    }

    internal void ConfigureUserData()
    {
        var serverConfigScriptFilePath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = ServerConfigScriptAsset.Bucket,
            BucketKey = ServerConfigScriptAsset.S3ObjectKey
        });

        var adminAppFilePath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = AdminAppAsset.Bucket,
            BucketKey = AdminAppAsset.S3ObjectKey
        });

        var sslConfigFilePath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = SslConfigAsset.Bucket,
            BucketKey = SslConfigAsset.S3ObjectKey
        });

        var adminAppConfigVirtualHostFilePath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = AdminAppVirtualHostConfigAsset.Bucket,
            BucketKey = AdminAppVirtualHostConfigAsset.S3ObjectKey
        });

        var kestrelServiceFilPath = Instance.UserData.AddS3DownloadCommand(new S3DownloadOptions
        {
            Bucket = KestrelServiceAsset.Bucket,
            BucketKey = KestrelServiceAsset.S3ObjectKey
        });

        Instance.UserData.AddExecuteFileCommand(new ExecuteFileOptions
        {
            FilePath = serverConfigScriptFilePath,
            Arguments = $"{adminAppFilePath} {sslConfigFilePath} {adminAppConfigVirtualHostFilePath} {kestrelServiceFilPath}"
        });
    }

    internal void CreateAdminAppCognitoUserPoolClient(EC2ComputeStackProps props)
    {
        // As with the customer pool, the admin pool uses Hosted UI and so requires a HTTPS callback url
        var Ec2UserPoolClient = new UserPoolClient(this, "EC2Client", new UserPoolClientProps
        {
            UserPool = props.AdminAppUserPool,
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