using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.ECS;
using Amazon.CDK.AWS.ECS.Patterns;
using Amazon.CDK.AWS.S3;
using Constructs;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.SSM;
using Bookstore.Common;
using HealthCheck = Amazon.CDK.AWS.ElasticLoadBalancingV2.HealthCheck;

namespace Bookstore.Cdk;

public class EcsStackProps : StackProps
{
    public IVpc Vpc { get; set; }

    public DatabaseInstance Database { get; set; }

    public Bucket ImageBucket { get; set; }

    public UserPool WebAppUserPool { get; set; }
}

public class EcsStack : Stack
{
    internal EcsStack(Construct scope, string id, EcsStackProps props) : base(scope, id, props)
    {
        var service = CreateEcsStack(props);

        CreateCognitoUserPoolClient(service, props);

        CreateEcsPermissions(service, props);
    }

    internal ApplicationLoadBalancedFargateService CreateEcsStack(EcsStackProps props)
    {
        var cluster = new Cluster(this, "ECSCluster", new ClusterProps
        {
            Vpc = props.Vpc
        });

        var service = new ApplicationLoadBalancedFargateService(this, "ECSService",
            new ApplicationLoadBalancedFargateServiceProps
            {
                Cluster = cluster,
                CircuitBreaker = new DeploymentCircuitBreaker { Rollback = true },
                DesiredCount = 2,
                Cpu = 512,
                MemoryLimitMiB = 1024,
                HealthCheckGracePeriod = Duration.Seconds(30),
                TaskImageOptions = new ApplicationLoadBalancedTaskImageOptions
                {
                    Image = ContainerImage.FromAsset(".\\"),
                    Environment = new Dictionary<string, string>
                    {
                            { "Services:Authentication", "local" }, //Can't use Cognito hosted UI without an https redirect.
                            { "Services:Database", "aws" },
                            { "Services:FileService", "aws" },
                            { "Services:ImageValidationService", "aws" },
                            { "Services:LoggingService", "aws" }
                    }
                }
            });

        service.TargetGroup.HealthCheck = new HealthCheck
        {
            Path = "/Home/Privacy",
            HealthyThresholdCount = 2,
            Timeout = Duration.Seconds(25)
        };

        return service;
    }

    internal void CreateCognitoUserPoolClient(ApplicationLoadBalancedFargateService service, EcsStackProps props)
    {
        var ecsUserPoolClient = new UserPoolClient(this, "CognitoECSAppClient", new UserPoolClientProps
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
                        $"https://{service.LoadBalancer.LoadBalancerDnsName}/signin-oidc"
                    },
                LogoutUrls = new[]
                {
                        $"https://{service.LoadBalancer.LoadBalancerDnsName}/"
                    }
            }
        });

        _ = new[]
        {
                new StringParameter(this, "CognitoECSAppClientSSMParameter", new StringParameterProps
                {
                    ParameterName = $"/{Constants.AppName}/Authentication/Cognito/ECSClientId",
                    StringValue = ecsUserPoolClient.UserPoolClientId
                })
            };
    }

    internal void CreateEcsPermissions(ApplicationLoadBalancedFargateService service, EcsStackProps props)
    {
        service.Service.TaskDefinition.TaskRole.AddToPrincipalPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Actions = new[] { "rekognition:DetectModerationLabels" },
            Resources = new[] { "*" }
        }));

        service.Service.TaskDefinition.TaskRole.AddToPrincipalPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Actions = new[] { "ssm:GetParametersByPath", "ssm:GetParameter" },
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

        service.Service.TaskDefinition.TaskRole.AddToPrincipalPolicy(new PolicyStatement(new PolicyStatementProps
        {
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

        service.Service.Connections.AllowTo(props.Database, Port.Tcp(1433));

        props.ImageBucket.GrantReadWrite(service.Service.TaskDefinition.TaskRole);
    }
}