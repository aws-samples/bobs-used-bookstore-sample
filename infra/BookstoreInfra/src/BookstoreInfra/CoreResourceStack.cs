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

namespace BookstoreInfra;

// Defines the core resources that make up the BobsBookstore application. Resources
// used to deploy the front- and back-end applications are defined in separate
// stacks to allow targeting different compute options (Elastic Beanstalk, EC2, ECS, etc.)
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
            Cidr = "10.0.0.0/16",
            MaxAzs = 2,
            SubnetConfiguration = new[]
            {
                new()
                {
                    CidrMask = 24,
                    SubnetType = SubnetType.PUBLIC,
                    Name = "BookstorePublicSubnet"
                },
                new SubnetConfiguration
                {
                    CidrMask = 24,
                    SubnetType = SubnetType.PRIVATE_WITH_NAT,
                    Name = "BookstorePrivateSubnet"
                }
            }
        });

        // Create a Microsoft SQL Server database instance, on default port, and placed
        // into a private subnet
        var db = new DatabaseInstance(this, "BookstoreSqlDb", new DatabaseInstanceProps
        {
            Vpc = vpc,
            VpcSubnets = new SubnetSelection
            {
                SubnetType = SubnetType.PRIVATE_WITH_NAT
            },
            // SQL Server 2017 Express Edition, in conjunction with a db.t2.micro instance type,
            // fits inside the free tier for new accounts
            Engine = DatabaseInstanceEngine.SqlServerEx(new SqlServerExInstanceEngineProps
            {
                Version = SqlServerEngineVersion.VER_14
            }),
            InstanceType = InstanceType.Of(InstanceClass.BURSTABLE2, InstanceSize.MICRO),

            InstanceIdentifier = bookstoreDbInstance,

            // as this is a sample app, turn off automated backups to avoid any storage costs
            // of automated backup snapshots and it helps the stack launch a little faster by
            // avoiding an initial backup
            BackupRetention = Duration.Seconds(0)
        });

        // This secret, in Secrets Manager, holds the auto-generated database credentials
        new StringParameter(this, "BobsBookstoreDbSecret", new StringParameterProps
        {
            ParameterName = $"/{bookstoreDbInstance}/dbsecretsname",
            StringValue = db.Secret.SecretName
        });

        // An S3 bucket stores the book cover images
        var bookstoreBucket = new Bucket(this, "BobsBookstoreBucket");

        // Grant access to the bucket to CloudFront
        var cloudfrontOAI = new OriginAccessIdentity(this, "cloudfront-OAI");

        var policyProps = new PolicyStatementProps
        {
            Actions = new[] { "s3:GetObject" },
            Resources = new[] { bookstoreBucket.ArnForObjects("*") },
            Principals = new[]
            {
                new CanonicalUserPrincipal
                (
                    cloudfrontOAI.CloudFrontOriginAccessIdentityS3CanonicalUserId
                )
            }
        };

        bookstoreBucket.AddToResourcePolicy(new PolicyStatement(policyProps));

        // Front the bucket with a CloudFront distribution
        var distProps = new CloudFrontWebDistributionProps
        {
            OriginConfigs = new[]
            {
                new SourceConfiguration
                {
                    S3OriginSource = new S3OriginConfig
                    {
                        S3BucketSource = bookstoreBucket,
                        OriginAccessIdentity = cloudfrontOAI
                    },
                    Behaviors = new[]
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
            // origin will use HTTP but could also be set to require HTTPS
            ViewerProtocolPolicy = ViewerProtocolPolicy.REDIRECT_TO_HTTPS
        };

        var distribution = new CloudFrontWebDistribution(this, "SiteDistribution", distProps);

        // The customer user pool is where user registrations on the site will be held
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

        // The pool client will allow our front-end to permit user registration. Note that we cannot
        // recover the client secret here, so some post-processing or initial start-up code in the
        // application is needed to recover the secret and make it into a Parameter Store entry that
        // we can recover at runtime.
        var userPoolCustomerClient
            = userPoolCustomer.AddClient("BobsBookstorePoolCustomerClient", new UserPoolClientProps
            {
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

        // User pool for Bookstore staff
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
            GenerateSecret = false
        });

        // The app also needs to retrieve the database password, etc, posted automatically
        // to Secrets Manager, and, to enable deployment using CodeDeploy, add the relevant
        // service role
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

        // Provide permission to allow access to Rekognition for processing uploaded
        // book images
        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] { "rekognition:DetectModerationLabels" },
            Resources = new[] { "*" }
        }));

        // Add permissions for the app to retrieve the database password in Secrets Manager
        db.Secret.GrantRead(adminAppRole);

        // Add permissions to the app to access the S3 bucket
        bookstoreBucket.GrantReadWrite(adminAppRole);

        //Create the log group for Cloudwatch logs
        CfnLogGroup cfnAdminLogGroup = new CfnLogGroup(this, "BobsBookstoreAdminLogGroup", new CfnLogGroupProps
        {
            LogGroupName = adminLogGroupName

        });

        //add permissions to write logs
        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] {
                "logs:DescribeLogGroups",
                "logs:CreateLogGroup",
                "logs:CreateLogStream"
            },
            Resources = new[] {
                "arn:aws:logs:*:*:log-group:*"
               
            }
        }));

        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] {
                "logs:PutLogEvents",
            },
            Resources = new[] {
                "arn:aws:logs:*:*:log-group:*:log-stream:*",

            }
        }));
        //add permissions to send email
        adminAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] {
                "ses:SendEmail"
            },
            Resources = new[] {
                "*"
            },
            Conditions = new Dictionary<string, object>
            {
                {"StringEquals",
                new Dictionary<string, object> {{ "ses:ApiVersion", "2" } }
            }

        }
        }));
        

        var customerAppRole = new Role(this, "CustomerBookstoreApplicationRole", new RoleProps
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
        customerAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
                    ResourceName = $"{customerParameterNameRoot}/*"
                }, this)
            }
        }));

        // Provide permission to the app to allow access to Cognito user pools
        customerAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
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
                    ResourceName = userPoolCustomer.UserPoolId
                }, this)
            }
        }));

        // Add permissions for the app to retrieve the database password in Secrets Manager
        db.Secret.GrantRead(customerAppRole);

        // Add permissions to the app to access the S3 bucket
        bookstoreBucket.GrantReadWrite(customerAppRole);

        //Create the log group for Cloudwatch logs
        CfnLogGroup cfnCustomerLogGroup = new CfnLogGroup(this, "BobsBookstoreCustomerLogGroup", new CfnLogGroupProps
        {
            LogGroupName = customerLogGroupName

        });

        //add permissions to write logs
        customerAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] {
                "logs:DescribeLogGroups",
                "logs:CreateLogGroup",
                "logs:PutLogEvents",
                "logs:CreateLogStream"
            },
            Resources = new[] {
                $"arn:aws:logs:*:*:log-group:*",
              
            }
        }));
        customerAppRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
        {
            Effect = Effect.ALLOW,
            Actions = new[] {

                "logs:PutLogEvents",
            },
            Resources = new[] {
                $"arn:aws:logs:*:*:log-group:*:log-stream:*",
             
            }
        }));

        // Create an instance profile wrapping the role, which can be used later when the app
        // is deployed to Elastic Beanstalk or EC2 compute targets
        _ = new CfnInstanceProfile(this, "AdminBobsBookstoreInstanceProfile", new CfnInstanceProfileProps
        {
            Roles = new[] { adminAppRole.RoleName}
        });
        _ = new CfnInstanceProfile(this, "CustomerBobsBookstoreInstanceProfile", new CfnInstanceProfileProps
        {
            Roles = new[] { customerAppRole.RoleName}
        });

        // Store data such as the CloudFront distribution domain, S3 bucket name, and user pools
        // for the web apps in Parameter Store, using a */AWS/* format key path. This will
        // allow us to read all the parameters, in one pass, in the front-end and back-end apps
        // using the aws-dotnet-extensions-configuration package, which will add the values to the
        // app's configuration at runtime just as if they were statically held in appsettings.json.
        _ = new[]
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