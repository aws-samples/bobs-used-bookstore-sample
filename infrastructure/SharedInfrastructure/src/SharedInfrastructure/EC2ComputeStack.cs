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
using Amazon.CDK.AWS.StepFunctions.Tasks;
using Amazon.CDK.CustomResources;
using System.Collections.Generic;

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
}

public class EC2ComputeStack : Stack
{
    private const string EnvStageName = "Production";

    // -Test- in the parameter key roots maps to the ASP.NET Core environment in use
    // when using the "AdminSite Integrated" or "CustomerSite Integrated" launch profiles
    private string adminSiteParametersRoot = $"BobsUsedBooks-{EnvStageName}-AdminSite";
    private string adminSiteUserPoolName = $"BobsUsedBooks-{EnvStageName}-AdminPool";
    private const string adminSiteUserPoolCallbackUrlRoot = "http://localhost:5000";
    private string bookstoreDbCredentialsParameter = $"BobsUsedBooks-{EnvStageName}-DbSettings";
    private string adminSiteLogGroupName = $"BobsUsedBooks-{EnvStageName}-AdminSiteLogs";

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

        CreateAdminAppCognitoUserPool(props);
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
                Arn.Format(new ArnComponents
                {
                    Service = "ssm",
                    Resource = "parameter",
                    ResourceName = $"{bookstoreDbCredentialsParameter}/*"
                }, this),

                Arn.Format(new ArnComponents
                {
                    Service = "ssm",
                    Resource = "parameter",
                    ResourceName = $"{adminSiteParametersRoot}/*"
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
        _ = new LogGroup(this, "BobsBookstoreAdminAppLogGroup", new LogGroupProps
        {
            LogGroupName = adminSiteLogGroupName,
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
            Path = "configure_ec2_admin_app.sh"
        });
        ServerConfigScriptAsset.GrantRead(AdminAppRole);

        AdminAppAsset = new Asset(this, "AdminAppAsset", new AssetProps
        {
            Path = "../../app/Bookstore.Admin/bin/Release/net6.0/publish"
        });
        AdminAppAsset.GrantRead(AdminAppRole);

        SslConfigAsset = new Asset(this, "ApacheSSLConfigAsset", new AssetProps
        {
            Path = "ssl.conf"
        });
        SslConfigAsset.GrantRead(AdminAppRole);

        AdminAppVirtualHostConfigAsset = new Asset(this, "AdminAppVirtualHostConfigAsset", new AssetProps
        {
            Path = "bookstoreadmin.conf"
        });
        AdminAppVirtualHostConfigAsset.GrantRead(AdminAppRole);

        KestrelServiceAsset = new Asset(this, "KestrelServiceAsset", new AssetProps
        {
            Path = "bookstoreadmin.service"
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

        var adminAppSG = new SecurityGroup(this, "AdminAppSecurityGroup", new SecurityGroupProps
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

    internal void CreateAdminAppCognitoUserPool(EC2ComputeStackProps props)
    {
        var adminSiteUserPool = new UserPool(this, "AdminUserPool", new UserPoolProps
        {
            UserPoolName = adminSiteUserPoolName,
            SelfSignUpEnabled = false,
            StandardAttributes = new StandardAttributes
            {
                Email = new StandardAttribute
                {
                    Required = true
                }
            },
            AutoVerify = new AutoVerifiedAttrs { Email = true },
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        // Create default admin user for testing
        _ = new AwsCustomResource(this, "CreateAdminUser", new AwsCustomResourceProps
        {
            OnCreate = new AwsSdkCall
            {
                Service = "CognitoIdentityServiceProvider",
                Action = "adminCreateUser",
                Parameters = new Dictionary<string, string>
                {
                    { "UserPoolId", adminSiteUserPool.UserPoolId },
                    { "Username", "admin" },
                    { "TemporaryPassword", "P@ssword1" },
                    { "MessageAction", "SUPPRESS" }
                },                
                PhysicalResourceId = PhysicalResourceId.Of("bobsbookstoreadminuser")
            },
            OnDelete = new AwsSdkCall
            {
                Service = "CognitoIdentityServiceProvider",
                Action = "adminDeleteUser",
                Parameters = new Dictionary<string, string>
                {
                    { "UserPoolId", adminSiteUserPool.UserPoolId },
                    { "Username", "admin" }
                }
            },
            Policy = AwsCustomResourcePolicy.FromSdkCalls(new SdkCallsPolicyOptions { Resources = AwsCustomResourcePolicy.ANY_RESOURCE })
        });

        // As with the customer pool, the admin pool uses Hosted UI and so requires a HTTPS callback url
        var adminSiteUserPoolClient = adminSiteUserPool.AddClient("AdminPoolClient", new UserPoolClientProps
        {
            UserPool = adminSiteUserPool,
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
                    $"{adminSiteUserPoolCallbackUrlRoot}/signin-oidc",
                    $"https://{Instance.InstancePublicDnsName}/signin-oidc"
                },
                LogoutUrls = new[]
                {
                    $"{adminSiteUserPoolCallbackUrlRoot}/",
                    $"https://{Instance.InstancePublicDnsName}/"
                }
            }
        });

        var adminSiteUserPoolDomain = adminSiteUserPool.AddDomain("AdminPoolDomain", new UserPoolDomainOptions
        {
            CognitoDomain = new CognitoDomainOptions
            {
                // The prefix must be unique across the AWS Region in which the pool is created
                DomainPrefix = $"bobs-bookstore-admin-test-{Account}"
            }
        });

        adminSiteUserPoolDomain.SignInUrl(adminSiteUserPoolClient, new SignInUrlOptions
        {
            RedirectUri = $"{adminSiteUserPoolCallbackUrlRoot}/signin-oidc"
        });

        _ = new[]
        {
            new StringParameter(this, "AdminSiteUserPoolClientId", new StringParameterProps
            {
                ParameterName = $"/{adminSiteParametersRoot}/Authentication/Cognito/ClientId",
                StringValue = adminSiteUserPoolClient.UserPoolClientId
            }),

            new StringParameter(this, "AdminSiteUserPoolMetadataAddress", new StringParameterProps
            {
                ParameterName = $"/{adminSiteParametersRoot}/Authentication/Cognito/MetadataAddress",
                StringValue = $"https://cognito-idp.{Region}.amazonaws.com/{adminSiteUserPool.UserPoolId}/.well-known/openid-configuration"
            }),

            new StringParameter(this, "AdminSiteUserPoolCognitoDomain", new StringParameterProps
            {
                ParameterName = $"/{adminSiteParametersRoot}/Authentication/Cognito/CognitoDomain",
                StringValue = adminSiteUserPoolDomain.BaseUrl()
            })
        };

        AdminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[]
            {
                "cognito-idp:AdminListGroupsForUser",
                "cognito-idp:AdminGetUser",
                "cognito-idp:DescribeUserPool",
                "cognito-idp:DescribeUserPoolClient",
                "cognito-idp:ListUsers"
            },

            Resources = new[]
            {
                Arn.Format(new ArnComponents
                {
                    Service = "cognito-idp",
                    Resource = "userpool",
                    ResourceName = adminSiteUserPool.UserPoolId
                }, this)
            }
        }));
    }
}