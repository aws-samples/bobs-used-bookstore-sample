using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace Bookstore.Web.Startup
{
    public static class ObservabilitySetup
    {
        public static WebApplicationBuilder ConfigureObservability(this WebApplicationBuilder builder)
        {
            ConfigureLogging(builder);

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
    }
}