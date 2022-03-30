using Amazon.CDK;

namespace BookstoreInfra
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new CoreResourceStack(app, "BobsBookstoreCoreInfra", new StackProps
            {
                Env = MakeEnv(null, "us-west-2")
            });

            app.Synth();
        }

        private static Amazon.CDK.Environment MakeEnv(string account = null, string region = null)
        {
            return new Amazon.CDK.Environment
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
}
