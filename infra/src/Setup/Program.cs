using Amazon.CDK;

namespace BobsUsedBookstoreSetup
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();

            var infraStack = new BobsUsedBookstoreStack(app, "BobsUsedBookstoreStack");

            app.Synth();
        }
    }
}
