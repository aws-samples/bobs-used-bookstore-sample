using Amazon.CDK;
using SharedInfrastructure.Production;
using System.Diagnostics;
using System;

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
    public static void Main()
    {
        //PublishBookstore();

        var app = new App();

        var env = MakeEnv();

        var coreStack = new CoreStack(app, $"{Constants.AppName}Core", new StackProps { Env = env });
        var storageStack = new StorageStack(app, $"{Constants.AppName}Storage", new StorageStackProps { Env = env, ApplicationRole = coreStack.ApplicationRole });
        var networkStack = new NetworkStack(app, $"{Constants.AppName}Network", new StackProps { Env = env });
        var databaseStack = new DatabaseStack(app, $"{Constants.AppName}Database", new DatabaseStackProps { Env = env, Vpc = networkStack.Vpc });
        var appRunnerStack = new AppRunnerStack(app, $"{Constants.AppName}AppRunner", new AppRunnerStackProps {  Env = env, Vpc = networkStack.Vpc, ImageBucket = storageStack.ImageBucket });
        //var ec2Stack = new EC2ComputeStack(app, $"{Constants.AppName}EC2", new EC2ComputeStackProps 
        //{ 
        //    Env = env, 
        //    Vpc = networkStack.Vpc, 
        //    Database = databaseStack.Database, 
        //    WebAppUserPool= coreStack.WebAppUserPool
        //});

        app.Synth();
    }

    private static Amazon.CDK.Environment MakeEnv(string account = null, string region = null)
    {
        return new Amazon.CDK.Environment
        {
            Account = account ?? System.Environment.GetEnvironmentVariable("CDK_DEFAULT_ACCOUNT"),
            Region = region ?? System.Environment.GetEnvironmentVariable("CDK_DEFAULT_REGION")
        };
    }

    private static void PublishBookstore()
    {
        // Set the path to the project you want to build and publish
        string projectPath = @"..\..\..\..\Bookstore.Web\Bookstore.Web.csproj";

        // Set the path to the output directory where the published files will be placed
        string outputPath = @"..\..\..\..\Bookstore.Web\bin\Release\net6.0\publish\";

        // Publish the project
        Console.WriteLine("Publishing project...");
        var publishProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"publish \"{projectPath}\" -c Release -o \"{outputPath}\"",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        publishProcess.Start();
        string publishOutput = publishProcess.StandardOutput.ReadToEnd();
        publishProcess.WaitForExit();

        // Check if the publish was successful
        if (publishProcess.ExitCode != 0)
        {
            Console.WriteLine("Publish failed.");
            Console.WriteLine(publishOutput);
            return;
        }

        Console.WriteLine("Project published successfully.");
    }
}