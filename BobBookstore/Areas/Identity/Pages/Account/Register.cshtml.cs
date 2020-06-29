using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;

namespace BobBookstore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly CognitoUserPool _pool;

        public RegisterModel(
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager,
            ILogger<RegisterModel> logger,
            CognitoUserPool pool)
        {
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _signInManager = signInManager;
            _logger = logger;
            _pool = pool;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [Display(Name = "Username")]
            public string UserName { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [Display(Name = "First name")]
            public string FirstName { get; set; }
            [Required]
            [Display(Name = "Last name")]
            public string LastName { get; set; }
        }

        public void OnGet(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");
            if (ModelState.IsValid)
            {
                var user = _pool.GetUser(Input.UserName);
                user.Attributes.Add(CognitoAttribute.Email.AttributeName, Input.Email);
                user.Attributes.Add(CognitoAttribute.Address.AttributeName, "default");
                user.Attributes.Add("custom:AddressLine1", "default");
                user.Attributes.Add("custom:AddressLine2", "default");
                user.Attributes.Add("custom:City", "default");
                user.Attributes.Add("custom:State", "default");
                user.Attributes.Add("custom:Country", "default");
                user.Attributes.Add("custom:ZipCode", "default");
                user.Attributes.Add(CognitoAttribute.BirthDate.AttributeName, "0000-00-00");
                user.Attributes.Add(CognitoAttribute.Gender.AttributeName, "default");
                user.Attributes.Add(CognitoAttribute.NickName.AttributeName, "default");
                user.Attributes.Add(CognitoAttribute.PhoneNumber.AttributeName, "+01234567890");
                user.Attributes.Add(CognitoAttribute.FamilyName.AttributeName, Input.LastName);
                user.Attributes.Add(CognitoAttribute.GivenName.AttributeName, Input.FirstName);
                var existuser = await _userManager.FindByEmailAsync(Input.Email);
                if (existuser!=null)
                {
                    //throw new InvalidOperationException("E-mail address has been used");
                    ModelState.AddModelError(string.Empty, "E-mail address has been used");
                    return Page();
                }

                var result = await _userManager.CreateAsync(user, Input.Password);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    await _signInManager.SignInAsync(user, isPersistent: false);

                    
                    return RedirectToPage("./ConfirmAccount");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}