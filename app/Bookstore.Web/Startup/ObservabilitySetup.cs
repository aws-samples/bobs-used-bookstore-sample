using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using EMF = Amazon.CloudWatch.EMF;

namespace Bookstore.Web.Startup
{
    public static class ObservabilitySetup
    {
        public static WebApplicationBuilder ConfigureObservability(this WebApplicationBuilder builder)
        {
            ConfigureLogging(builder);
            ConfigureMetrics(builder);

            return builder;
        }

        public static void ConfigureLogging(WebApplicationBuilder builder)
        {
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            if(builder.Configuration["Services:LoggingService"] == "aws")
            {
                builder.Logging.AddAWSProvider();
            }
            else
            {
                builder.Logging.AddDebug(); 
            }
        }

        public static void ConfigureMetrics(this WebApplicationBuilder builder)
        {
            EMF.Config.EnvironmentConfigurationProvider.Config = new EMF.Config.Configuration
            {
                ServiceName = "BobsUsedBookstore",
                LogGroupName = "BobsUsedBookstore/emf",
                EnvironmentOverride = builder.Configuration["Services:MetricsService"] == "local"
                            ? EMF.Environment.Environments.Local
                            : EMF.Environment.Environments.Unknown
            };
        }
    }
}