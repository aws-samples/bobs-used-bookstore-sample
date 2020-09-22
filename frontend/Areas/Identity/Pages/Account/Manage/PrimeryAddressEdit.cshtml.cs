using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using BobBookstore.Data;
using BobBookstore.Models.Customer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BobBookstore.Areas.Identity.Pages.Account.Manage
{
    

    [AllowAnonymous]
    public class PrimeryAddressEditModel : PageModel
    {
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UsedBooksContext _context;

        [BindProperty]
        public AccountModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public string Username { get; set; }


        public PrimeryAddressEditModel(
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager,
            UsedBooksContext context)
        {
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _signInManager = signInManager;
            _context = context;
        }

        public class AccountModel
        {
           
           
            [Display(Name = "Address Line 1")]
            public string AddressLine1 { get; set; }
            [Display(Name = "Address Line 2")]
            public string AddressLine2 { get; set; }
            [Display(Name = "City")]
            public string City { get; set; }

            [Display(Name = "State")]
            public string State { get; set; }

            [Display(Name = "Country")]
            public string Country { get; set; }
            [Range(0, int.MaxValue, ErrorMessage = "Please enter valid integer Number")]
            [Display(Name = "Zip Code")]
            public string ZipCode { get; set; }

        }
        private async Task LoadAsync(CognitoUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            
            var addressLine1 = user.Attributes["custom:AddressLine1"];
            var addressLine2 = user.Attributes["custom:AddressLine2"];
            var city = user.Attributes["custom:City"];
            var state = user.Attributes["custom:State"];
            var Country = user.Attributes["custom:Country"];
            var zipCode = user.Attributes["custom:ZipCode"];


            Username = userName;

            Input = new AccountModel()
            {
                
                AddressLine1 = addressLine1,
                AddressLine2 = addressLine2,
                City = city,
                State = state,
                Country = Country,
                ZipCode = zipCode


            };
            

            if (Input.AddressLine1 == "default")
            {
                Input.AddressLine1 = "";
            }
            if (Input.AddressLine2 == "default")
            {
                Input.AddressLine2 = "";
            }
            if (Input.City == "default")
            {
                Input.City = "";
            }
            if (Input.State == "default")
            {
                Input.State = "";
            }
            if (Input.Country == "default")
            {
                Input.Country = "";
            }
            if (Input.ZipCode == "default")
            {
                Input.ZipCode = "";
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

            
            if (string.IsNullOrWhiteSpace(Input.AddressLine1))
            {
                Input.AddressLine1 = "default";
            }
            if (string.IsNullOrWhiteSpace(Input.AddressLine2))
            {
                Input.AddressLine2 = "default";
            }
            if (string.IsNullOrWhiteSpace(Input.City))
            {
                Input.City = "default";
            }
            if (string.IsNullOrWhiteSpace(Input.State))
            {
                Input.State = "default";
            }
            if (string.IsNullOrWhiteSpace(Input.Country))
            {
                Input.Country = "default";
            }
            if (string.IsNullOrWhiteSpace(Input.ZipCode))
            {
                Input.ZipCode = "default";
            }

            
            user.Attributes["custom:AddressLine1"] = Input.AddressLine1;
            user.Attributes["custom:AddressLine2"] = Input.AddressLine2;
            user.Attributes["custom:City"] = Input.City;
            user.Attributes["custom:State"] = Input.State;
            user.Attributes["custom:Country"] = Input.Country;

            user.Attributes["custom:ZipCode"] = Input.ZipCode;

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)

            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            //input customer information into the DB
            else
            {

                var id = user.Attributes[CognitoAttribute.Sub.AttributeName];

                //get customer information
                var recentCustomer = await _context.Customer.FindAsync(id);
                var address = from m in _context.Address
                              where m.Customer==recentCustomer && m.IsPrimary==true
                              select m;
                
                Address recentAddress = new Address();
                foreach (var add in address)
                {
                    //addressId = add.Address_Id;
                    recentAddress = add;
                }
                //var recentAddress =await  _context.Address.FindAsync(addressId);
                recentAddress.AddressLine1 = Input.AddressLine1;
                recentAddress.AddressLine2 = Input.AddressLine2;
                recentAddress.City = Input.City;
                recentAddress.Country = Input.Country;
                //recentAddress.ZipCode = Convert.ToInt32(Input.ZipCode);
                recentAddress.State = Input.State;
                recentAddress.Customer = recentCustomer;
                recentAddress.IsPrimary = true;

                _context.Update(recentAddress);


                await _context.SaveChangesAsync();


            }

            return RedirectToPage();
        }
    }
}
