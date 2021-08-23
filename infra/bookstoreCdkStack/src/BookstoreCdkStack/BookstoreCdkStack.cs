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
        const string vpcResource = "BookstoreVpcversion25";
        const string dbResource = "BookstoreCdkDBversion25";
        const string roleResource = "BookstoreCdkInstanceRoleversion25";
        const string subnetResource1 = "BookstoreVpcversion25CdkPublic1";
        const string subnetResource2 = "BookstoreVpcversion25CdkPrivate1";
        const string customerParameterNameRoot = "BobsUsedBookCustomerStoreversion25";
        const string adminParameterNameRoot = "BobsUsedBookAdminStoreversion25";
        const string dbParameter = "bookstorecdkversion25";
        const string userPoolCustomerResource = "BobsBookstoreCustomerUsersversion25";
        const string userPoolAdminResource = "BobsBookstoreAdminUsersversion25";

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

            new StringParameter(this, "BookCdkversion25", new StringParameterProps
            {
                ParameterName = $"/{dbParameter}/dbsecretsname",
                StringValue = db.Secret.SecretName
            });

            var bookstoreBucket = new Bucket(this, "BookstoreBucketversion25");
            // Grant access to CloudFront to access the bucket
            var cloudfrontOAI = new OriginAccessIdentity(this, "cloudfront-OAIversion25");

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
                = new CloudFrontWebDistribution(this, "SiteDistributionversion25", distProps);



            // The user pool is where user registrations on the site will be held.
            var userPoolCustomer = new UserPool(this, "BookstoreCustomerUserPoolversion25", new UserPoolProps
            {
                UserPoolName = userPoolCustomerResource,
                SelfSignUpEnabled = true,
                StandardAttributes = new StandardAttributes {
                Email= new StandardAttribute
                {
                    Required = true
                }
                },
                AutoVerify = new AutoVerifiedAttrs { Email=true},
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
            
                /*new ClientAttributes().WithCustomAttributes(new string[] { "AddressLine1" })
                /*, "AddressLine2", "City", "State", "Country", "ZipCode" }),*/

            var userPoolCustomerClient = userPoolCustomer.AddClient("BookstorePoolCustomerClientversion25", new UserPoolClientProps
            {
                GenerateSecret = false,
                ReadAttributes = new ClientAttributes().WithCustomAttributes(new string[] { "AddressLine1", "AddressLine2", "City", "State", "Country", "ZipCode" }).WithStandardAttributes(new StandardAttributesMask()
                {
                    Address = true,
                    PhoneNumber = true,
                    Email = true
                }),
                /*WriteAttributes = new ClientAttributes().WithCustomAttributes(new string[] { "AddressLine1", "AddressLine2", "City", "State", "Country", "ZipCode" })*/



            });

            var userPoolAdmin = new UserPool(this, "BookstoreAdminUserPoolversion25", new UserPoolProps
            {
                UserPoolName = userPoolAdminResource

            });

            var userPoolAdminClient = userPoolAdmin.AddClient("BookstorePoolAdminClientversion25", new UserPoolClientProps
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

            var userPoolDomain = new UserPoolDomain(this, "BookstorePoolDomainversion25", new UserPoolDomainProps
            {
                UserPool = userPoolAdmin,
                CognitoDomain = new CognitoDomainOptions
                {
                    DomainPrefix = "bobsbookstoreversion25"
                }
            });
           
            var ssmCustomerParameters = new StringParameter[]
            {
                new StringParameter(this, "BookstoreUserPoolCustomerIdversion25", new StringParameterProps
                {
                    ParameterName = $"/{customerParameterNameRoot}/AWS/UserPoolId",
                    StringValue = userPoolCustomer.UserPoolId
                }),

                new StringParameter(this, "BookstoreUserPoolCustomerClientIdversion25", new StringParameterProps
                {
                    ParameterName = $"/{customerParameterNameRoot}/AWS/UserPoolClientId",
                    StringValue = userPoolCustomerClient.UserPoolClientId
                }),
            };

            var ssmAdminParameters = new StringParameter[]
            {
                new StringParameter(this, "BookstoreUserPoolAdminIdversion25", new StringParameterProps
                {
                    ParameterName = $"/{adminParameterNameRoot}/AWS/UserPoolId",
                    StringValue = userPoolAdmin.UserPoolId
                }),

                new StringParameter(this, "BookstoreUserPoolAdminClientIdversion25", new StringParameterProps
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
                    }, this),

                    Arn.Format(new ArnComponents
                    {
                        Service = "ssm",
                        Resource = "parameter",
                        ResourceName = $"{adminParameterNameRoot}/*"
                    }, this),
                    Arn.Format(new ArnComponents
                    {
                        Service = "ssm",
                        Resource = "parameter",
                        ResourceName = $"{customerParameterNameRoot}/*"
                    }, this)
                }
            }));

            appRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps {
                Effect = Effect.ALLOW,
                Actions = new string[] { "cognito-idp:AdminListGroupsForUser", "cognito-idp:AdminGetUser", "cognito-idp:DescribeUserPool" },

                Resources = new string[]
                {
                    Arn.Format(new ArnComponents
                    {
                        Service = "cognito-idp",
                        Resource = "userpool",
                        ResourceName = userPoolAdmin.UserPoolId
                    }, this)

                }

            }));

            var instanceProfile = new CfnInstanceProfile(this, "BookstoreInstanceProfileversion25", new CfnInstanceProfileProps
            {
                Roles = new string[] {appRole.RoleName}

            });


                 new StringParameter(this, "CDNversion25", new StringParameterProps
                 {
                     ParameterName = $"/{adminParameterNameRoot}/AWS/CloudFrontDomain",
                     StringValue = $"https://{distribution.DistributionDomainName}"
                 });

            new StringParameter(this, "S3BucketNameversion25", new StringParameterProps
            {
                ParameterName = $"/{adminParameterNameRoot}/AWS/BucketName",
                StringValue = bookstoreBucket.BucketName
            });
            new StringParameter(this, "DomainNameversion25", new StringParameterProps
            {
                ParameterName = $"/{adminParameterNameRoot}/AWS/DomainName",
                StringValue = userPoolDomain.DomainName
            });

            // the app also needs to retrieve the database password, etc, posted automatically
            // to Secrets Manager
            db.Secret.GrantRead(appRole);
            bookstoreBucket.GrantReadWrite(appRole);
        }
    }
}
