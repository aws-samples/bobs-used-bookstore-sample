using AdminSite.Startup;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

builder.ConfigureServices();

builder.ConfigureAuthentication();

builder.ConfigureDependencyInjection();

builder.ConfigureLogging();

builder.ConfigureConfiguration();

var app = builder.Build();

await app.ConfigureMiddlewareAsync();

app.Run();