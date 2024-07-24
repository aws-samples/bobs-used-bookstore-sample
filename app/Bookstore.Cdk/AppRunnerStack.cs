using Amazon.CDK;
using Amazon.CDK.AWS.AppRunner.Alpha;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.Ecr.Assets;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SSM;
using Amazon.CDK.CustomResources;
using Bookstore.Common;
using Constructs;
using System.Collections.Generic;

namespace Bookstore.Cdk;

public class AppRunnerStackProps : StackProps
{
    public IVpc Vpc { get; set; }

    public DatabaseInstance Database { get; set; }

    public Bucket ImageBucket { get; set; }

    public UserPool WebAppUserPool { get; set; }
}

public class AppRunnerStack : Stack
{
    private SecurityGroup appRunnerSecurityGroup;
    private Role appRunnerInstanceRole;
    private Role appRunnerEcrRole;
    private DockerImageAsset dockerImage;

    internal AppRunnerStack(Construct scope, string id, AppRunnerStackProps props) : base(scope, id, props)
    {
        CreateAppRunnerSecurityGroup(props);

        CreateAppRunnerInstanceRole(props);

        CreateAppRunnerEcrRole();

        CreateDockerImage();

        var appRunnerService = CreateAppRunnerService(props);

        CreateCognitoUserPoolClient(appRunnerService, props);

        //UpdateAppRunnerEnvironmentVariables(props.WebAppUserPool, userPoolClient, appRunnerService);
    }

    internal void CreateAppRunnerSecurityGroup(AppRunnerStackProps props)
    {
        appRunnerSecurityGroup = new SecurityGroup(this, "AppRunnerSecurityGroup", new SecurityGroupProps
        {
            SecurityGroupName = $"{Constants.AppName}AppRunnerSecurityGroup",
            Vpc = props.Vpc,
            AllowAllOutbound = true
        });
    }

    internal void CreateAppRunnerInstanceRole(AppRunnerStackProps props)
    {
        appRunnerInstanceRole = new Role(this, "AppRunnerRole", new RoleProps
        {
            AssumedBy = new ServicePrincipal("tasks.apprunner.amazonaws.com")
        });

        // Access to read parameters by path is not in the AmazonSSMManagedInstanceCore
        // managed policy
        appRunnerInstanceRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
        appRunnerInstanceRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] { "rekognition:DetectModerationLabels" },
            Resources = new[] { "*" }
        }));

        // Add permissions to the app to access the S3 image bucket
        props.ImageBucket.GrantReadWrite(appRunnerInstanceRole);

        // Add permissions to write logs
        appRunnerInstanceRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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

        appRunnerSecurityGroup.Connections.AllowTo(props.Database, Port.Tcp(1433));
    }

    internal void CreateAppRunnerEcrRole()
    {
        appRunnerEcrRole = new Role(this, "AppRunnerEcrRole", new RoleProps
        {
            AssumedBy = new ServicePrincipal("build.apprunner.amazonaws.com"),
            RoleName = $"{Constants.AppName}AppRunnerEcrRole"
        });
    }

    internal void CreateDockerImage()
    {
        dockerImage = new DockerImageAsset(this, $"{Constants.AppName}AppRunnerDockerImage", new DockerImageAssetProps
        {
            Directory = "" //This points to the current directory
        });
    }

    private Service CreateAppRunnerService(AppRunnerStackProps props)
    {
        var appRunnerService = new Service(this, "AppRunnerService", new ServiceProps
        {
            Source = Source.FromAsset(new AssetProps { Asset = dockerImage, ImageConfiguration = new ImageConfiguration { Port = 80 } }),
            Cpu = Cpu.HALF_VCPU,
            Memory = Memory.ONE_GB,
            InstanceRole = appRunnerInstanceRole,
            AccessRole = appRunnerEcrRole, 
            VpcConnector = new VpcConnector(this, "VPCConnector", new VpcConnectorProps
            {
                VpcConnectorName = $"{Constants.AppName}VPCConnector",
                Vpc = props.Vpc,
                VpcSubnets = new SubnetSelection { Subnets = props.Vpc.PrivateSubnets },
                SecurityGroups = new[] { appRunnerSecurityGroup }
            })
        });

        appRunnerService.AddEnvironmentVariable("Services:Authentication", "aws");
        appRunnerService.AddEnvironmentVariable("Services:Database", "aws");
        appRunnerService.AddEnvironmentVariable("Services:FileService", "aws");
        appRunnerService.AddEnvironmentVariable("Services:ImageValidationService", "aws");
        appRunnerService.AddEnvironmentVariable("Services:LoggingService", "aws");
        appRunnerService.AddEnvironmentVariable("Cognito:ClientIdSSMParameterName", "Cognito:AppRunnerClientId");
        appRunnerService.AddEnvironmentVariable("ASPNETCORE_HTTP_PORTS", "80");  // .NET8 default port changed to 8080. Instructing to use 80 https://learn.microsoft.com/en-us/dotnet/core/compatibility/containers/8.0/aspnet-port     

        return appRunnerService;
    }

    internal UserPoolClient CreateCognitoUserPoolClient(Service appRunnerService, AppRunnerStackProps props)
    {
        var appRunnerUserPoolClient = new UserPoolClient(this, "AppRunnerClient", new UserPoolClientProps
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
                    $"https://{appRunnerService.ServiceUrl}/signin-oidc"
                },
                LogoutUrls = new[]
                {
                    $"https://{appRunnerService.ServiceUrl}/"
                }
            }
        });

        _ = new StringParameter(this, "CognitoAppRunnerUserPoolClientSSMParameter", new StringParameterProps
        {
            ParameterName = $"/{Constants.AppName}/Authentication/Cognito/AppRunnerClientId",
            StringValue = appRunnerUserPoolClient.UserPoolClientId
        });

        return appRunnerUserPoolClient;
    }
}