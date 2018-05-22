using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Session;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Client.Models;

namespace WebShop.Client.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private readonly ShoppingCartService shoppingCartService;

        private readonly UserManager<Customer> _userManager;

        public ShoppingCartController( ApplicationState applicationState,
            UserManager<Customer> userManager,IStoreService storeService,ShoppingCartService service)
            :base(storeService,applicationState)
        {
            this.shoppingCartService = service;
            _userManager = userManager;

        }

        public ViewResult Index()
        {
 
            var shoppingViewModel = new ShoppingCartViewModel
            {
                ShoppingCartItems = shoppingCartService.GetShoppingCartItems(),
                ShoppingCartTotal = shoppingCartService.GetShoppingCartTotal()
            };

            return View(shoppingViewModel);
        }

        public RedirectToActionResult AddToShoppingCart(int productModellNumber)
        {
            // Ha van belépett felhasználó
            if (User.Identity.IsAuthenticated)
            {
                var selectedProduct = storeService.GetProduct(productModellNumber);
                if (selectedProduct != null)
                {
                    shoppingCartService.AddShoppingCartItem(productModellNumber);
                }
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Index", "Account");
            }
        }

        public RedirectToActionResult RemoveFromShoppingCart(int productModellNumber)
        {
            var selectedProduct = storeService.GetProduct(productModellNumber);
            if (selectedProduct != null)
            {
                shoppingCartService.RemoveShoppingCartItem(productModellNumber);
            }
            return RedirectToAction("Index");
        }
        public RedirectToActionResult RemoveAllFromShoppingCart()
        {
            shoppingCartService.ClearCart();
            return RedirectToAction("Index");
        }
    }
}
