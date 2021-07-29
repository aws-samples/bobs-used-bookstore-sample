using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using BobsBookstore.DataAccess.Data;
using BobsBookstore.DataAccess.Repository.Interface;
using BobsBookstore.DataAccess.Repository.Implementation;
using BobsBookstore.DataAccess.Repository.Interface.SearchImplementations;
using BobsBookstore.DataAccess.Repository.Implementation.SearchImplementation;

namespace BobBookstore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddCognitoIdentity();
            services.AddRazorPages();
            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("BobBookstoreContextConnection")));

            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IBookSearch, BookSearchRepository>();
            services.AddTransient<IPriceSearch, PriceSearchRepository>();


            //new part
            services.AddSession();
            //services.AddMvc();
            services.AddSingleton<IHttpContextAccessor,
            HttpContextAccessor>();
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
            //new part
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
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}
