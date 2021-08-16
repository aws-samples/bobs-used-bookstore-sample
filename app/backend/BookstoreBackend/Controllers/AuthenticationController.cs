using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Configuration;

namespace BookstoreBackend.Controllers
{
    public class AuthenticationController : Controller
    {
        public IConfiguration Configuration { get; }

        public AuthenticationController(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public async Task SignOut()
        {
            // deletes the cookies from the local machine
            await HttpContext.SignOutAsync("Cookies");
            // the cognito logout URL; should be the same as the signout URL on cognito
            string redirecturl = "https://localhost:5000/Home/Index";
            // Cognito  user pool domain URL 
            string url = $"https://{Constants.DomainName}.auth.{Configuration["Authentication:Cognito:Region"]}.amazoncognito.com";
            string logout_url = $"{url}/logout?client_id={Configuration["AWS:UserPoolClientId"]}&logout_uri={redirecturl}";
            // redirects to the response from the cognito logout endpoint
            HttpContext.Response.Redirect(logout_url);
        }
    }
}
