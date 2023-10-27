using Amazon;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Bookstore.Web.Startup
{
    public static class ConfigurationObservability
    {
        public static WebApplicationBuilder AddObservabilityOptions(this WebApplicationBuilder builder)
        {
            ConfigureLogging(builder);

            return builder;
        }

        public static void ConfigureLogging(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            if (builder.Configuration["Services:Observability"] == "AWS")
            {
                builder.Logging.AddAWSProvider();
            }
            else { builder.Logging.AddDebug(); }
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