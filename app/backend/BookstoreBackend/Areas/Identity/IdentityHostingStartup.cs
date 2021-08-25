using System;
using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(BookstoreBackend.Areas.Identity.IdentityHostingStartup))]
namespace BookstoreBackend.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
            });
        }
    }
}