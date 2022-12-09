using Amazon.Polly;
using Amazon.Rekognition;
using Amazon.S3;
using Bookstore.Data.Data;
using CustomerSite;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookstore.Customer.Startup
{
    public static class ServicesSetup
    {
        public static WebApplicationBuilder ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages(); //TODO I think this can be removed
            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddAWSService<IAmazonPolly>();
            builder.Services.AddAWSService<IAmazonRekognition>();
            builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
            builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(builder.Configuration.GetConnectionString("BookstoreDbDefaultConnection")));
            builder.Services.AddAutoMapper(x => x.AddProfile<AutoMapperProfile>());
            builder.Services.AddSession();

            return builder;
        }
    }
}