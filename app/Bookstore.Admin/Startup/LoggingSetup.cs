using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace AdminSite.Startup
{
    public static class LoggingSetup
    {
        public static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
        {
            builder.Logging.AddAWSProvider();

            return builder;
        }
    }
}
