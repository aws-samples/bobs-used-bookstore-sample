using Amazon.CDK;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SSM;
using Amazon.CDK.AWS.CloudFront.Origins;
using System.Collections.Generic;

namespace BookstoreCdkStack
{
    public class BookstoreCdkStack : Stack
    {
        const double dbPort = 1433;
        const string vpcResource = "BookstoreVpcVersion7";
        const string dbResource = "BookstoreCdkDBVersion7";
        const string roleResource = "BookstoreCdkInstanceRoleVersion7";
        const string subnetResource1 = "BookstoreVpcVersion7CdkPublic1";
        const string subnetResource2 = "BookstoreVpcVersion7CdkPrivate1";
        const string customerParameterNameRoot = "BobsUsedBookCustomerStoreVersion7";
        const string adminParameterNameRoot = "BobsUsedBookAdminStoreVersion7";
        const string dbParameter = "bookstorecdkVersion7";
        const string userPoolCustomerResource = "BobsBookstoreCustomerUsersVersion7";
        const string userPoolAdminResource = "BobsBookstoreAdminUsersVersion7";

        internal BookstoreCdkStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // SQL Server
            // Create a new vpc spanning two AZs and with public and private subnets
            // to host the application resources
            var vpc = new Vpc(this, vpcResource, new VpcProps
            {
                Cidr = "10.0.0.0/16",
                MaxAzs = 2,
                SubnetConfiguration = new SubnetConfiguration[]
                {
                    new SubnetConfiguration
                    {
                        CidrMask = 24,
                        SubnetType = SubnetType.PUBLIC,
                        Name = subnetResource1
                    },
                    new SubnetConfiguration
                    {
                        CidrMask = 24,
                        SubnetType = SubnetType.PRIVATE,
                        Name = subnetResource2
                    }

                }
            }) ;

            var db = new DatabaseInstance(this, dbResource, new DatabaseInstanceProps
            {
                Vpc = vpc,
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.PUBLIC
                },
                // SQL Server 2017 Express Edition, in conjunction with a db.t2.micro instance type,
                // fits inside the free tier for new accounts
                Engine = DatabaseInstanceEngine.SqlServerEx(new SqlServerExInstanceEngineProps
                {
                    Version = SqlServerEngineVersion.VER_14
                }),
                InstanceType = InstanceType.Of(InstanceClass.BURSTABLE2, InstanceSize.MICRO),
                Port = dbPort,

                InstanceIdentifier = dbParameter,
                // turn off automated backups so that this sample launches a little faster by
                // avoiding an initial backup :-)
                BackupRetention = Duration.Seconds(0)
            });

            new StringParameter(this, "BookCdkVersion7", new StringParameterProps
            {
                ParameterName = $"/{dbParameter}/dbsecretsname",
                StringValue = db.Secret.SecretName
            });

            var bookstoreBucket = new Bucket(this, "BookstoreBucketVersion7");
            // Grant access to CloudFront to access the bucket
            var cloudfrontOAI = new OriginAccessIdentity(this, "cloudfront-OAIVersion7");

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

            // CloudFront distribution fronting the bucket.
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
                        Behaviors = new []
                        {
                            new Behavior
                            {
                                IsDefaultBehavior = true,
                                Compress = true,
                                AllowedMethods
                                    = CloudFrontAllowedMethods.GET_HEAD_OPTIONS
                            }
                        }
                    }
                },
                // Require HTTPS between viewer and CloudFront; CloudFront to
                // origin will use HTTP but could also be set to require HTTPS
                ViewerProtocolPolicy = ViewerProtocolPolicy.REDIRECT_TO_HTTPS
            };

            var distribution
                = new CloudFrontWebDistribution(this, "SiteDistributionVersion7", distProps);



            // The user pool is where user registrations on the site will be held.
            var userPoolCustomer = new UserPool(this, "BookstoreCustomerUserPoolVersion7", new UserPoolProps
            {
                UserPoolName = userPoolCustomerResource,
                CustomAttributes =  new Dictionary<string, ICustomAttribute>{
                    {"AddressLine1", new StringAttribute()},
                    {"AddressLine2", new StringAttribute() },
                    {"City", new StringAttribute()},
                    {"State", new StringAttribute()},
                    {"Country", new StringAttribute() },
                    {"ZipCode", new StringAttribute() }
                }

            });
            // The pool client will allow our front-end to permit user registration. Note that we cannot
            // recover the client secret here, so some post-processing or initial start-up code in the
            // application is needed to recover the secret and make it into a Parameter Store entry that
            // we can recover at runtime.


            var userPoolCustomerClient = userPoolCustomer.AddClient("BookstorePoolCustomerClientVersion7", new UserPoolClientProps
            {
                GenerateSecret = false,
            });

            var userPoolAdmin = new UserPool(this, "BookstoreAdminUserPoolVersion7", new UserPoolProps
            {
                UserPoolName = userPoolAdminResource

            });

            var userPoolAdminClient = userPoolAdmin.AddClient("BookstorePoolAdminClientVersion7", new UserPoolClientProps
            {
                GenerateSecret = false,
                SupportedIdentityProviders = new UserPoolClientIdentityProvider[] {UserPoolClientIdentityProvider.COGNITO},
                OAuth = new OAuthSettings
                {
                    CallbackUrls = new string[] { "https://bobsbackend-dev.us-east-1.elasticbeanstalk.com/signin-oidc", "https://localhost:44336/Home/lndex" },
                    LogoutUrls = new string[] { "https://localhost:44336/Home/Index", "https://localhost:5001/Home/Index"},
                    Flows = new OAuthFlows
                    {
                        AuthorizationCodeGrant = true
                    },
                    Scopes = new OAuthScope[] {OAuthScope.EMAIL,OAuthScope.OPENID,OAuthScope.PROFILE}
                    
                }

            });

            var ssmCustomerParameters = new StringParameter[]
            {
                new StringParameter(this, "BookstoreUserPoolCustomerIdVersion7", new StringParameterProps
                {
                    ParameterName = $"/{customerParameterNameRoot}/AWS/UserPoolId",
                    StringValue = userPoolCustomer.UserPoolId
                }),

                new StringParameter(this, "BookstoreUserPoolCustomerClientIdVersion7", new StringParameterProps
                {
                    ParameterName = $"/{customerParameterNameRoot}/AWS/UserPoolClientId",
                    StringValue = userPoolCustomerClient.UserPoolClientId
                }),
            };

            var ssmAdminParameters = new StringParameter[]
            {
                new StringParameter(this, "BookstoreUserPoolAdminIdVersion7", new StringParameterProps
                {
                    ParameterName = $"/{adminParameterNameRoot}/AWS/UserPoolId",
                    StringValue = userPoolAdmin.UserPoolId
                }),

                new StringParameter(this, "BookstoreUserPoolAdminClientIdVersion7", new StringParameterProps
                {
                    ParameterName = $"/{adminParameterNameRoot}/AWS/UserPoolClientId",
                    StringValue = userPoolAdminClient.UserPoolClientId
                }),
            };
            // the app also needs to retrieve the database password, etc, posted automatically
            // to Secrets Manager
            var appRole = new Role(this, roleResource, new RoleProps
            {
                AssumedBy = new ServicePrincipal("ec2.amazonaws.com"),
                ManagedPolicies = new IManagedPolicy[]
                 {
                    ManagedPolicy.FromAwsManagedPolicyName("AmazonSSMManagedInstanceCore"),
                    ManagedPolicy.FromAwsManagedPolicyName("service-role/AWSCodeDeployRole")
                 }
            });

            // access to read parameters by path is not in the AmazonSSMManagedInstanceCore
            appRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
            {
                Effect = Effect.ALLOW,
                Actions = new string[] { "ssm:GetParametersByPath" },
                Resources = new string[]
                {
                    Arn.Format(new ArnComponents
                    {
                        Service = "ssm",
                        Resource = "parameter",
                        ResourceName = $"{dbParameter}/*"
                    }, this)
                }
            }));

            
                 new StringParameter(this, "CDNVersion7", new StringParameterProps
                 {
                     ParameterName = $"/{adminParameterNameRoot}/AWS/CloudFrontDomain",
                     StringValue = $"https://{distribution.DistributionDomainName}"
                 });

            new StringParameter(this, "S3BucketNameVersion7", new StringParameterProps
            {
                ParameterName = $"/{adminParameterNameRoot}/AWS/BucketName",
                StringValue = bookstoreBucket.BucketName
            });

            // the app also needs to retrieve the database password, etc, posted automatically
            // to Secrets Manager
            db.Secret.GrantRead(appRole);
            bookstoreBucket.GrantReadWrite(appRole);
        }
    }
}
