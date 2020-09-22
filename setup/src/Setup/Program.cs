using Amazon.CDK;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BobsUsedBookstoreSetup
{
    sealed class Program
    {
        public static void Main(string[] args)
        {
            var app = new App();
            new BobsUsedBookstoreStack(app, "BobsUsedBookstoreStack");
            app.Synth();
        }
    }
}
