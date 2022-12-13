using Amazon.Polly;
using Amazon.Rekognition;
using Amazon.S3;
using Amazon.Translate;
using Bookstore.Data.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AdminSite.Startup
{
    public static class ServicesSetup
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddAWSService<IAmazonPolly>();
            builder.Services.AddAWSService<IAmazonRekognition>();
            builder.Services.AddAWSService<IAmazonTranslate>();
            builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
            builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("BookstoreDbDefaultConnection")));

            return builder;
        }
    }
}
