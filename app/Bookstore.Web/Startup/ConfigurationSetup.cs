using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Bookstore.Web.Startup
{
    public static class ConfigurationSetup
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.AddSystemsManager("/BobsBookstore/", optional: true);

            return builder;
        }
    }
}
