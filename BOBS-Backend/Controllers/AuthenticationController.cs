using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;

namespace BOBS_Backend.Controllers
{
    public class AuthenticationController : Controller
    {
        public async Task SignOut()
        {
            // deletes the cookies from the local machine
            await HttpContext.SignOutAsync("Cookies");
            // the cognito logout URL; should be the same as the signout URL on cognito
            string redirecturl = "https://localhost:44336/Home/logout";
            // Cognito  user pool domain URL 
            string url = "https://loginsampledomain.auth.us-west-2.amazoncognito.com/logout?client_id=5f2rnsqn8prl48cre728iqb5ut&logout_uri=";
            string logout_url = url + redirecturl;
            // redirects to the response from the cognito logout endpoint
            HttpContext.Response.Redirect(logout_url);




        }
    }
}
