using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BobBookstore.Areas.Identity.Pages.Account.Manage
{
    public partial class IndexModel : PageModel
    {
        private readonly UserManager<CognitoUser> _userManager;
        private readonly SignInManager<CognitoUser> _signInManager;

        public IndexModel(
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            
            [EmailAddress]
            [Display(Name = "email")]
            public string EmailAddress { get; set; }
        }

        private async Task LoadAsync(CognitoUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var emailAddress = await _userManager.GetEmailAsync(user);
            
            Username = userName;

            Input = new InputModel
            {
                
                EmailAddress=emailAddress

            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            
            
            return Page();
        }

        
    }
}
