using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace Bookstore.Web.Startup
{
    public static class ConfigurationSetup
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
        {
            if (builder.Configuration["Services:Authentication"] == "AWS" ||
                builder.Configuration["Services:Database"] == "AWS" ||
                builder.Configuration["Services:FileService"] == "AWS" ||
                builder.Configuration["Services:ImageValidationService"] == "AWS")
            {
                builder.Configuration.AddSystemsManager("/BobsBookstore/");
            }

            return builder;
        }
    }
}