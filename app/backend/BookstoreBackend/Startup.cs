using System;
using System.Text.Json;
using Amazon.CloudWatch;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Polly;
using Amazon.Rekognition;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using Amazon.Translate;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Implementation;
using BobsBookstore.DataAccess.Repository.Implementation.InventoryImplementation;
using BobsBookstore.DataAccess.Repository.Implementation.NotificationsImplementations;
using BobsBookstore.DataAccess.Repository.Implementation.OrderImplementations;
using BobsBookstore.DataAccess.Repository.Implementation.SearchImplementation;
using BobsBookstore.DataAccess.Repository.Implementation.WelcomePageImplementation;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.DataAccess.Repository.Interface.Implementations;
using BobsBookstore.DataAccess.Repository.Interface.InventoryInterface;
using BobsBookstore.DataAccess.Repository.Interface.NotificationsInterface;
using BobsBookstore.DataAccess.Repository.Interface.OrdersInterface;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;
using BobsBookstore.DataAccess.Repository.Interface.WelcomePageInterface;
using BobsBookstore.Models.AdminUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookstoreBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _ = (Configuration as IConfigurationRoot).GetDebugView();
        }

        public IConfiguration Configuration { get; }

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

            //rds db
           // var connectionString = GetConnectionString(awsOptions);

            //local db
            var connectionString = "Server=(localdb)\\MSSQLLocalDB; Initial Catalog=BobsUsedBookStore;Integrated Security= true";



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

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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
