using System.Collections.Generic;
using Amazon.CDK;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SSM;
using Amazon.CDK.AWS.Logs;
using Constructs;

namespace BookstoreCoreInfra;

// Defines the core resources that make up the BobsBookstore application when deployed to the
// AWS Cloud.
public class CoreResourceStack : Stack
{
    private const string customerParameterNameRoot = "BobsUsedBookCustomerStore";
    private const string adminParameterNameRoot = "BobsUsedBookAdminStore";
    private const string bookstoreDbInstance = "bookstoredb";
    private const string customerUserPoolName = "BobsBookstoreCustomerUsers";
    private const string adminUserPoolName = "BobsBookstoreAdminUsers";
    private const string adminLogGroupName = "BobsBookstoreAdminLogs";
    private const string customerLogGroupName = "BobsBookstoreCustomerLogs";

    internal CoreResourceStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        // Create a new vpc spanning two AZs and with public and private subnets
        // to host the application resources
        var vpc = new Vpc(this, "BobsBookstoreVpc", new VpcProps
        {
            IpAddresses = IpAddresses.Cidr("10.0.0.0/16"),
            // Cap at 2 AZs in case we are deployed to a region with only 2
            MaxAzs = 2,
            // Reduce the number of NAT gateways to 1 (the default is to create 1 gateway per AZ - more
            // than we need for this sample)
            NatGateways = 1,
            SubnetConfiguration = new []
            {
                new SubnetConfiguration
                {
                    CidrMask = 24,
                    SubnetType = SubnetType.PUBLIC,
                    Name = "BookstorePublicSubnet"
                },
                new SubnetConfiguration
                {
                    CidrMask = 24,
                    SubnetType = SubnetType.PRIVATE_WITH_EGRESS,
                    Name = "BookstorePrivateSubnet"
                }
            }
        });

        // Create a Microsoft SQL Server database instance, on default port, and placed
        // into a private subnet so that it cannot be accessed from the public internet
        var db = new DatabaseInstance(this, "BookstoreSqlDb", new DatabaseInstanceProps
        {
            Vpc = vpc,
            VpcSubnets = new SubnetSelection
            {
                // Allow the RDS database egress-only access to the internet; we could also
                // use an Isolated subnet, which would block internet access at both ingress
                // and egress levels.
                SubnetType = SubnetType.PRIVATE_WITH_EGRESS
            },
            // SQL Server 2017 Express Edition, in conjunction with a db.t2.micro instance type,
            // fits inside the free tier for new accounts
            Engine = DatabaseInstanceEngine.SqlServerEx(new SqlServerExInstanceEngineProps
            {
                Version = SqlServerEngineVersion.VER_14
            }),
            InstanceType = InstanceType.Of(InstanceClass.BURSTABLE2, InstanceSize.MICRO),

            InstanceIdentifier = bookstoreDbInstance,

            // As this is a sample app, turn off automated backups to avoid any storage costs
            // of automated backup snapshots. It also helps the stack launch a little faster by
            // avoiding an initial backup.
            BackupRetention = Duration.Seconds(0)
        });

        // This secret, in Secrets Manager, holds the auto-generated database credentials. Note
        // that the secret will have a random string suffix; to enable the apps to recover the
        // data, we will later create a Systems Manager parameter, with fixed name, that points
        // at the secret in Secrets Manager.
        _ = new StringParameter(this, "BobsBookstoreDbSecret", new StringParameterProps
        {
            ParameterName = $"/{bookstoreDbInstance}/dbsecretsname",
            StringValue = db.Secret.SecretName
        });

        // A non-publicly accessible Amazon S3 bucket is used to store the cover
        // images for books. As this is a sample application, configure the bucket
        // to be deleted (EVEN IF IT CONTAINS DATA - BEWARE) when the stack is deleted
        // to avoid storage charges
        var bookstoreBucket = new Bucket(this, "BobsBookstoreBucket", new BucketProps
        {
            // !DO NOT USE THESE SETTINGS FOR PRODUCTION DEPLOYMENTS - YOU WILL LOSE DATA
            // WHEN THE STACK IS DELETED!
            AutoDeleteObjects = true,
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        // Access to the bucket is only granted to traffic coming a CloudFront distribution
        var cloudfrontOAI = new OriginAccessIdentity(this, "cloudfront-OAI");

        var policyProps = new PolicyStatementProps
        {
            Actions = new [] { "s3:GetObject" },
            Resources = new [] { bookstoreBucket.ArnForObjects("*") },
            Principals = new []
            {
                new CanonicalUserPrincipal
                (
                    cloudfrontOAI.CloudFrontOriginAccessIdentityS3CanonicalUserId
                )
            }
        };

        bookstoreBucket.AddToResourcePolicy(new PolicyStatement(policyProps));

        // Place a CloudFront distribution in front of the storage bucket. Access to objects
        // in the bucket will be made using the distribution url suffixed with the object's
        // key path only.
        var distProps = new CloudFrontWebDistributionProps
        {
            OriginConfigs = new []
            {
                new SourceConfiguration
                {
                    S3OriginSource = new S3OriginConfig
                    {
                        S3BucketSource = bookstoreBucket,
                        OriginAccessIdentity = cloudfrontOAI
                    },
                    Behaviors = new []
                    {
                        new Behavior
                        {
                            IsDefaultBehavior = true,
                            Compress = true,
                            AllowedMethods = CloudFrontAllowedMethods.GET_HEAD_OPTIONS
                        }
                    }
                }
            },
            // Require HTTPS between viewer and CloudFront; CloudFront to
            // origin (the bucket) will use HTTP but could also be set to require HTTPS
            ViewerProtocolPolicy = ViewerProtocolPolicy.REDIRECT_TO_HTTPS
        };

        var distribution = new CloudFrontWebDistribution(this, "SiteDistribution", distProps);

        // The customer user pool holds registrations on the customer-facing website
        var userPoolCustomer = new UserPool(this, "BobsBookstoreCustomerUserPool", new UserPoolProps
        {
            UserPoolName = customerUserPoolName,
            SelfSignUpEnabled = true,
            StandardAttributes = new StandardAttributes
            {
                Email = new StandardAttribute
                {
                    Required = true
                }
            },
            AutoVerify = new AutoVerifiedAttrs { Email = true },
            CustomAttributes = new Dictionary<string, ICustomAttribute>
            {
                { "AddressLine1", new StringAttribute() },
                { "AddressLine2", new StringAttribute() },
                { "City", new StringAttribute() },
                { "State", new StringAttribute() },
                { "Country", new StringAttribute() },
                { "ZipCode", new StringAttribute() }
            }
        });

        // The pool client will allow our front-end to permit user registration.
        var userPoolCustomerClient
            = userPoolCustomer.AddClient("BobsBookstorePoolCustomerClient", new UserPoolClientProps
            {
                UserPool = userPoolCustomer,
                GenerateSecret = false,
                ReadAttributes = new ClientAttributes()
                    .WithCustomAttributes("AddressLine1", "AddressLine2", "City", "State", "Country", "ZipCode")
                    .WithStandardAttributes(new StandardAttributesMask
                    {
                        Address = true,
                        PhoneNumber = true,
                        Email = true
                    })
            });

        // The admin site, used by bookstore staff, has its own user pool
        var userPoolAdmin = new UserPool(this, "BobsBookstoreAdminUserPool", new UserPoolProps
        {
            UserPoolName = adminUserPoolName,
            SelfSignUpEnabled = false,
            StandardAttributes = new StandardAttributes
            {
                Email = new StandardAttribute
                {
                    Required = true
                }
            },
            AutoVerify = new AutoVerifiedAttrs { Email = true }
        });

        var userPoolAdminClient = userPoolAdmin.AddClient("BobsBookstorePoolAdminClient", new UserPoolClientProps
        {
            UserPool = userPoolAdmin,
            GenerateSecret = false
        });

        // Create an application role for the admin website, seeded with the Systems Manager
        // permissions allowing future management from Systems Manager and remote access
        // from the console. Also add the CodeDeploy service role allowing deployments through
        // CodeDeploy if we wish. The trust relationship to EC2 enables the running application
        // to obtain temporary, auto-rotating credentials for calls to service APIs made by the
        // AWS SDK for .NET, without needing to place credentials onto the compute host.
        var adminAppRole = new Role(this, "AdminBookstoreApplicationRole", new RoleProps
        {
            AssumedBy = new ServicePrincipal("ec2.amazonaws.com"),
            ManagedPolicies = new[]
            {
                ManagedPolicy.FromAwsManagedPolicyName("AmazonSSMManagedInstanceCore"),
                ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSCodeDeployRole")
            }
        });

        // Access to read parameters by path is not in the AmazonSSMManagedInstanceCore
        // managed policy
        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] { "ssm:GetParametersByPath" },
            Resources = new[]
            {
                Arn.Format(new ArnComponents
                {
                    Service = "ssm",
                    Resource = "parameter",
                    ResourceName = $"{bookstoreDbInstance}/*"
                }, this),

                Arn.Format(new ArnComponents
                {
                    Service = "ssm",
                    Resource = "parameter",
                    ResourceName = $"{adminParameterNameRoot}/*"
                }, this)
            }
        }));

        // Provide permission to the app to allow access to Cognito user pools
        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
                    ResourceName = userPoolAdmin.UserPoolId
                }, this)
            }
        }));

        // Provide permission to allow access to Amazon Rekognition for processing uploaded
        // book images. Credentials for the calls will be provided courtesy of the application
        // role defined above, and the trust relationship with EC2.
        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new [] { "rekognition:DetectModerationLabels" },
            Resources = new [] { "*" }
        }));

        // Add permissions for the app to retrieve the database password in Secrets Manager
        db.Secret.GrantRead(adminAppRole);

        // Add permissions to the app to access the S3 bucket
        bookstoreBucket.GrantReadWrite(adminAppRole);

        // Create an Amazon CloudWatch log group for the admin website
        _ = new CfnLogGroup(this, "BobsBookstoreAdminLogGroup", new CfnLogGroupProps
        {
            LogGroupName = adminLogGroupName
        });

        // Add permissions to write logs
        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new [] {
                "logs:DescribeLogGroups",
                "logs:CreateLogGroup",
                "logs:CreateLogStream"
            },
            Resources = new [] {
                "arn:aws:logs:*:*:log-group:*"
            }
        }));

        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new [] {
                "logs:PutLogEvents",
            },
            Resources = new [] {
                "arn:aws:logs:*:*:log-group:*:log-stream:*",
            }
        }));

        // Add permissions to send email via Amazon SES's API
        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new [] {
                "ses:SendEmail"
            },
            Resources = new [] {
                "*"
            },
            Conditions = new Dictionary<string, object>
            {
                {"StringEquals",
                new Dictionary<string, object> {{ "ses:ApiVersion", "2" } }
            }
        }
        }));

        // As with the admin website, create an application role scoping permissions for service
        // API calls and resources needed by the customer-facing website, and providing temporary
        // credentials via a trust relationship
        var customerAppRole = new Role(this, "CustomerBookstoreApplicationRole", new RoleProps
        {
            AssumedBy = new ServicePrincipal("ec2.amazonaws.com"),
            ManagedPolicies = new []
            {
                ManagedPolicy.FromAwsManagedPolicyName("AmazonSSMManagedInstanceCore"),
                ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSCodeDeployRole")
            }
        });

        // Access to read parameters by path is not in the AmazonSSMManagedInstanceCore
        // managed policy
        customerAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new [] { "ssm:GetParametersByPath" },
            Resources = new []
            {
                Arn.Format(new ArnComponents
                {
                    Service = "ssm",
                    Resource = "parameter",
                    ResourceName = $"{bookstoreDbInstance}/*"
                }, this),

                Arn.Format(new ArnComponents
                {
                    Service = "ssm",
                    Resource = "parameter",
                    ResourceName = $"{customerParameterNameRoot}/*"
                }, this)
            }
        }));

        // Provide permission to the app to allow access to Cognito user pools
        customerAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new []
            {
                "cognito-idp:AdminListGroupsForUser",
                "cognito-idp:AdminGetUser",
                "cognito-idp:DescribeUserPool",
                "cognito-idp:DescribeUserPoolClient",
                "cognito-idp:ListUsers"
            },

            Resources = new []
            {
                Arn.Format(new ArnComponents
                {
                    Service = "cognito-idp",
                    Resource = "userpool",
                    ResourceName = userPoolCustomer.UserPoolId
                }, this)
            }
        }));

        // Add permissions for the app to retrieve the database password in Secrets Manager
        db.Secret.GrantRead(customerAppRole);

        // Add permissions to the app to access the S3 bucket
        bookstoreBucket.GrantReadWrite(customerAppRole);

        // Create a separate log group for the customer site
        _ = new CfnLogGroup(this, "BobsBookstoreCustomerLogGroup", new CfnLogGroupProps
        {
            LogGroupName = customerLogGroupName
        });

        // Add permissions to write logs
        customerAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new [] {
                "logs:DescribeLogGroups",
                "logs:CreateLogGroup",
                "logs:PutLogEvents",
                "logs:CreateLogStream"
            },
            Resources = new [] {
                $"arn:aws:logs:*:*:log-group:*",
            }
        }));

        customerAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new [] {
                "logs:PutLogEvents",
            },
            Resources = new [] {
                $"arn:aws:logs:*:*:log-group:*:log-stream:*",
            }
        }));

        // Create an instance profile wrapping the role, which can be used later when the app
        // is deployed to Elastic Beanstalk or EC2 compute hosts
        _ = new CfnInstanceProfile(this, "AdminBobsBookstoreInstanceProfile", new CfnInstanceProfileProps
        {
            Roles = new[] { adminAppRole.RoleName}
        });
        _ = new CfnInstanceProfile(this, "CustomerBobsBookstoreInstanceProfile", new CfnInstanceProfileProps
        {
            Roles = new[] { customerAppRole.RoleName}
        });

        // Store data such as the CloudFront distribution domain, S3 bucket name, and user pools
        // for the web apps in Systems Manager Parameter Store, using a */AWS/* format key path.
        // This will allow us to read all the parameters, in one pass, in the customer-facing and
        // admin app using the AWS-DotNet-Extensions-Configuration package
        // (https://github.com/aws/aws-dotnet-extensions-configuration). This package reads all
        // parameters beneath a parameter key root and injects them into the app's configuration at
        // runtime, just as if they were statically held in appsettings.json. This is why the
        // application roles defined above need permissions to call the ssm:GetParametersByPath
        // action.
        _ = new []
        {
            new(this, "CDN", new StringParameterProps
            {
                ParameterName = $"/{adminParameterNameRoot}/AWS/CloudFrontDomain",
                StringValue = $"https://{distribution.DistributionDomainName}"
            }),

            new StringParameter(this, "S3BucketName", new StringParameterProps
            {
                ParameterName = $"/{adminParameterNameRoot}/AWS/BucketName",
                StringValue = bookstoreBucket.BucketName
            }),

            new StringParameter(this, "BookstoreUserPoolAdminId", new StringParameterProps
            {
                ParameterName = $"/{adminParameterNameRoot}/AWS/UserPoolId",
                StringValue = userPoolAdmin.UserPoolId
            }),

            new StringParameter(this, "BookstoreUserPoolAdminClientId", new StringParameterProps
            {
                ParameterName = $"/{adminParameterNameRoot}/AWS/UserPoolClientId",
                StringValue = userPoolAdminClient.UserPoolClientId
            }),

            new StringParameter(this, "BookstoreUserPoolCustomerId", new StringParameterProps
            {
                ParameterName = $"/{customerParameterNameRoot}/AWS/UserPoolId",
                StringValue = userPoolCustomer.UserPoolId
            }),

            new StringParameter(this, "BookstoreUserPoolCustomerClientId", new StringParameterProps
            {
                ParameterName = $"/{customerParameterNameRoot}/AWS/UserPoolClientId",
                StringValue = userPoolCustomerClient.UserPoolClientId
            })
        };
    }
}
