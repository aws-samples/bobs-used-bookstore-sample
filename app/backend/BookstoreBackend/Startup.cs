using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Amazon.S3;
using Amazon.Polly;
using BookstoreBackend.Notifications.NotificationsInterface;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;
using BobsBookstore.DataAccess.Repository.Implementation.SearchImplementation;
using BobsBookstore.DataAccess.Repository.Interface.OrdersInterface;
using BobsBookstore.DataAccess.Repository.Implementation.OrderImplementations;
using BobsBookstore.DataAccess.Repository.Interface.InventoryInterface;
using BobsBookstore.DataAccess.Repository.Implementation.InventoryImplementation;
using BobsBookstore.DataAccess.Repository.Interface.Implementations;
using BobsBookstore.DataAccess.Repository.Interface.WelcomePageInterface;
using BobsBookstore.DataAccess.Repository.Implementation.WelcomePageImplementation;
using BobsBookstore.DataAccess.Repository.Implementation;
using BobsBookstore.DataAccess.Repository.Interface;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SecretsManager;
using BobsBookstore.Models.AdminUser;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using Amazon.SecretsManager.Model;

namespace BookstoreBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration )
        {
            Configuration = configuration;
            var config = (Configuration as IConfigurationRoot).GetDebugView();

        }

        public IConfiguration Configuration { get; }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions());
            services.AddAWSService<IAmazonS3>();
            services.AddAWSService<IAmazonPolly>();
            services.AddAWSService<Amazon.Rekognition.IAmazonRekognition>();
            services.AddAWSService<Amazon.Translate.IAmazonTranslate>();
            services.AddAWSService<Amazon.CloudWatch.IAmazonCloudWatch>();
            services.AddAutoMapper(typeof(Startup));

            services.AddControllersWithViews();
            var awsOptions = Configuration.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
            var connectionString = GetConnectionString(awsOptions);
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
            

            services.AddTransient<INotifications, Notifications.Implementations.Notifications>();
            services.AddTransient<ICustomAdminPage, CustomAdmin>();

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAuthentication(options =>
            {
                // uses cookies on local machine for maintaining authentication
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
                
                
            })
       .AddCookie(options =>
       {
           options.Cookie.Name = "BobsAdminCookie";
           options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
           

       })
       .AddOpenIdConnect(options =>
       {

           // sets the OpenId connect options for cognito hosted UI
           options.ResponseType = Configuration["Authentication:Cognito:ResponseType"];
           options.MetadataAddress = $"https://cognito-idp.{Configuration["Authentication:Cognito:Region"]}.amazonaws.com/{Configuration["AWS:UserPoolId"]}/.well-known/openid-configuration";
           options.ClientId = Configuration["AWS:UserPoolClientId"];
           options.Authority = $"https://{Constants.DomainName}.auth.{Configuration["Authentication:Cognito:Region"]}.amazoncognito.com";
           options.GetClaimsFromUserInfoEndpoint = true;
           options.TokenValidationParameters.ValidateIssuer = true;
           



       });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }

        private string GetConnectionString(AWSOptions awsOptions)
        {
            var connString = Configuration.GetConnectionString("DefaultConnection");
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
                Console.WriteLine($"Failed to read secret {Configuration.GetValue<string>("DbSecretsParameterName")}, error {e.Message}, inner {e.InnerException.Message}");
                throw;
            }
            catch (JsonException e)
            {
                Console.WriteLine($"Failed to parse content for secret {Configuration.GetValue<string>("DbSecretsParameterName")}, error {e.Message}");
                throw;
            }
        }

    }
}
