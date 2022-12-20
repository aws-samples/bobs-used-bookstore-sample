using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AdminSite.Startup
{
    public static class ConfigurationSetup
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
        {
            if (!builder.Environment.IsDevelopment())
            {
                // This root contains subkeys related to the storage bucket and CloudFront distribution
                // the bucket must be accessed through, plus Cognito user pool data
                builder.Configuration.AddSystemsManager("/BobsUsedBookAdminStore/", optional: true);

                // This root contains a subkey, dbsecretsname, containing the name of the Secrets Manager
                // secret with the RDS database credentials
                builder.Configuration.AddSystemsManager("/bookstoredb/", optional: true);
            }
            
            return builder;
        }
    }
}
