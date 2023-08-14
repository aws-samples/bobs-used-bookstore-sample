using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Logs;

namespace SharedInfrastructure.Production;

public class CoreStack : Stack
{
    public Role ApplicationRole { get; private set; }

    internal CoreStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        CreateApplicationRole();
    }

    internal void CreateApplicationRole()
    {
        //=========================================================================================
        // Create an application role for the website, seeded with the Systems Manager
        // permissions allowing future management from Systems Manager and remote access
        // from the console. Also add the CodeDeploy service role allowing deployments through
        // CodeDeploy if we wish. The trust relationship to EC2 enables the running application
        // to obtain temporary, auto-rotating credentials for calls to service APIs made by the
        // AWS SDK for .NET, without needing to place credentials onto the compute host.
        ApplicationRole = new Role(this, $"{Constants.AppName}Role", new RoleProps
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
        ApplicationRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
        ApplicationRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] { "rekognition:DetectModerationLabels" },
            Resources = new[] { "*" }
        }));

        // Add permissions for the app to retrieve the database password in Secrets Manager
        //ApplicationRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        //{
        //    Effect = Effect.ALLOW,
        //    Actions = new[]
        //    {
        //        "secretsmanager:DescribeSecret",
        //        "secretsmanager:GetSecretValue"
        //    },
        //    Resources = new[]
        //    {
        //        "*"
        //    }
        //}));

        // Create an Amazon CloudWatch log group for the website
        _ = new LogGroup(this, $"{Constants.AppName}LogGroup", new LogGroupProps
        {
            LogGroupName = Constants.AppName,
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        // Add permissions to write logs
        ApplicationRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
}