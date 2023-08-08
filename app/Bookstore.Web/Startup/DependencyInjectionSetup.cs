using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Bookstore.Domain.Customers;
using Bookstore.Domain.Books;
using Bookstore.Domain;
using Bookstore.Domain.Offers;
using Bookstore.Domain.Orders;
using Bookstore.Domain.Carts;
using Bookstore.Domain.ReferenceData;
using Bookstore.Data.Repositories;
using Bookstore.Data.FileServices;
using Bookstore.Domain.Addresses;
using Bookstore.Data.ImageValidationServices;
using Bookstore.Data.ImageResizeService;

namespace Bookstore.Web.Startup
{
    public static class DependencyInjectionSetup
    {
        public static WebApplicationBuilder ConfigureDependencyInjection(this WebApplicationBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<IBookService, BookService>();
            builder.Services.AddTransient<IOrderService, OrderService>();
            builder.Services.AddTransient<IReferenceDataService, ReferenceDataService>();
            builder.Services.AddTransient<IOfferService, OfferService>();
            builder.Services.AddTransient<ICustomerService, CustomerService>();
            builder.Services.AddTransient<IAddressService, AddressService>();
            builder.Services.AddTransient<IShoppingCartService, ShoppingCartService>();
            builder.Services.AddTransient<IImageResizeService, ImageResizeService>();

            builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
            builder.Services.AddScoped<IAddressRepository, AddressRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IOfferRepository, OfferRepository>();
            builder.Services.AddScoped<IShoppingCartRepository, ShoppingCartRepository>();
            builder.Services.AddScoped<IOrderRepository, OrderRepository>();
            builder.Services.AddScoped<IReferenceDataRepository, ReferenceDataRepository>();

            builder.Services.AddScoped(typeof(IPaginatedList<>), typeof(PaginatedList<>));

            if (builder.Configuration["Services:FileService"] == "AWS")
            {
                builder.Services.AddTransient<IFileService, S3FileService>();
            }
            else
            {
                builder.Services.AddTransient<IFileService, LocalFileService>(x => new LocalFileService(builder.Environment.WebRootPath));
            }

            if (builder.Configuration["Services:ImageValidationService"] == "AWS")
            {
                builder.Services.AddTransient<IImageValidationService, RekognitionImageValidationService>();
            }
            else
            {
                builder.Services.AddTransient<IImageValidationService, LocalImageValidationService>();
            }

            return builder;
        }
    }
}