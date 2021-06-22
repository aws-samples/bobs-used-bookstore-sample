using Amazon.CDK;

namespace BobsBookstoreInfra
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            var infraStack = new BobsBookstoreInfraStack(app, "BobsUsedBookstoreStack");

            app.Synth();
        }
    }
}
