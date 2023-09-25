using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.Logs;
using Amazon.CDK.AWS.AppRunner;
using System.Linq;
using Amazon.CDK.AWS.Ecr.Assets;
using Amazon.CDK.AWS.AppRunner.Alpha;
using System.IO;
using System;
using Amazon.CDK.AWS.RDS;

namespace SharedInfrastructure.Production;

public class AppRunnerStackProps : StackProps
{
    public IVpc Vpc { get; set; }

    //public DatabaseInstance Database { get; set; }

    public Bucket ImageBucket { get; set; }

    //public UserPool WebAppUserPool { get; set; }
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

        CreateAppRunnerService(props);
    }

    internal void CreateAppRunnerSecurityGroup(AppRunnerStackProps props)
    {
        appRunnerSecurityGroup = new SecurityGroup(this, $"{Constants.AppName}AppRunnerSecurityGroup", new SecurityGroupProps
        {
            SecurityGroupName = $"{Constants.AppName}AppRunnerSecurityGroup",
            Vpc = props.Vpc,
            AllowAllOutbound = true
        });
    }

    internal void CreateAppRunnerInstanceRole(AppRunnerStackProps props)
    {
        appRunnerInstanceRole = new Role(this, $"{Constants.AppName}AppRunnerRole", new RoleProps
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

        // Create an Amazon CloudWatch log group for the website
        _ = new LogGroup(this, $"{Constants.AppName}AppRunnerLogGroup", new LogGroupProps
        {
            LogGroupName = Constants.AppName,
            RemovalPolicy = RemovalPolicy.DESTROY
        });

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
    }

    internal void CreateAppRunnerEcrRole()
    {
        appRunnerEcrRole = new Role(this, $"{Constants.AppName}AppRunnerEcrRole", new RoleProps
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

    private void CreateAppRunnerService(AppRunnerStackProps props)
    {
        _ = new Service(this, $"{Constants.AppName}AppRunnerService", new ServiceProps
        {
            Source = Source.FromAsset(new AssetProps { Asset = dockerImage, ImageConfiguration = new ImageConfiguration { Port = 80 } }),
            InstanceRole = appRunnerInstanceRole,
            AccessRole = appRunnerEcrRole,
            VpcConnector = new VpcConnector(this, $"{Constants.AppName}VpcConnector", new VpcConnectorProps
            {
                VpcConnectorName = $"{Constants.AppName}VpcConnector",
                Vpc = props.Vpc,
                VpcSubnets = new SubnetSelection() { Subnets = props.Vpc.PrivateSubnets },
                SecurityGroups = new[] { appRunnerSecurityGroup }
            })
        });
    }
}