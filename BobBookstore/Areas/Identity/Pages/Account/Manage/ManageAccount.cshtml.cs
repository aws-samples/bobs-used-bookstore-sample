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
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;

namespace BobBookstore.Areas.Identity.Pages.Account.Manage
{
    [AllowAnonymous]
    public class ManageAccountModel : PageModel
    {
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly UsedBooksContext _context;

        [BindProperty]
        public AccountModel Input { get; set; }

        [TempData]
        public string StatusMessage { get; set; }
        public string Username { get; set; }


        public ManageAccountModel(
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
            var address = user.Attributes[CognitoAttribute.Address.AttributeName];
            var birthDate = user.Attributes[CognitoAttribute.BirthDate.AttributeName];
            var gender = user.Attributes[CognitoAttribute.Gender.AttributeName];
            var nickName = user.Attributes[CognitoAttribute.NickName.AttributeName];
            var phoneNumber = user.Attributes[CognitoAttribute.PhoneNumber.AttributeName];
            var familyName = user.Attributes[CognitoAttribute.FamilyName.AttributeName];
            var givenName = user.Attributes[CognitoAttribute.GivenName.AttributeName];
            var addressLine1 = user.Attributes["custom:AddressLine1"];
            var addressLine2 = user.Attributes["custom:AddressLine2"];
            var city = user.Attributes["custom:City"];
            var state = user.Attributes["custom:State"];
            var Country = user.Attributes["custom:Country"];
            var zipCode = user.Attributes["custom:ZipCode"];
            

            Username = userName;

            Input = new AccountModel()
            {
                Address = address,
                BirthDate = birthDate,
                GivenName = givenName,
                Gender = gender,
                NickName = nickName,
                PhoneNumber = phoneNumber,
                FamilyName = familyName,
                AddressLine1=addressLine1,
                AddressLine2= addressLine2,
                City=city,
                State=state,
                Country=Country,
                ZipCode=zipCode
                
                
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
           
            if (Input.AddressLine1=="default")
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

           //update cognito
            user.Attributes[CognitoAttribute.Address.AttributeName] = Input.Address;
            user.Attributes[CognitoAttribute.BirthDate.AttributeName] = Input.BirthDate;
            user.Attributes[CognitoAttribute.Gender.AttributeName] = Input.Gender;
            user.Attributes[CognitoAttribute.NickName.AttributeName] = Input.NickName;
            user.Attributes[CognitoAttribute.PhoneNumber.AttributeName] = Input.PhoneNumber;
            user.Attributes[CognitoAttribute.FamilyName.AttributeName] = Input.FamilyName;
            user.Attributes[CognitoAttribute.GivenName.AttributeName] = Input.GivenName;
            user.Attributes["custom:AddressLine1"] = Input.AddressLine1;
            user.Attributes["custom:AddressLine2"] = Input.AddressLine2;
            user.Attributes["custom:City"] = Input.City;
            user.Attributes["custom:State"] = Input.State;
            user.Attributes["custom:Country"] = Input.Country;
            
            user.Attributes["custom:ZipCode"] = Input.ZipCode;
            
            var result = await _userManager.UpdateAsync(user);
            if(!result.Succeeded)
           
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            //input and update customer address information into the DB
            else
            {

                
                var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                //get customer information
                var recentCustomer = await _context.Customer.FindAsync(id);
                var address = from m in _context.Address
                              where m.Customer==recentCustomer && m.IsPrimary==true
                              select m;
                if (address == null)
                {
                    var newAddress = new Address { 
                        IsPrimary = true, 
                        AddressLine1 = Input.AddressLine1, 
                        AddressLine2 = Input.AddressLine2, 
                        City = Input.City, 
                        Country = Input.Country, 
                        State = Input.State,
                        Customer = recentCustomer, 
                        ZipCode = Convert.ToInt32(Input.ZipCode) 
                    };
                    _context.Add(newAddress);

                }
                else
                {
                    Address recentAddress = new Address();
                    foreach (var add in address)
                    {
                        
                        recentAddress = add;
                    }
                    //var recentAddress =await  _context.Address.FindAsync(addressId);
                    recentAddress.AddressLine1 = Input.AddressLine1;
                    recentAddress.AddressLine2 = Input.AddressLine2;
                    recentAddress.City = Input.City;
                    recentAddress.Country = Input.Country;
                    
                    recentAddress.ZipCode = Convert.ToInt32(Input.ZipCode);
                    recentAddress.State = Input.State;
                    recentAddress.Customer = recentCustomer;
                    recentAddress.IsPrimary = true;
                    _context.Update(recentAddress);

                }
                await _context.SaveChangesAsync();

                
            }
            
            return RedirectToPage();
        }
    }
}
