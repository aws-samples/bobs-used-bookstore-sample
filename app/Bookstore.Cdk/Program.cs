using Amazon.CDK;
using Bookstore.Common;

namespace Bookstore.Cdk;

internal sealed class Program
{
    public static void Main()
    {
        var app = new App();

        var env = MakeEnv();

        var coreStack = new CoreStack(app, $"{Constants.AppName}Core", new StackProps { Env = env });
        var networkStack = new NetworkStack(app, $"{Constants.AppName}Network", new StackProps { Env = env });
        var databaseStack = new DatabaseStack(app, $"{Constants.AppName}Database", new DatabaseStackProps { Env = env, Vpc = networkStack.Vpc });
        var appRunnerStack = new AppRunnerStack(app, $"{Constants.AppName}AppRunner", new AppRunnerStackProps {  Env = env, Vpc = networkStack.Vpc, Database = databaseStack.Database, ImageBucket = coreStack.ImageBucket, WebAppUserPool = coreStack.WebAppUserPool });
        var ecsStack = new EcsStack(app, $"{Constants.AppName}ECS", new EcsStackProps { Env = env, Vpc = networkStack.Vpc, Database = databaseStack.Database, ImageBucket = coreStack.ImageBucket, WebAppUserPool = coreStack.WebAppUserPool });

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