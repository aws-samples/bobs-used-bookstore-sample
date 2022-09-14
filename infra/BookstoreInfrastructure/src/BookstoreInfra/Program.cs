using Amazon.CDK;

namespace BookstoreInfra;

internal sealed class Program
{
    public static void Main(string[] args)
    {
        var app = new App();
        new CoreResourceStack(app, "BobsBookstoreCoreInfra");

        app.Synth();
    }   
}