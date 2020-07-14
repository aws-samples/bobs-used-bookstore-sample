using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BobBookstore.Data;
using BobBookstore.Models.Customer;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using Microsoft.AspNetCore.Identity;
using Amazon.Extensions.CognitoAuthentication;

namespace BobBookstore.Controllers
{
    public class AddressesController : Controller
    {
        
        private readonly UsedBooksContext _context;
        private readonly SignInManager<CognitoUser> _SignInManager;
        private readonly UserManager<CognitoUser> _userManager;
        public AddressesController( UsedBooksContext context, SignInManager<CognitoUser> SignInManager, UserManager<CognitoUser> userManager)
        {
           
            _context = context;
            _SignInManager = SignInManager;
            _userManager = userManager;
        }

        // GET: Addresses
        public async Task<IActionResult> Index()
        {
            
            var user = await _userManager.GetUserAsync(User);
            var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
            var customer = _context.Customer.Find(id);
            var address = from c in _context.Address
                          where c.Customer == customer
                          select c;




            return View(await address.ToListAsync());
        }

        // GET: Addresses/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .FirstOrDefaultAsync(m => m.Address_Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // GET: Addresses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Addresses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Address_Id,AddressLine1,AddressLine2,City,State,Country,ZipCode")] Address address)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.GetUserAsync(User);
                var id = user.Attributes[CognitoAttribute.Sub.AttributeName];
                var customer = _context.Customer.Find(id);
                address.Customer = customer;
                _context.Add(address);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        // GET: Addresses/Edit/5
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address.FindAsync(id);
            if (address == null)
            {
                return NotFound();
            }
            return View(address);
        }

        // POST: Addresses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Address_Id,AddressLine1,AddressLine2,City,State,Country,ZipCode")] Address address)
        {
            if (id != address.Address_Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (address.IsPrimary == true)
                    {
                        var user = await _userManager.GetUserAsync(User);
                        user.Attributes["custom:AddressLine1"] = address.AddressLine1;
                        user.Attributes["custom:AddressLine2"] = address.AddressLine2;
                        user.Attributes["custom:City"] = address.City;
                        user.Attributes["custom:State"] =address.State;
                        user.Attributes["custom:Country"] = address.Country;

                        user.Attributes["custom:ZipCode"] = Convert.ToString( address.ZipCode);

                        var result = await _userManager.UpdateAsync(user);
                    }
                    _context.Update(address);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AddressExists(address.Address_Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(address);
        }

        // GET: Addresses/Delete/5
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var address = await _context.Address
                .FirstOrDefaultAsync(m => m.Address_Id == id);
            if (address == null)
            {
                return NotFound();
            }

            return View(address);
        }

        // POST: Addresses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var address = await _context.Address.FindAsync(id);
            _context.Address.Remove(address);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AddressExists(long id)
        {
            return _context.Address.Any(e => e.Address_Id == id);
        }
    }
}
