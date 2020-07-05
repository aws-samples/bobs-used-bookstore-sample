using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using BobBookstore.Data;
using BobBookstore.Models.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BobBookstore.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class ConfirmAccountModel : PageModel
    {
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly UsedBooksContext _context;
        public ConfirmAccountModel(UserManager<CognitoUser> userManager, UsedBooksContext context)
        {
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _context = context;
        }
        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "Code")]
            public string Code { get; set; }
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
                var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name).Value;

                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return NotFound($"Unable to load user with ID '{userId}'.");
                }

                var result = await _userManager.ConfirmSignUpAsync(user, Input.Code, true);

                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Error confirming account for user with ID '{userId}':");
                }
                else
                {
                    //this part is to add customer information into the DB,birthday need to be convert
                    var userName = await _userManager.GetUserNameAsync(user);
                    var firstName = user.Attributes[CognitoAttribute.GivenName.AttributeName];
                    var lastName = user.Attributes[CognitoAttribute.FamilyName.AttributeName];
                    var email = user.Attributes[CognitoAttribute.Email.AttributeName];
                    var dateOfBirth = user.Attributes[CognitoAttribute.BirthDate.AttributeName];
                    var phone = user.Attributes[CognitoAttribute.PhoneNumber.AttributeName];
                    var customer = new Customer()
                    {
                        Username = userName,
                        FirstName = firstName,
                        LastName = lastName,
                        Email = email,
                        Phone = phone
                    };
                    _context.Add(customer);
                    await _context.SaveChangesAsync();


                    return returnUrl != null ? LocalRedirect(returnUrl) : Page() as IActionResult;
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
