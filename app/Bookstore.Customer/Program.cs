using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Bookstore.Customer.Startup;
using AdminSite.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

builder.ConfigureAuthentication();

builder.ConfigureDependencyInjection();

builder.ConfigureConfiguration();

var app = builder.Build();

await app.ConfigureMiddlewareAsync();

app.Run();