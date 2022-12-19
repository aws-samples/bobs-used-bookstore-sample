using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Services;
using Microsoft.Extensions.Hosting;
using Bookstore.Services;
using Bookstore.Data;

namespace AdminSite.Startup
{
    public static class DependencyInjectionSetup
    {
        public static WebApplicationBuilder ConfigureDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<IInventoryService, InventoryService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IReferenceDataService, ReferenceDataService>();
            builder.Services.AddTransient<IOfferService, OfferService>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            if (builder.Environment.IsDevelopment())
            {
                builder.Services.AddTransient<IFileService, LocalFileService>();
            }
            else
            {
                builder.Services.AddTransient<IFileService, S3FileService>();
            }

            return builder;
        }
    }
}