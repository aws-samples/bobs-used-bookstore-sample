using Amazon.CDK;
using Constructs;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SSM;
using Amazon.CDK.AWS.CloudFront;
using Amazon.CDK.AWS.IAM;

namespace SharedInfrastructure.Production;

public class CoreStack : Stack
{
    private const string EnvStageName = "Production";
    private string adminSiteParametersRoot = $"BobsUsedBooks-{EnvStageName}-AdminSite";

    public Bucket ImageBucket { get; private set; }

    internal CoreStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
    {
        CreateImageS3Bucket();
        CreateCloudFrontDistribution();
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
            ParameterName = $"/{adminSiteParametersRoot}/AWS/BucketName",
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
            ParameterName = $"/{adminSiteParametersRoot}/AWS/CloudFrontDomain",
            StringValue = $"https://{distribution.DistributionDomainName}"
        });
    }
}