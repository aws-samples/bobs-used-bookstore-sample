using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AdminSite.Startup
{
    public static class ConfigurationSetup
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
        {
            var dbSettingsRootKey = $"/BobsUsedBooks-{builder.Environment.EnvironmentName}-DbSettings/";
            var appSettingsRootKey = $"/BobsUsedBooks-{builder.Environment.EnvironmentName}-AdminSite/";

            if (!builder.Environment.IsDevelopment())
            {
                Console.WriteLine($"Configuring settings injection from root key {appSettingsRootKey}");

                // This root contains subkeys related to the storage bucket and CloudFront distribution
                // the bucket must be accessed through, plus Cognito user pool data
                builder.Configuration.AddSystemsManager(appSettingsRootKey, optional: true);

                // local and integrated debug profiles use a localdb instance
                if (builder.Environment.IsProduction())
                {
                    Console.WriteLine($"Production environment, adding db settings injection from root key {dbSettingsRootKey}");

                    // This root contains a subkey, dbsecretsname, that contains the name of the Secrets Manager
                    // secret with the RDS database credentials. In production, we read these to build
                    // the connection string at runtime instead of storing sensitive info in appsettings.
                    builder.Configuration.AddSystemsManager(dbSettingsRootKey, optional: false);
                }
            }
            
            return builder;
        }
    }
}
