using Amazon.CDK;
using SharedInfrastructure.IntegratedTest;
using SharedInfrastructure.Production;

namespace SharedInfrastructure;

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
    public static void Main(string[] args)
    {
        var app = new App();

        var env = MakeEnv();

        var integratedTestStack = new IntegratedTestStack(app, "BookstoreIntegratedTest", new StackProps { Env = env });
        var productionStack = new ProductionStack(app, "BookstoreProduction", new StackProps { Env = env });
        var ec2ProductionStack = new EC2ProductionStack(app, "EC2ProductionStack", new StackProps { Env = env });

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
