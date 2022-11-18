using DataAccess.Repository.Implementation.InventoryImplementation;
using DataAccess.Repository.Implementation.NotificationsImplementations;
using DataAccess.Repository.Implementation.OrderImplementations;
using DataAccess.Repository.Implementation.SearchImplementation;
using DataAccess.Repository.Implementation.WelcomePageImplementation;
using DataAccess.Repository.Implementation;
using DataAccess.Repository.Interface.Implementations;
using DataAccess.Repository.Interface.InventoryInterface;
using DataAccess.Repository.Interface.NotificationsInterface;
using DataAccess.Repository.Interface.OrdersInterface;
using DataAccess.Repository.Interface.SearchImplementations;
using DataAccess.Repository.Interface.WelcomePageInterface;
using DataAccess.Repository.Interface;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace AdminSite.Startup
{
    public static class DependencyInjectionSetup
    {
        public static WebApplicationBuilder ConfigureDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddTransient<ISearchDatabaseCalls, SearchDatabaseCalls>();
            builder.Services.AddTransient<IExpressionFunction, ExpressionFunction>();
            builder.Services.AddTransient<IOrderDatabaseCalls, OrderDatabaseCalls>();
            builder.Services.AddTransient<IInventory, Inventory>();
            builder.Services.AddTransient<IRekognitionNPollyRepository, RekognitionNPollyRepository>();
            builder.Services.AddTransient<ISearchRepository, SearchRepository>();
            builder.Services.AddTransient<IOrderRepository, OrderRepository>();
            builder.Services.AddTransient<IOrderDetailRepository, OrderDetailRepository>();
            builder.Services.AddTransient<IOrderStatusRepository, OrderStatusRepository>();
            builder.Services.AddTransient<INotifications, Notifications>();
            builder.Services.AddTransient<ICustomAdminPage, CustomAdmin>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            return builder;
        }
    }
}