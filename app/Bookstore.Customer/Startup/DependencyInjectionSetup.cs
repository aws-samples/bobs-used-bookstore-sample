using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Bookstore.Data.Repository.Implementation;
using Bookstore.Data.Repository.Interface.InventoryInterface;
using Bookstore.Data.Repository.Implementation.SearchImplementation;
using Bookstore.Data.Repository.Interface.NotificationsInterface;
using Bookstore.Data.Repository.Implementation.InventoryImplementation;
using Bookstore.Data.Repository.Interface.SearchImplementations;
using Bookstore.Data.Repository.Interface;
using Bookstore.Data.Repository.Implementation.NotificationsImplementation;
using Services;
using Microsoft.Extensions.Hosting;
using Bookstore.Services;
using Microsoft.AspNetCore.Http;

namespace Bookstore.Customer.Startup
{
    public static class DependencyInjectionSetup
    {
        public static WebApplicationBuilder ConfigureDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<ISearchDatabaseCalls, SearchDatabaseCalls>();
            builder.Services.AddTransient<IExpressionFunction, ExpressionFunction>();
            builder.Services.AddTransient<IRekognitionNPollyRepository, RekognitionNPollyRepository>();
            builder.Services.AddTransient<ISearchRepository, SearchRepository>();
            builder.Services.AddTransient<INotifications, Notifications>();
            builder.Services.AddTransient<IInventoryService, InventoryService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IReferenceDataService, ReferenceDataService>();
            builder.Services.AddTransient<IOfferService, OfferService>();
            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();
            builder.Services.AddTransient<IShoppingCartClientManager, ShoppingCartClientManager>();
            builder.Services.AddTransient<IBookSearch, BookSearchRepository>();
            builder.Services.AddTransient<IPriceSearch, PriceSearchRepository>();
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