using BookstoreBackend;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using NLog;

namespace DIAndLoggingTestApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //var logger = NLog.Web.NLogBuilder.ConfigureNLog("NLog.config").GetCurrentClassLogger();
            try
            {
                //logger.Debug("init main");
                CreateHostBuilder(args).Build().Run();
            }
            finally
            {
                // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
                LogManager.Shutdown();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddSystemsManager("/BobsUsedBookAdminStore/");
                    builder.AddSystemsManager("/bookstoredb/");
                })
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
        /* .ConfigureLogging(logging =>
        {
             logging.ClearProviders();
             logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
         })
         .UseNLog();  
         */
        // NLog: Setup NLog for Dependency injection
    }
}