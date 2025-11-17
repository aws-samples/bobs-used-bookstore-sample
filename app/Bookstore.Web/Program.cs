using Microsoft.AspNetCore.Builder;

using Bookstore.Web.Startup;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureConfiguration();

builder.ConfigureServices();

builder.ConfigureAuthentication();

builder.ConfigureDependencyInjection();

var app = builder.Build();

await app.ConfigureMiddlewareAsync();

app.Run();