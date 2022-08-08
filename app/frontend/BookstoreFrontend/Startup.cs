using System;
using System.Text.Json;
using Amazon.Extensions.NETCore.Setup;
using Amazon.Polly;
using Amazon.Rekognition;
using Amazon.S3;
using Amazon.SecretsManager;
using Amazon.SecretsManager.Model;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Implementation;
using BobsBookstore.DataAccess.Repository.Implementation.InventoryImplementation;
using BobsBookstore.DataAccess.Repository.Implementation.SearchImplementation;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.DataAccess.Repository.Interface.Implementations;
using BobsBookstore.DataAccess.Repository.Interface.InventoryInterface;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;
using BobsBookstore.Models.AdminUser;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BookstoreFrontend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var config = (Configuration as IConfigurationRoot).GetDebugView();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();

            var awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);

            services.AddCognitoIdentity();
            services.AddRazorPages();

            //RDS db
            var rdsConnectionString = GetConnectionString(awsOptions);

            //local db
            var localConnectionString = "Server=(localdb)\\MSSQLLocalDB; Initial Catalog=BobsUsedBookStore;Integrated Security= true";


            //Set this to rdsConnectionString or localConnectionString
            var connectionString = localConnectionString;

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonPolly>();
            services.AddAWSService<IAmazonRekognition>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddTransient<IBookSearch, BookSearchRepository>();
            services.AddTransient<IPriceSearch, PriceSearchRepository>();
            services.AddTransient<IInventory, Inventory>();
            services.AddTransient<IRekognitionNPollyRepository, RekognitionNPollyRepository>();
            services.AddTransient<ISearchRepository, SearchRepository>();
            services.AddTransient<ISearchDatabaseCalls, SearchDatabaseCalls>();

            services.AddAutoMapper(typeof(Startup));

            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ApplicationDbContext db)
        {
            if (env.IsDevelopment())
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
            db.Database.EnsureCreated();

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
            var connString = Configuration.GetConnectionString("BobsBookstoreContextConnection");
            try
            {
                Console.WriteLine("Non-development mode, building connection string for SQL Server");

                //take the db details from secret manager
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
