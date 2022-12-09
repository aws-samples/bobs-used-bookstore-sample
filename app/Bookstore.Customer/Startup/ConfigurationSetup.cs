using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Bookstore.Customer.Startup
{
    public static class ConfigurationSetup
    {
        public static WebApplicationBuilder ConfigureConfiguration(this WebApplicationBuilder builder)
        {
            if (!builder.Environment.IsDevelopment())
            {
                builder.Configuration.AddSystemsManager("/BobsUsedBookCustomerStore/", optional: true);
                builder.Configuration.AddSystemsManager("/bookstoredb/", optional: true);
            }

            return builder;
        }
    }
}
