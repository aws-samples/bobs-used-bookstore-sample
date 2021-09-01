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
        const string vpcResource = "BookstoreVpcVersion27";
        const string dbResource = "BookstoreCdkDBVersion27";
        const string roleResource = "BookstoreCdkInstanceRoleVersion27";
        const string subnetResource1 = "BookstoreVpcVersion27CdkPublic1";
        const string subnetResource2 = "BookstoreVpcVersion27CdkPrivate1";
        const string customerParameterNameRoot = "BobsUsedBookCustomerStoreVersion27";
        const string adminParameterNameRoot = "BobsUsedBookAdminStoreVersion27";
        const string dbParameter = "bookstoredb";
        const string userPoolCustomerResource = "BobsBookstoreCustomerUsersVersion27";
        const string userPoolAdminResource = "BobsBookstoreAdminUsersVersion27";

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
            //Create a Microsoft SQL Server database instance
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

            //Create a secret in Secrets Manager storing the database credentials
            new StringParameter(this, "BookCdkVersion27", new StringParameterProps
            {
                ParameterName = $"/{dbParameter}/dbsecretsname",
                StringValue = db.Secret.SecretName
            });

            //Create a S3 bucket to store the book covers
            var bookstoreBucket = new Bucket(this, "BookstoreBucketVersion27");

            // Grant access to CloudFront to access the bucket
            var cloudfrontOAI = new OriginAccessIdentity(this, "cloudfront-OAIVersion27");

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
                = new CloudFrontWebDistribution(this, "SiteDistributionVersion27", distProps);



            // The user pool is where user registrations on the site will be held.
            //Create user pool for Customer appplication
            var userPoolCustomer = new UserPool(this, "BookstoreCustomerUserPoolVersion27", new UserPoolProps
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
            
            var userPoolCustomerClient = userPoolCustomer.AddClient("BookstorePoolCustomerClientVersion27", new UserPoolClientProps
            {
                GenerateSecret = false,
                ReadAttributes = new ClientAttributes().WithCustomAttributes(new string[] { "AddressLine1", "AddressLine2", "City", "State", "Country", "ZipCode" }).WithStandardAttributes(new StandardAttributesMask()
                {
                    Address = true,
                    PhoneNumber = true,
                    Email = true
                }),

            });

            //Create user pool for Admin appplication
            var userPoolAdmin = new UserPool(this, "BookstoreAdminUserPoolVersion27", new UserPoolProps
            {
                UserPoolName = userPoolAdminResource,
                SelfSignUpEnabled = false,
                StandardAttributes = new StandardAttributes
                {
                    Email = new StandardAttribute
                    {
                        Required = true
                    }
                },
                AutoVerify = new AutoVerifiedAttrs { Email = true },

            });

            var userPoolAdminClient = userPoolAdmin.AddClient("BookstorePoolAdminClientVersion27", new UserPoolClientProps
            {
                GenerateSecret = false,
                
            });
           
           //Store the user pool details in Parameter store
            var ssmCustomerParameters = new StringParameter[]
            {
                new StringParameter(this, "BookstoreUserPoolCustomerIdVersion27", new StringParameterProps
                {
                    ParameterName = $"/{customerParameterNameRoot}/AWS/UserPoolId",
                    StringValue = userPoolCustomer.UserPoolId
                }),

                new StringParameter(this, "BookstoreUserPoolCustomerClientIdVersion27", new StringParameterProps
                {
                    ParameterName = $"/{customerParameterNameRoot}/AWS/UserPoolClientId",
                    StringValue = userPoolCustomerClient.UserPoolClientId
                }),
            };

            var ssmAdminParameters = new StringParameter[]
            {
                new StringParameter(this, "BookstoreUserPoolAdminIdVersion27", new StringParameterProps
                {
                    ParameterName = $"/{adminParameterNameRoot}/AWS/UserPoolId",
                    StringValue = userPoolAdmin.UserPoolId
                }),

                new StringParameter(this, "BookstoreUserPoolAdminClientIdVersion27", new StringParameterProps
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

            //Provide permission to allow access to Cognito user pools
            appRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps {
                Effect = Effect.ALLOW,
                Actions = new string[] { "cognito-idp:AdminListGroupsForUser", "cognito-idp:AdminGetUser", "cognito-idp:DescribeUserPool", "cognito-idp:DescribeUserPoolClient" },

                Resources = new string[]
                {
                    Arn.Format(new ArnComponents
                    {
                        Service = "cognito-idp",
                        Resource = "userpool",
                        ResourceName = userPoolAdmin.UserPoolId
                    }, this),
                      Arn.Format(new ArnComponents
                    {
                        Service = "cognito-idp",
                        Resource = "userpool",
                        ResourceName = userPoolCustomer.UserPoolId
                    }, this),

                }

            }));

            //Provide permission to allow access to Rekognition
            appRole.AddToPolicy(new PolicyStatement(new PolicyStatementProps
            {
                Effect = Effect.ALLOW,
                Actions = new string[] { "rekognition:DetectModerationLabels" },
                Resources = new string[] {"*"}
                    

            }));

            //Create an instance profile which is later used in Elastic Beanstalk deployment
            var instanceProfile = new CfnInstanceProfile(this, "BookstoreInstanceProfileVersion27", new CfnInstanceProfileProps
            {
                Roles = new string[] {appRole.RoleName}

            });

            //Store the cloudfront distribution domain in Parameter Store
            new StringParameter(this, "CDNVersion27", new StringParameterProps
               {
                 ParameterName = $"/{adminParameterNameRoot}/AWS/CloudFrontDomain",
                 StringValue = $"https://{distribution.DistributionDomainName}"
                 });

            //Store the S3 bucket name in Parameter Store
            new StringParameter(this, "S3BucketNameVersion27", new StringParameterProps
            {
                ParameterName = $"/{adminParameterNameRoot}/AWS/BucketName",
                StringValue = bookstoreBucket.BucketName
            });
            
             // the app also needs to retrieve the database password, etc, posted automatically
            // to Secrets Manager
            db.Secret.GrantRead(appRole);

            //Provide the app access to S3 bucket
            bookstoreBucket.GrantReadWrite(appRole);
        }
    }
}
