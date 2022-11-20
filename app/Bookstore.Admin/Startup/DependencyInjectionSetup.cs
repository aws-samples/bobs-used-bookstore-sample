using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Bookstore.Data.Repository.Interface.WelcomePageInterface;
using Bookstore.Data.Repository.Interface.OrdersInterface;
using Bookstore.Data.Repository.Implementation;
using Bookstore.Data.Repository.Implementation.WelcomePageImplementation;
using Bookstore.Data.Repository.Interface.InventoryInterface;
using Bookstore.Data.Repository.Implementation.OrderImplementations;
using Bookstore.Data.Repository.Implementation.SearchImplementation;
using Bookstore.Data.Repository.Interface.NotificationsInterface;
using Bookstore.Data.Repository.Implementation.InventoryImplementation;
using Bookstore.Data.Repository.Interface.SearchImplementations;
using Bookstore.Data.Repository.Interface;
using Bookstore.Data.Repository.Implementation.NotificationsImplementation;
using Services;
using Microsoft.Extensions.Hosting;
using Bookstore.Services;

namespace AdminSite.Startup
{
    public static class DependencyInjectionSetup
    {
        public static WebApplicationBuilder ConfigureDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ISearchDatabaseCalls, SearchDatabaseCalls>();
            builder.Services.AddTransient<IExpressionFunction, ExpressionFunction>();
            builder.Services.AddTransient<IOrderDatabaseCalls, OrderDatabaseCalls>();
            builder.Services.AddTransient<IRekognitionNPollyRepository, RekognitionNPollyRepository>();
            builder.Services.AddTransient<ISearchRepository, SearchRepository>();
            builder.Services.AddTransient<IOrderRepository, OrderRepository>();
            builder.Services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddTransient<IOrderStatusRepository, OrderStatusRepository>();
            builder.Services.AddTransient<INotifications, Notifications>();
            builder.Services.AddTransient<ICustomAdminPage, CustomAdmin>();

            builder.Services.AddTransient<IInventoryService, InventoryService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IReferenceDataService, ReferenceDataService>();
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