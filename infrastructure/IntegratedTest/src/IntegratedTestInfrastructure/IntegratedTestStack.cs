using System.Collections.Generic;
using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SSM;
using Amazon.CDK.AWS.Logs;

namespace IntegratedTestInfrastructure;

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
public class IntegratedTestStack : Stack
{
    // -Test- in the parameter key roots maps to the ASP.NET Core environment in use
    // when using the "AdminSite Integrated" or "CustomerSite Integrated" launch profiles
    private const string customerSiteParametersRoot = "BobsUsedBooks-Test-CustomerSite";
    private const string adminSiteParametersRoot = "BobsUsedBooks-Test-AdminSite";

    private const string customerSiteUserPoolName = "BobsUsedBooks-Test-CustomerPool";
    private const string customerSiteLogGroupName = "BobsBooks-Test-CustomerSiteLogs";
    private const string customerSiteUserPoolCallbackUrlRoot = "https://localhost:4001";

    private const string adminSiteUserPoolName = "BobsBooks-Test-AdminPool";
    private const string adminSiteLogGroupName = "BobsBooks-Test-AdminSiteLogs";
    private const string adminSiteUserPoolCallbackUrlRoot = "https://localhost:5000";

    internal IntegratedTestStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        //=========================================================================================
        // A non-publicly accessible Amazon S3 bucket is used to store the cover
        // images for books.
        //
        // NOTE: As this is a sample application the bucket is configured to be deleted when
        // the stack is deleted to avoid charges on an unused resource - EVEN IF IT CONTAINS DATA
        // - BEWARE!
        //
        var bookstoreBucket = new Bucket(this, "CoverImages-Bucket", new BucketProps
        {
            // !DO NOT USE THESE TWO SETTINGS FOR PRODUCTION DEPLOYMENTS - YOU WILL LOSE DATA
            // WHEN THE STACK IS DELETED!
            AutoDeleteObjects = true,
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        _ = new StringParameter(this, "CoverImages-BucketName", new StringParameterProps
        {
            ParameterName = $"/{adminSiteParametersRoot}/AWS/BucketName",
            StringValue = bookstoreBucket.BucketName
        });

        //=========================================================================================
        // Access to the bucket is only granted to traffic coming from a CloudFront distribution
        //
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

        // Place a CloudFront distribution in front of the storage bucket. S3 will only respond to
        // requests for objects if that request came from the CloudFront distribution.
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

        _ = new StringParameter(this, "CoverImages-Distribution", new StringParameterProps
        {
            ParameterName = $"/{adminSiteParametersRoot}/AWS/CloudFrontDomain",
            StringValue = $"https://{distribution.DistributionDomainName}"
        });

        //=========================================================================================
        // Configure a Cognito user pool for the customer web site, to hold customer registrations.
        //
        var customerUserPool = new UserPool(this, "CustomerSiteUserPool", new UserPoolProps
        {
            UserPoolName = customerSiteUserPoolName,
            SelfSignUpEnabled = true,
            StandardAttributes = new StandardAttributes
            {
                Email = new StandardAttribute      { Required = true },
                FamilyName = new StandardAttribute { Required = true },
                GivenName = new StandardAttribute  { Required = true }
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

        // The pool client controls user registration and sign-in from the customer-facing website.
        // The pool will use Cognito's Hosted UI for sign-in, which requires a HTTPS callback url.
        var customerUserPoolClient
            = customerUserPool.AddClient("CustomerSiteUserPoolClient", new UserPoolClientProps
            {
                UserPool = customerUserPool,
                PreventUserExistenceErrors = true,
                GenerateSecret = false,
                ReadAttributes = new ClientAttributes()
                    .WithCustomAttributes("AddressLine1", "AddressLine2", "City", "State", "Country", "ZipCode")
                    .WithStandardAttributes(new StandardAttributesMask
                    {
                        GivenName = true,
                        FamilyName = true,
                        Address = true,
                        PhoneNumber = true,
                        Email = true
                    }),
                SupportedIdentityProviders = new []
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
                    Scopes = new []
                    {
                        OAuthScope.OPENID,
                        OAuthScope.EMAIL,
                        OAuthScope.PHONE,
                        OAuthScope.PROFILE,
                        OAuthScope.COGNITO_ADMIN
                    },
                    CallbackUrls = new []
                    {
                        $"{customerSiteUserPoolCallbackUrlRoot}/signin-oidc"
                    },
                    LogoutUrls = new []
                    {
                        $"{customerSiteUserPoolCallbackUrlRoot}/"
                    }
                }
            });

        var customerUserPoolDomain = customerUserPool.AddDomain("CustomerSiteUserPoolDomain", new UserPoolDomainOptions
        {
            CognitoDomain = new CognitoDomainOptions
            {
                // The prefix must be unique across the AWS Region in which the pool is created
                DomainPrefix = $"bobs-bookstore-test-{Account}"
            }
        });

        customerUserPoolDomain.SignInUrl(customerUserPoolClient, new SignInUrlOptions
        {
            RedirectUri = $"{customerSiteUserPoolCallbackUrlRoot}/signin-oidc"
        });

        _ = new []
        {
            new StringParameter(this, "CustomerSiteUserPoolClientId", new StringParameterProps
            {
                ParameterName = $"/{customerSiteParametersRoot}/Authentication/Cognito/ClientId",
                StringValue = customerUserPoolClient.UserPoolClientId
            }),

            new StringParameter(this, "CustomerSiteUserPoolMetadataAddress", new StringParameterProps
            {
                ParameterName = $"/{customerSiteParametersRoot}/Authentication/Cognito/MetadataAddress",
                StringValue = $"https://cognito-idp.{Region}.amazonaws.com/{customerUserPool.UserPoolId}/.well-known/openid-configuration"
            }),

            new StringParameter(this, "CustomerSiteUserPoolCognitoDomain", new StringParameterProps
            {
                ParameterName = $"/{customerSiteParametersRoot}/Authentication/Cognito/CognitoDomain",
                StringValue = customerUserPoolDomain.BaseUrl()
            })
        };

        //=========================================================================================
        // The admin site, used by bookstore staff, has its own user pool
        //
        var adminSiteUserPool = new UserPool(this, "AdminSiteUserPool", new UserPoolProps
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
            AutoVerify = new AutoVerifiedAttrs { Email = true }
        });

        // As with the customer pool, the admin pool uses Hosted UI and so requires a HTTPS callback url
        var adminSiteUserPoolClient = adminSiteUserPool.AddClient("AdminSiteUserPoolClient", new UserPoolClientProps
        {
            UserPool = adminSiteUserPool,
            GenerateSecret = false,
            PreventUserExistenceErrors = true,
            SupportedIdentityProviders = new []
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
                Scopes = new []
                {
                    OAuthScope.OPENID,
                    OAuthScope.EMAIL,
                    OAuthScope.COGNITO_ADMIN,
                    OAuthScope.PROFILE
                },
                CallbackUrls = new []
                {
                    $"{adminSiteUserPoolCallbackUrlRoot}/signin-oidc"
                },
                LogoutUrls = new []
                {
                    $"{adminSiteUserPoolCallbackUrlRoot}/"
                }
            }
        });

        var adminSiteUserPoolDomain = adminSiteUserPool.AddDomain("AdminSiteUserPoolDomain", new UserPoolDomainOptions
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

        _ = new []
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

        // Create an Amazon CloudWatch log group for the admin website
        _ = new CfnLogGroup(this, "AdminSiteLogGroup", new CfnLogGroupProps
        {
            LogGroupName = adminSiteLogGroupName
        });

        // Create a separate log group for the customer site
        _ = new CfnLogGroup(this, "CustomerSiteLogGroup", new CfnLogGroupProps
        {
            LogGroupName = customerSiteLogGroupName
        });
    }
}
