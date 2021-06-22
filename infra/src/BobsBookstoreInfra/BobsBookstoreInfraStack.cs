using Amazon.CDK;
using Amazon.CDK.AWS.EC2;
using Amazon.CDK.AWS.S3;
using Amazon.CDK.AWS.SSM;
using Amazon.CDK.AWS.Cognito;
using Amazon.CDK.AWS.RDS;
using Amazon.CDK.AWS.SecretsManager;
using Amazon.CDK.AWS.IAM;
using Amazon.CDK.AWS.CloudFront;

namespace BobsBookstoreInfra
{
    public class BobsBookstoreInfraStack : Stack
    {
        internal BobsBookstoreInfraStack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            // Start with creating a new VPC to hold the application resources.
            var vpc = new Vpc(this, "BookstoreVpc", new VpcProps
            {
                // one public, and one private, subnet will be created per AZ
                MaxAzs = 2
            });

            // The bucket is used to store book cover images and other static resources.
            // NOTE: this bucket will not be removed when the stack is deleted if it contains content.
            // You will need to first delete all content in the bucket, then delete the bucket itself,
            // manually. TODO: give the user an app to delete the bucket, or explicit instructions on
            // how to easily do this from a toolkit or PowerShell tools.
            var bookstoreBucket = new Bucket(this, "BookstoreBucket", new BucketProps
            {
                PublicReadAccess = false,
                BlockPublicAccess = BlockPublicAccess.BLOCK_ALL
            });

            // Setup bucket policy to only serve content thru CloudFront, and then create a
            // CloudFront distribution.
            // TODO

            // Create a secret to hold the database credentials (it will get updated with host etc automatically
            // once attached to the database). We do this, rather than allow a secret to be automatically created,
            // so that we can predict the secret name (not available if auto-created) which we'll use in a Parameter
            // Store entry later.
            var secretName = $"BobsUsedBookstoreDbCredentials{System.DateTime.UtcNow.Ticks}";
            var dbSecret = new Secret(this, "DbCredentialsSecret", new SecretProps
            {
                SecretName = secretName,
                GenerateSecretString = new SecretStringGenerator
                {
                    // keys must be 'username' and 'password'
                    GenerateStringKey = "password",
                    SecretStringTemplate = "{ \"username\": \"Admin\"}",
                    ExcludePunctuation = true
                }
            });

            // Create an RDS SQL Server Express instance inside the VPC. Note that this does not create the
            // Bookstore database - that will be done by seed data inside the sample application using
            // EF Core tooling
            var sqlServerInstance = new DatabaseInstance(this, "BookstoreDatabase", new DatabaseInstanceProps
            {
                Engine = DatabaseInstanceEngine.SqlServerEx(new SqlServerExInstanceEngineProps
                {
                    Version = SqlServerEngineVersion.VER_14_00_3192_2_V1
                }),
                MasterUsername = "Admin",
                MasterUserPassword = dbSecret.SecretValueFromJson("password"),
                Vpc = vpc,
                InstanceType = new InstanceType("t2.micro"),
                // Place the demo database into public subnets, so that we can work with it from EF Core
                // on our development machines, or our IDEs. For production usage, it is a recommended
                // practice to place databases into private subnets.
                VpcSubnets = new SubnetSelection
                {
                    SubnetType = SubnetType.PUBLIC
                },
                AllocatedStorage = 20,
                // NOTE: this setting allows the database instance to be deleted automatically when
                // the stack is destroyed - DATA WILL BE LOST IF THIS IS USED ON A PRODUCTION INSTANCE
                DeletionProtection = false
            });

            // This will cause the secret and the instance to be linked, and the secret updated with
            // details such as host, port, etc.
            dbSecret.Attach(sqlServerInstance);

            // This makes the database publicly accessible, so we can work with to from within our
            // client-side tools (eg Visual Studio, EF Core command line, et al). Note that for production
            // scenarios the recommended best practice is to place databases in private subnets and restrict
            // access.
            sqlServerInstance.Connections.AllowDefaultPortFromAnyIpv4();

            // The user pool is where user registrations on the site will be held.
            var userPool = new UserPool(this, "BookstoreUserPool", new UserPoolProps
            {
                UserPoolName = "BobsBookstoreUsers"
            });

            // The pool client will allow our front-end to permit user registration. Note that we cannot
            // recover the client secret here, so some post-processing or initial start-up code in the
            // application is needed to recover the secret and make it into a Parameter Store entry that
            // we can recover at runtime.
            var userPoolClient = new UserPoolClient(this, "BookstorePoolClient", new UserPoolClientProps
            {
                UserPool = userPool,
                GenerateSecret = true
            });

            // Build a set of Parameter Store entries relating to the app settings and secrets that will
            // be retrieved dynamically at runtime instead of hard-coding them into the application's
            // source and configuration files, where they could present security risks (eg by checking
            // credentials into public source repositories).
            var ssmParameters = new StringParameter[]
            {
                // Since we already have a Secrets Manager secret with database details, reference it
                // in such a way that Parameter Store can access it. The application will read this
                // parameter (thru Systems Manager) on startup, which in turn will yield the secret value
                // from within Secrets Manager - the app will then use the username, password, and
                // host/port data to build the connection string.
                new StringParameter(this, "BookstoreDbDetails", new StringParameterProps
                {
                    ParameterName = "/BobsUsedBookstore/DatabaseDetails",
                    StringValue = $"/aws/reference/secretsmanager/{secretName}"
                }),

                // Record the user pool details. We will still however be missing a parameter - the user
                // pool client secret that we will later need to recover and turn into a secure Parameter
                // Store entry. Note that the common root of the parameter names allows the
                // Amazon.Extensions.Configuration.SystemsManager NuGet package we use in the application to
                // retrieve all these settings at startup and inject them into the ASP.NET Core Configuration
                // system.
                new StringParameter(this, "BookstoreUserPoolId", new StringParameterProps
                {
                    ParameterName = "/BobsUsedBookstore/AWS/UserPoolId",
                    StringValue = userPool.UserPoolId
                }),

                new StringParameter(this, "BookstoreUserPoolClientId", new StringParameterProps
                {
                    ParameterName = "/BobsUsedBookstore/AWS/UserPoolClientId",
                    StringValue = userPoolClient.UserPoolClientId
                }),

                // CloudFront url to be placed in seed, and subsequent, urls to
                // book cover images
                // TODO
            };

        }
    }
}
