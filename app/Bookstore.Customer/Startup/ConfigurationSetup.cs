using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Bookstore.Customer.Startup
{
    public static class ConfigurationSetup
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
        {
            if (!builder.Environment.IsDevelopment())
            {
                // This root contains subkeys related to the storage bucket and CloudFront distribution
                // the bucket must be accessed through, plus Cognito user pool data
                builder.Configuration.AddSystemsManager($"/BobsUsedBooks-{builder.Environment.EnvironmentName}-CustomerSite/", optional: true);

                // local and integrated debug profiles use a localdb instance
                if (builder.Environment.IsProduction())
                {
                    // This root contains a subkey, dbsecretsname, containing the name of the Secrets Manager
                    // secret with the RDS database credentials. In production, we read these to build
                    // the connection string at runtime instead of storing sensitive info in appsettings.
                    builder.Configuration.AddSystemsManager("/BobsUsedBooksProdDBSettings/", optional: true);
                }
            }

            return builder;
        }
    }
}
