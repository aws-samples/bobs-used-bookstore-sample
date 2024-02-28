using Amazon.Rekognition;
using Amazon.S3;
using Bookstore.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using System.IO;
using Bookstore.Web.Helpers;
using Amazon.CloudWatch.EMF.Web;

namespace Bookstore.Web.Startup
{
    public static class ServicesSetup
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews(x =>
            {
                x.Filters.Add(new AuthorizeFilter());
                x.Filters.Add(new AutoValidateAntiforgeryTokenAttribute());
                x.Filters.Add<LogActionFilter>();
            });

            builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddAWSService<IAmazonRekognition>();

            var connString = builder.Configuration["ConnectionStrings:BookstoreDatabaseConnection"];

            if (builder.Configuration["Services:Database"] == "aws")
            {
                builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connString));
            }
            else
            {
                connString = $"Data Source={Path.Combine(Path.GetTempPath(), "bookstore.db")}";
                builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlite(connString));
            }

            builder.Services.AddSession();
            builder.Services.AddEmf();

            return builder;
        }
    }
}