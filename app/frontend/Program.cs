using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;


namespace BobBookstore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }
            
        public static IHostBuilder CreateHostBuilder(string[] args) =>
             Host.CreateDefaultBuilder(args)
            
                 .ConfigureAppConfiguration((context, builder) =>
                 {
                   /*  builder.AddSystemsManager("/BobsUsedBookCustomerStoreVersion27/");
                      builder.AddSystemsManager("/bookstorecdkVersion27/");*/
                     builder.AddSystemsManager("/BobsUsedBookCustomerStore/");
                     builder.AddSystemsManager("/bookstoredb/");

                 })
              .ConfigureWebHostDefaults(webBuilder =>
              {
                  webBuilder
                  .UseStartup<Startup>();
                  
              });
    }
}
