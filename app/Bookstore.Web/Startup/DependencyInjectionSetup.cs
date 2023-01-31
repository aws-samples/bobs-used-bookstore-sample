using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Microsoft.Extensions.Hosting;
using Bookstore.Services;
using Bookstore.Data;

namespace Bookstore.Web.Startup
{
    public static class DependencyInjectionSetup
    {
        public static WebApplicationBuilder ConfigureDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IInventoryService, InventoryService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IReferenceDataService, ReferenceDataService>();
            builder.Services.AddTransient<IOfferService, OfferService>();
            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddTransient<IFileService, LocalFileService>(x => new LocalFileService(builder.Environment.WebRootPath));
            }
            else
            {
                builder.Services.AddTransient<IFileService, S3FileService>();
            }

            return builder;
        }
    }
}