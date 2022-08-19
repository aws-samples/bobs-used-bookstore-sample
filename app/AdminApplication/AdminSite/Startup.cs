using System;
using System.Text.Json;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Polly;
using Amazon.Rekognition;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.Translate;
using DataAccess.Data;
using DataAccess.Repository.Implementation;
using DataAccess.Repository.Implementation.InventoryImplementation;
using DataAccess.Repository.Implementation.NotificationsImplementations;
using DataAccess.Repository.Implementation.OrderImplementations;
using DataAccess.Repository.Implementation.SearchImplementation;
using DataAccess.Repository.Implementation.WelcomePageImplementation;
using DataAccess.Repository.Interface;
using DataAccess.Repository.Interface.Implementations;
using DataAccess.Repository.Interface.InventoryInterface;
using DataAccess.Repository.Interface.NotificationsInterface;
using DataAccess.Repository.Interface.OrdersInterface;
using DataAccess.Repository.Interface.SearchImplementations;
using DataAccess.Repository.Interface.WelcomePageInterface;
using DataModels.AdminUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AdminSite
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            CurrentEnvironment = env;
            _ = (Configuration as IConfigurationRoot).GetDebugView();
        }

        public IConfiguration Configuration { get; }
        private IWebHostEnvironment CurrentEnvironment { get; set; }

        public void ConfigureServices(IServiceCollection services)
        {
            var awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);

            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonPolly>();
            services.AddAWSService<IAmazonRekognition>();
            services.AddAWSService<IAmazonTranslate>();

            services.AddAutoMapper(typeof(Startup));
            services.AddCognitoIdentity();
            services.AddRazorPages();
            services.AddControllersWithViews();

            var connectionString = Configuration.GetConnectionString("BobsBookstoreContextConnection");

            if (!CurrentEnvironment.IsDevelopment())
            {
                connectionString = GetConnectionString(awsOptions);
            }


            services.AddDbContext<ApplicationDbContext>(option => option.UseSqlServer(connectionString));
            services.AddTransient<ISearchDatabaseCalls, SearchDatabaseCalls>();
            services.AddTransient<IExpressionFunction, ExpressionFunction>();
            services.AddTransient<IOrderDatabaseCalls, OrderDatabaseCalls>();

            services.AddTransient<IInventory, Inventory>();
            services.AddTransient<IRekognitionNPollyRepository, RekognitionNPollyRepository>();

            services.AddTransient<ISearchRepository, SearchRepository>();
            services.AddTransient<IOrderRepository, OrderRepository>();
            services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
            services.AddTransient<IOrderStatusRepository, OrderStatusRepository>();

            services.AddTransient<INotifications, Notifications>();
            services.AddTransient<ICustomAdminPage, CustomAdmin>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext db)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/Error/Index/{0}");

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            DbInitializer.Initialize(db, Configuration["AWS:BucketName"], Configuration["AWS:CloudFrontDomain"], env.WebRootPath);

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }

        private string GetConnectionString(AWSOptions awsOptions)
        {
            var connString = Configuration.GetConnectionString("DefaultConnection");
            try
            {
                Console.WriteLine("Non-development mode, building connection string for SQL Server");

                // take the db details from Secrets Manager
                var secretsClient = awsOptions.CreateServiceClient<IAmazonSecretsManager>();
                var response = secretsClient.GetSecretValueAsync(new GetSecretValueRequest
                {
                    SecretId = Configuration.GetValue<string>("dbsecretsname")
                }).Result;

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
                    $"Failed to read secret {Configuration.GetValue<string>("DbSecretsParameterName")}, error {e.Message}, inner {e.InnerException.Message}");
                throw;
            }
            catch (JsonException e)
            {
                Console.WriteLine(
                    $"Failed to parse content for secret {Configuration.GetValue<string>("DbSecretsParameterName")}, error {e.Message}");
                throw;
            }
        }
    }
}
