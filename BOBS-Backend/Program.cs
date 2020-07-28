using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Targets;
using NLog.Config;

using NLog.AWS.Logger;
namespace BOBS_Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            Logger logger = LogManager.GetCurrentClassLogger();
            logger.Info("Check the AWS Console CloudWatch Logs console in us-east-1");
            logger.Info("to see messages in the log streams for the");
            logger.Info("log group NLog.ConfigExample");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)

                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
             

    }
}


