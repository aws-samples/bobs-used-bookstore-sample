using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Bookstore.Web.Startup
{
    public static class ConfigurationSetup
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
        {
            LoadConfigurationFromSystemsManager(builder, "Services:Authentication", "/BobsBookstore/Authentication/");
            LoadConfigurationFromSystemsManager(builder, "Services:FileService", "/BobsBookstore/Files/");
            LoadConfigurationFromSystemsManager(builder, "Services:Database", "/BobsBookstore/Database/");

            return builder;
        }

        private static void LoadConfigurationFromSystemsManager(WebApplicationBuilder builder, string key, string path)
        {
            if (builder.Configuration[key] == "AWS")
            {
                builder.Configuration.AddSystemsManager(path);
            }
        }
    }
}