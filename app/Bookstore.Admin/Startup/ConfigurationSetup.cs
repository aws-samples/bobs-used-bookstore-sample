using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace AdminSite.Startup
{
    public static class ConfigurationSetup
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddSystemsManager("/BobsUsedBookAdminStore/", optional: true);
            builder.Configuration.AddSystemsManager("/bookstoredb/", optional: true);

            return builder;
        }
    }
}
