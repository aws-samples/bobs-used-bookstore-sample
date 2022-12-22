using Amazon.CDK;

namespace LocalTestInfrastructure;

internal sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        _ = new LocalTestStack(app, "BobsUsedBooksLocalTest", new StackProps
        {
            // if no explicit region is set on synth/deployment, or set in environment variables,
            // default to US West (Oregon)
            Env = MakeEnv(null, "us-west-2")
        });

        app.Synth();
    }

    private static Environment MakeEnv(string account = null, string region = null)
    {
        return new Environment
        {
            Account = account ??
                      System.Environment.GetEnvironmentVariable("CDK_DEPLOY_ACCOUNT") ??
                      System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
            Region = region ??
                     System.Environment.GetEnvironmentVariable("CDK_DEPLOY_REGION") ??
                     System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION")
        };
    }
}
