using Bookstore.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Bookstore.Web.Startup
{
    public static class ConfigurationSetup
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
        {
            LoadConfigurationFromSystemsManager(builder, "Services:Authentication", $"/{Constants.AppName}/Authentication/");
            LoadConfigurationFromSystemsManager(builder, "Services:FileService", $"/{Constants.AppName}/Files/");
            LoadConfigurationFromSystemsManager(builder, "Services:Database", $"/{Constants.AppName}/Database/");

            return builder;
        }

        private static void LoadConfigurationFromSystemsManager(WebApplicationBuilder builder, string key, string path)
        {
            if (builder.Configuration[key] == "aws")
            {
                builder.Configuration.AddSystemsManager(path);
            }
        }
    }
}