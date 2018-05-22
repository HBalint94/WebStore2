using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Client.Models;

namespace WebShop.Client.Controllers
{
    public class RentController : BaseController
    {

        private ShoppingCartService shoppingCartService;
        private readonly UserManager<Customer> userManager;

        public RentController(IStoreService storeService, ApplicationState applicationState,ShoppingCartService shoppingCartService, UserManager<Customer> userManager)
           : base(storeService, applicationState)
        {
            this.shoppingCartService = shoppingCartService;
            this.userManager = userManager;
        }


        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            // Megnézem, hogy van e valami a kosárban, mert ha nincs akkor értelmetlen a rendelés
            if(shoppingCartService.GetShoppingCartItems().Count == 0)
            {
                return RedirectToAction("Index", "ShoppingCart");
            }

            // létrehozunk egy foglalást csak az alapadatokkal (apartman, dátumok)
            RentViewModel rent = storeService.NewRent();

            if (rent == null) // ha nem sikerül (nem volt azonosító)
                return RedirectToAction("Index", "Home"); // visszairányítjuk a főoldalra

            // ha a felhasználó be van jelentkezve
            if (User.Identity.IsAuthenticated)
            {
                Customer customer = await userManager.FindByNameAsync(User.Identity.Name); 

                // akkor az adatait közvetlenül is betölthetjük
                if (customer != null)
                {
                    rent.CustomerAddress = customer.Address;
                    rent.CustomerEmail = customer.Email;
                    rent.CustomerName = customer.Name;
                    rent.CustomerPhoneNumber = customer.PhoneNumber;
                }
                // így nem kell újra megadnia
            }

            return View("Index", rent);
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // védelem XSRF támadás ellen
        public async Task<IActionResult> IndexAsync(RentViewModel rent)
        {
            if (rent == null)
                return RedirectToAction("Index", "Home");

            Customer customer;
            // bejelentkezett felhasználó esetén nem kell felvennünk az új felhasználót
            if (User.Identity.IsAuthenticated)
            {
                customer = await userManager.FindByNameAsync(User.Identity.Name);
                
            }
            else
            {
                customer = new Customer
                {
                    UserName = "user" + Guid.NewGuid(),
                    Email = rent.CustomerEmail,
                    Name = rent.CustomerName,
                    Address = rent.CustomerAddress,
                    PhoneNumber = rent.CustomerPhoneNumber
                };
                var result = await userManager.CreateAsync(customer);
                if (!result.Succeeded)
                {
                    ModelState.AddModelError("", "A foglalás rögzítése sikertelen, kérem próbálja újra!");
                    return View("Index", rent);
                }
            }

            if (!storeService.SaveRentAsync(customer.UserName, rent))
            {
                ModelState.AddModelError("", "A foglalás rögzítése sikertelen, kérem próbálja újra!");
                return View("Index", rent);
            }

            rent.TotalPrice = shoppingCartService.GetShoppingCartTotal();
            ViewBag.Message = "A foglalását sikeresen rögzítettük!";
            shoppingCartService.SetShoppingCart(new List<ShoppingCartItem>());
            return View("Result",rent);
        }

    }
}
