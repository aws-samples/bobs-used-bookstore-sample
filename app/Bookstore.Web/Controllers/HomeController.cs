using System.Diagnostics;
using System;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Bookstore.Web.ViewModel;
using Bookstore.Domain.Books;
using System.Threading.Tasks;
using Bookstore.Web.ViewModel.Home;

namespace Bookstore.Web.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        private readonly IBookService bookService;
        private readonly IConfiguration configuration;

        public HomeController(IBookService bookService, IConfiguration configuration)
        {
            this.bookService = bookService;
            this.configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            var books = await bookService.ListBestSellingBooksAsync(4);
            var dbEndpoint = GetDatabaseEndpoint();

            return View(new HomeIndexViewModel(books, dbEndpoint));
        }

        private string GetDatabaseEndpoint()
        {
            var connString = configuration.GetConnectionString("BookstoreDbDefaultConnection");
            if (!string.IsNullOrEmpty(connString))
            {
                try
                {
                    var builder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(connString);
                    return $"{builder.DataSource}/{builder.InitialCatalog}";
                }
                catch
                {
                    return "Local SQL Server Express LocalDB";
                }
            }
            
            try
            {
                var dbSecretId = configuration["dbsecretsname"];
                if (!string.IsNullOrEmpty(dbSecretId))
                {
                    var secretsManagerClient = new Amazon.SecretsManager.AmazonSecretsManagerClient();
                    var response = secretsManagerClient.GetSecretValueAsync(new Amazon.SecretsManager.Model.GetSecretValueRequest
                    {
                        SecretId = dbSecretId
                    }).Result;
                    
                    var dbSecrets = System.Text.Json.JsonSerializer.Deserialize<dynamic>(response.SecretString);
                    var host = dbSecrets.GetProperty("host").GetString();
                    return $"{host}";
                }
            }
            catch
            {
                return "AWS RDS PostgreSQL";
            }
            
            return "Unknown";
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Search()
        {
            return RedirectToAction("Index", "Search");
        }

        public IActionResult Cart()
        {
            return RedirectToAction("Index", "ShoppingCart");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
