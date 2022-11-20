using Amazon.Polly;
using Amazon.Rekognition;
using Amazon.S3;
using Bookstore.Data.Data;
using Bookstore.Data.Repository.Implementation.InventoryImplementation;
using Bookstore.Data.Repository.Implementation.SearchImplementation;
using Bookstore.Data.Repository.Implementation;
using Bookstore.Data.Repository.Interface.InventoryInterface;
using Bookstore.Data.Repository.Interface.SearchImplementations;
using Bookstore.Data.Repository.Interface;
using CustomerSite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SecretsManager.Model;
using Amazon.SecretsManager;
using Bookstore.Domain.AdminUser;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;
using System;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var awsOptions = builder.Configuration.GetAWSOptions();
builder.Services.AddDefaultAWSOptions(awsOptions);

builder.Configuration.AddSystemsManager("/BobsUsedBookCustomerStore/");
builder.Configuration.AddSystemsManager("/bookstoredb/");

builder.Services.AddCognitoIdentity();
builder.Services.AddRazorPages();

var connectionString =
    builder.Environment.IsDevelopment()
        ? builder.Configuration.GetConnectionString("BookstoreDbDefaultConnection")
        : await GetConnectionStringAsync(awsOptions, builder.Configuration);

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddAWSService<IAmazonS3>();
builder.Services.AddAWSService<IAmazonPolly>();
builder.Services.AddAWSService<IAmazonRekognition>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddTransient<IBookSearch, BookSearchRepository>();
builder.Services.AddTransient<IPriceSearch, PriceSearchRepository>();
//builder.Services.AddTransient<IInventory, Inventory>();
builder.Services.AddTransient<IRekognitionNPollyRepository, RekognitionNPollyRepository>();
builder.Services.AddTransient<ISearchRepository, SearchRepository>();
builder.Services.AddTransient<ISearchDatabaseCalls, SearchDatabaseCalls>();

builder.Services.AddAutoMapper(typeof(Startup));

builder.Services.AddSession();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
//app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        "default",
        "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapRazorPages();
});

app.Run();

async Task<string> GetConnectionStringAsync(AWSOptions awsOptions, ConfigurationManager configuration)
{
    var connString = configuration.GetConnectionString("BookstoreDbDefaultConnection");

    try
    {
        Console.WriteLine("Non-development mode, building connection string for SQL Server");

        //take the db details from secret manager
        var secretsClient = awsOptions.CreateServiceClient<IAmazonSecretsManager>();
        var response = await secretsClient.GetSecretValueAsync(new GetSecretValueRequest
        {
            SecretId = configuration.GetValue<string>("dbsecretsname")
        });

        var dbSecrets = JsonSerializer.Deserialize<DbSecrets>(response.SecretString, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        var partialConnString = string.Format(connString, dbSecrets.Host, dbSecrets.Port);

        var builder = new SqlConnectionStringBuilder(partialConnString)
        {
            UserID = dbSecrets.Username,
            Password = dbSecrets.Password
        };

        return builder.ConnectionString;
    }
    catch (AmazonSecretsManagerException e)
    {
        Console.WriteLine(
            $"Failed to read secret {builder.Configuration.GetValue<string>("DbSecretsParameterName")}, error {e.Message}, inner {e.InnerException.Message}");
        throw;
    }
    catch (JsonException e)
    {
        Console.WriteLine(
            $"Failed to parse content for secret {builder.Configuration.GetValue<string>("DbSecretsParameterName")}, error {e.Message}");
        throw;
    }
}


//using Microsoft.AspNetCore.Hosting;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
//using NLog.Web;
//namespace CustomerSite
//{
//    public class Program
//    {
//        public static void Main(string[] args)
//        {
//            CreateHostBuilder(args).Build().Run();
//        }

//        private static IHostBuilder CreateHostBuilder(string[] args)
//        {
//            return Host.CreateDefaultBuilder(args)
//                .ConfigureAppConfiguration((context, builder) =>
//                {
//                    builder.AddSystemsManager("/BobsUsedBookCustomerStore/");
//                    builder.AddSystemsManager("/bookstoredb/");
//                })
//                .ConfigureWebHostDefaults(webBuilder =>
//                {
//                    webBuilder.UseStartup<Startup>();
//                }).ConfigureLogging(logging =>
//                {
//                    logging.AddAWSProvider();

//                    // When you need logging below set the minimum level. Otherwise the logging framework will default to Informational for external providers.
//                    logging.SetMinimumLevel(LogLevel.Debug);
//                }).UseNLog();
//        }
//    }
//}