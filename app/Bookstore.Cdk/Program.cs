namespace Bookstore.Cdk;

using Amazon.CDK;

internal sealed class Program
{
    // The target account and region can be controlled by hardcoding them in this class
    // (recommended for production workloads) or using --profile on the CDK CLI.
    //
    // Hardcode the account and region by passing them as parameters to the MakeEnv() 
    // method, e.g. MakeEnv("8373873873", "us-west-2")
    //
    // Specify the account and region from the CDK CLI by specifying a profile to use, 
    // e.g. cdk deploy --profile "USWest2Profile"
    //
    // If the account and region are not hardcoded and a profile is not specified on the CLI
    // the CDK will use the account and region of the default profile.
    // 
    // For more information refer to https://docs.aws.amazon.com/cdk/v2/guide/environments.html
    public static void Main()
    {
        var app = new App();

        var env = MakeEnv();

        var coreStack = new CoreStack(app, $"{Constants.AppName}Core", new StackProps { Env = env });
        var networkStack = new NetworkStack(app, $"{Constants.AppName}Network", new StackProps { Env = env });
        var databaseStack = new DatabaseStack(app, $"{Constants.AppName}Database", new DatabaseStackProps { Env = env, Vpc = networkStack.Vpc });

        var ec2Stack = new EC2ComputeStack(app, $"{Constants.AppName}EC2", new EC2ComputeStackProps 
        { 
            Env = env, 
            Vpc = networkStack.Vpc, 
            Database = databaseStack.Database, 
            ImageBucket = coreStack.ImageBucket,
            WebAppUserPool = coreStack.WebAppUserPool
        });

        app.Synth();
    }

    private static Environment MakeEnv(string account = null, string region = null)
    {
        return new Environment
        {
            Account = account ?? System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
            Region = region ?? System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION")
        };
    }
}
