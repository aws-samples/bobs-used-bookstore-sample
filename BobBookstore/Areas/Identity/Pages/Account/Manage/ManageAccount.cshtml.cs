using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace BobBookstore.Areas.Identity.Pages.Account.Manage
{
    [AllowAnonymous]
    public class ManageAccountModel : PageModel
    {
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly SignInManager<CognitoUser> _signInManager;

        [BindProperty]
        public AccountModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public string Username { get; set; }


        public ManageAccountModel(
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager)
        {
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _signInManager = signInManager;
        }

        public class AccountModel
        {
            //[Required]
            [Display(Name ="Address")]
            public string Address { get; set; }
            //[Required]
            [Display(Name = "Birth Date ")]
            public string BirthDate { get; set; }
            //[Required]
            [Display(Name = "Gender")]
            public string Gender { get; set; }
            //[Required]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            //[Required]
            [Display(Name = "Last Name")]
            public string FamilyName { get; set; }
            //[Required]
            [Display(Name = "First Name")]
            public string GivenName { get; set; }
            //[Required]
            [Display(Name = "Nick Name")]
            public string NickName { get; set; }
            


        }
        private async Task LoadAsync(CognitoUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var address = user.Attributes[CognitoAttribute.Address.AttributeName];
            var birthDate = user.Attributes[CognitoAttribute.BirthDate.AttributeName];
            var gender = user.Attributes[CognitoAttribute.Gender.AttributeName];
            var nickName = user.Attributes[CognitoAttribute.NickName.AttributeName];
            var phoneNumber = user.Attributes[CognitoAttribute.PhoneNumber.AttributeName];
            var familyName = user.Attributes[CognitoAttribute.FamilyName.AttributeName];
            var givenName = user.Attributes[CognitoAttribute.GivenName.AttributeName];
           
            

            Username = userName;
            
            Input = new AccountModel()
            {
                Address = address,
                BirthDate=birthDate,
                GivenName=givenName,
                Gender=gender,
                NickName=nickName,
                PhoneNumber=phoneNumber,
                FamilyName=familyName
            };
            if (Input.FamilyName == "default")
            {
                Input.FamilyName = "";
            }

            if (Input.Address == "default")
            {
                Input.Address = "";
            }
            if (Input.Gender == "default")
            {
                Input.Gender = string.Empty;
            }
            if (Input.NickName == "default")
            {
                Input.NickName = string.Empty;
            }
            if (Input.GivenName == "default")
            {
                Input.GivenName = "";
            }
            if (Input.BirthDate == "0000-00-00")
            {
                Input.BirthDate = "";
            }
            if (Input.PhoneNumber == "+01234567890")
            {
                Input.PhoneNumber = "";
            }
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

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            
            
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }
            
            if (string.IsNullOrWhiteSpace(Input.FamilyName))
            {
                Input.FamilyName = "default";
            }
            if (string.IsNullOrWhiteSpace(Input.Address))
            {
                Input.Address = "default";
            }
            if (string.IsNullOrWhiteSpace(Input.Gender))
            {
                Input.Gender = "default";
            }
            if (string.IsNullOrWhiteSpace(Input.GivenName))
            {
                Input.GivenName = "default";
            }
            if (string.IsNullOrWhiteSpace(Input.BirthDate))
            {
                Input.BirthDate = "0000-00-00";
            }
            if (string.IsNullOrWhiteSpace(Input.PhoneNumber))
            {
                Input.PhoneNumber = "+01234567890";
            }
            if (string.IsNullOrWhiteSpace(Input.NickName))
            {
                Input.NickName = "default";
            }
            user.Attributes[CognitoAttribute.Address.AttributeName] = Input.Address;
            user.Attributes[CognitoAttribute.BirthDate.AttributeName] = Input.BirthDate;
            user.Attributes[CognitoAttribute.Gender.AttributeName] = Input.Gender;
            user.Attributes[CognitoAttribute.NickName.AttributeName] = Input.NickName;
            user.Attributes[CognitoAttribute.PhoneNumber.AttributeName] = Input.PhoneNumber;
            user.Attributes[CognitoAttribute.FamilyName.AttributeName] = Input.FamilyName;
            user.Attributes[CognitoAttribute.GivenName.AttributeName] = Input.GivenName;
            
            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
           
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToPage();
        }
    }
}
