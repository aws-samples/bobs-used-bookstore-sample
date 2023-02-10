using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SSM;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.CustomResources;
using System.Collections.Generic;

namespace SharedInfrastructure.Production;

public class CoreStack : Stack
{
    private const string userPoolCallbackUrlRoot = "https://localhost:5000";

    public Bucket ImageBucket { get; private set; }

    public UserPool WebAppUserPool { get; private set; }

    private CfnUserPoolGroup CognitoAdminUserGroup;

    internal CoreStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        CreateImageS3Bucket();
        CreateCloudFrontDistribution();
        CreateCognitoUserPool();
        CreateCognitoAdministratorsUserGroup();
        CreateDefaultAdminUser();
        CreateUserPoolClient();
    }

    internal void CreateImageS3Bucket()
    {
        //=========================================================================================
        // A non-publicly accessible Amazon S3 bucket is used to store the cover
        // images for books.
        //
        // NOTE: As this is a sample application the bucket is configured to be deleted when
        // the stack is deleted to avoid charges on an unused resource - EVEN IF IT CONTAINS DATA
        // - BEWARE!
        //
        ImageBucket = new Bucket(this, "CoverImages-Bucket", new BucketProps
        {
            // !DO NOT USE THESE TWO SETTINGS FOR PRODUCTION DEPLOYMENTS - YOU WILL LOSE DATA
            // WHEN THE STACK IS DELETED!
            AutoDeleteObjects = true,
            RemovalPolicy = RemovalPolicy.DESTROY
        });

        _ = new StringParameter(this, "CoverImages-BucketName", new StringParameterProps
        {
            ParameterName = $"/{Constants.AppName}/AWS/BucketName",
            StringValue = ImageBucket.BucketName
        });
    }

    internal void CreateCloudFrontDistribution()
    {
        //=========================================================================================
        // Access to the bucket is only granted to traffic coming from a CloudFront distribution
        //
        var cloudfrontOAI = new OriginAccessIdentity(this, "cloudfront-OAI");

        var policyProps = new PolicyStatementProps
        {
            Actions = new[] { "s3:GetObject" },
            Resources = new[] { ImageBucket.ArnForObjects("*") },
            Principals = new[]
            {
                new CanonicalUserPrincipal
                (
                    cloudfrontOAI.CloudFrontOriginAccessIdentityS3CanonicalUserId
                )
            }
        };

        ImageBucket.AddToResourcePolicy(new PolicyStatement(policyProps));

        // Place a CloudFront distribution in front of the storage bucket. S3 will only respond to
        // requests for objects if that request came from the CloudFront distribution.
        var distProps = new CloudFrontWebDistributionProps
        {
            OriginConfigs = new[]
            {
                new SourceConfiguration
                {
                    S3OriginSource = new S3OriginConfig
                    {
                        S3BucketSource = ImageBucket,
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
            ParameterName = $"/{Constants.AppName}/AWS/CloudFrontDomain",
            StringValue = $"https://{distribution.DistributionDomainName}"
        });
    }

    internal void CreateCognitoUserPool()
    {
        WebAppUserPool = new UserPool(this, $"{Constants.AppName}UserPool", new UserPoolProps
        {
            UserPoolName = Constants.AppName,
            SelfSignUpEnabled = true,
            StandardAttributes = new StandardAttributes
            {
                Email = new StandardAttribute { Required = true },
                FamilyName = new StandardAttribute { Required = true },
                GivenName = new StandardAttribute { Required = true }
            },
            AutoVerify = new AutoVerifiedAttrs { Email = true },
            RemovalPolicy = RemovalPolicy.DESTROY
        });
    }

    internal void CreateCognitoAdministratorsUserGroup()
    {
        CognitoAdminUserGroup = new CfnUserPoolGroup(this, "AdministratorsGroup", new CfnUserPoolGroupProps
        {
            UserPoolId = WebAppUserPool.UserPoolId,
            GroupName = "Administrators",
            Precedence = 0
        });
    }

    internal void CreateDefaultAdminUser()
    {
        const string UserName = "admin";

        // Create default admin user for testing
        var defaultUser = new AwsCustomResource(this, "CreateAdminUser", new AwsCustomResourceProps
        {
            OnCreate = new AwsSdkCall
            {
                Service = "CognitoIdentityServiceProvider",
                Action = "adminCreateUser",
                Parameters = new Dictionary<string, string>
                {
                    { "UserPoolId", WebAppUserPool.UserPoolId },
                    { "Username", UserName },
                    { "TemporaryPassword", "P@ssword1" },
                    { "MessageAction", "SUPPRESS" }
                },
                PhysicalResourceId = PhysicalResourceId.Of($"{Constants.AppName}AdminUser")
            },
            OnDelete = new AwsSdkCall
            {
                Service = "CognitoIdentityServiceProvider",
                Action = "adminDeleteUser",
                Parameters = new Dictionary<string, string>
                {
                    { "UserPoolId", WebAppUserPool.UserPoolId },
                    { "Username", UserName }
                }
            },
            Policy = AwsCustomResourcePolicy.FromSdkCalls(new SdkCallsPolicyOptions { Resources = AwsCustomResourcePolicy.ANY_RESOURCE })
        });

        var adminUserAttachment = new CfnUserPoolUserToGroupAttachment(this, "AttachAdminUserToAdministratorsGroup", new CfnUserPoolUserToGroupAttachmentProps
        {
            GroupName = CognitoAdminUserGroup.GroupName,
            Username = UserName,
            UserPoolId = WebAppUserPool.UserPoolId
        });

        adminUserAttachment.Node.AddDependency(defaultUser);
    }

    internal void CreateUserPoolClient()
    {
        var localClient = new UserPoolClient(this, "LocalClient", new UserPoolClientProps
        {
            UserPool = WebAppUserPool,
            GenerateSecret = false,
            PreventUserExistenceErrors = true,
            ReadAttributes = new ClientAttributes()
                    .WithStandardAttributes(new StandardAttributesMask
                    {
                        GivenName = true,
                        FamilyName = true,
                        Email = true
                    }),
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
                    $"{userPoolCallbackUrlRoot}/signin-oidc"
                },
                LogoutUrls = new[]
                {
                    $"{userPoolCallbackUrlRoot}/"
                }
            }
        });

        var bobsBookstoreUserPoolDomain = WebAppUserPool.AddDomain($"{Constants.AppName}UserPoolDomain", new UserPoolDomainOptions
        {
            CognitoDomain = new CognitoDomainOptions
            {
                // The prefix must be unique across the AWS Region in which the pool is created
                DomainPrefix = $"{Constants.AppName.ToLower()}-{Account}"
            }
        });

        bobsBookstoreUserPoolDomain.SignInUrl(localClient, new SignInUrlOptions
        {
            RedirectUri = $"{userPoolCallbackUrlRoot}/signin-oidc"
        });

        _ = new[]
        {
            new StringParameter(this, "UserPoolLocalClientId", new StringParameterProps
            {
                ParameterName = $"/{Constants.AppName}/Authentication/Cognito/LocalClientId",
                StringValue = localClient.UserPoolClientId
            }),

            new StringParameter(this, "UserPoolMetadataAddress", new StringParameterProps
            {
                ParameterName = $"/{Constants.AppName}/Authentication/Cognito/MetadataAddress",
                StringValue = $"https://cognito-idp.{Region}.amazonaws.com/{WebAppUserPool.UserPoolId}/.well-known/openid-configuration"
            }),

            new StringParameter(this, "UserPoolCognitoDomain", new StringParameterProps
            {
                ParameterName = $"/{Constants.AppName}/Authentication/Cognito/CognitoDomain",
                StringValue = bobsBookstoreUserPoolDomain.BaseUrl()
            })
        };
    }
}