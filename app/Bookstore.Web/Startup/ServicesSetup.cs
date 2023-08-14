using Amazon.Rekognition;
using Amazon.S3;
using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Bookstore.Data;
using Bookstore.Domain.AdminUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text.Json;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;

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
            });

            builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
            builder.Services.AddAWSService<IAmazonS3>();
            builder.Services.AddAWSService<IAmazonRekognition>();

            var connString = builder.Configuration["ConnectionStrings:BookstoreDatabaseConnection"];

            if (builder.Configuration["Services:Database"] == "AWS")
            {
                builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connString));
            }
            else
            {
                builder.Services.AddDbContext<ApplicationDbContext>(option => option.UseSqlite(connString));
            }

            builder.Services.AddSession();

            return builder;
        }
    }
}