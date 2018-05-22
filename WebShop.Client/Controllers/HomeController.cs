using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Client.Models;

namespace WebShop.Client.Controllers
{
    public class HomeController : BaseController
    {
       //private readonly IStoreService storeService;

        public HomeController(IStoreService storeService, ApplicationState applicationState)
            :base(storeService,applicationState)
        { }
        
        

        // kezdő oldal
        public IActionResult Index()
        {
            // egy egy terméket küldök minden categoriából.

            IList<Product> prods = storeService.Products.ToList();
            IList<Product> randomProducts = new List<Product>();

            randomProducts.Add(prods[0]);
            for(int i = 0; i < prods.Count; i++)
            {
                if (!isThereAnyProductinRandomProductsWhichHasTheSameCategoryId(prods[i],randomProducts))
                {
                    randomProducts.Add(prods[i]);
                }
            }
            return View("Index",randomProducts);
        }
        private Boolean isThereAnyProductinRandomProductsWhichHasTheSameCategoryId(Product p,IList<Product> l)
        {
            for (int i = 0; i < l.Count; i++)
            {
                if (l[i].CategoryId == p.CategoryId)
                {
                    return true;
                }
            }
            return false;
        }

        public IActionResult Products(int categoryId)
        {
            // termékek listája
            IList<Product> products = storeService.GetProductsBasedOnCategoryId(categoryId).ToList();

            return View("Products", products);
        }

        public IActionResult GetAscendingOrderedProductsByPrice(int categoryId)
        {
            IList<Product> products = storeService.GetAscendingOrderedByPriceProductsBasedOnCategoryId(categoryId).ToList();
            return View("Products", products);
        }
        public IActionResult GetDescendingOrderedProductsByPrice(int categoryId)
        {
            IList<Product> products = storeService.GetDescendingOrderedByPriceProductsBasedOnCategoryId(categoryId).ToList();
            return View("Products", products);
        }

        public IActionResult PageUp(int categoryId)
        {
            List<Product> prods = storeService.GetProductsBasedOnCategoryId(categoryId).ToList();
            List<Product> newListOfProducts = prods.GetRange(20, prods.Count - 20);
            return View("Products", newListOfProducts);
        }
    }
}
