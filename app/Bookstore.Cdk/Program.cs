using System;
using System.Diagnostics;
using Amazon.CDK;
using Bookstore.Common;
using SharedInfrastructure.Production;

namespace Bookstore.Cdk;

internal sealed class Program
{
    public static void Main()
    {
        //PublishBookstore();

        var app = new App();

        var env = MakeEnv();

        var coreStack = new CoreStack(app, $"{Constants.AppName}Core", new StackProps { Env = env });
        var networkStack = new NetworkStack(app, $"{Constants.AppName}Network", new StackProps { Env = env });
        var databaseStack = new DatabaseStack(app, $"{Constants.AppName}Database", new DatabaseStackProps { Env = env, Vpc = networkStack.Vpc });
        var appRunnerStack = new AppRunnerStack(app, $"{Constants.AppName}AppRunner", new AppRunnerStackProps {  Env = env, Vpc = networkStack.Vpc, ImageBucket = coreStack.ImageBucket });

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