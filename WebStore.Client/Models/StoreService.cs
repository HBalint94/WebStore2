using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebStore.Models
{
    public class StoreService : IStoreService
    {
        private readonly WebStoreContext context;

        private ShoppingCartService shoppingCartService;

        private readonly UserManager<Customer> userManager;

        public StoreService(ShoppingCartService shoppingCartService, WebStoreContext context, UserManager<Customer> userManager)
        {
            this.shoppingCartService = shoppingCartService;
            this.context = context;
            this.userManager = userManager;
        }

        public IEnumerable<Category> Categories => context.Categories;

        public IEnumerable<Product> Products => context.Products;

        public IEnumerable<Rent> Rents => context.Rents;

        public IEnumerable<RentProductConnection> RentProductConnection => context.RentProductConnections;

        public int GetRentedProductCountInARent(int prodModellNumber, int rentId)
        {
            if (prodModellNumber == 0 || rentId == 0) return 0;

            RentProductConnection rent= context.RentProductConnections.FirstOrDefault(rentProdConnection => rentProdConnection.ProductModellNumber == prodModellNumber && rentProdConnection.RentId == rentId);

            return rent.CountProduct;
        }

        public IEnumerable<RentProductConnection> GetRentProductionConnectionWithRentId(int rentId)
        {
            if (rentId == 0 || !context.RentProductConnections.Any(prodRentConn => prodRentConn.RentId == rentId)) return null;

            return context.RentProductConnections.Where(prodRentConn => prodRentConn.RentId == rentId);
        }

        public IEnumerable<RentProductConnection> GetRentProductionConnectionWithProdModellNumber(int prodModellNumber)
        {
            if (prodModellNumber == 0|| !context.RentProductConnections.Any(prodRentConn => prodRentConn.ProductModellNumber == prodModellNumber)) return null;

            return context.RentProductConnections.Where(prodRentConn => prodRentConn.ProductModellNumber == prodModellNumber);
        }

        public Product GetProduct(int prodModellNumber)
        {
            if (prodModellNumber == 0|| !context.Products.Any(prod => prod.ModellNumber == prodModellNumber)) return null;

            return context.Products.FirstOrDefault(prod => prod.ModellNumber == prodModellNumber);
        }

        public Category GetCategory(int categoryId)
        {
            if (categoryId == 0 || !context.Categories.Any(category => category.Id == categoryId)) return null;

            return context.Categories.FirstOrDefault(category => category.Id == categoryId);
        }

        public Rent GetRent(int rentId)
        {
            if (rentId ==  0 || !context.Rents.Any(rent => rent.Id == rentId)) return null;

            return context.Rents.FirstOrDefault(rent => rent.Id == rentId);
        }

        public int GetPriceOfRent(int rentId)
        {
            // ha nem is létezik ezzel az id val rendelés akkor 0 t adunk vissza.
            if (rentId == 0 || !context.Rents.Any(rent => rent.Id == rentId)) return 0;
            int price = 0;
            IEnumerable<RentProductConnection> connections = context.RentProductConnections.Where(rent => rent.RentId == rentId);
            foreach(RentProductConnection rent in connections)
            {
                price = GetProduct(rent.ProductModellNumber).Price * rent.CountProduct;
            }
            return price;
        }

        public IEnumerable<Product> GetProductsBasedOnCategoryId(int categoryId)
        {
            if (categoryId == 0) return null;

            return context.Products.Where(product => product.CategoryId == categoryId && product.Inventory != 0 && product.Available == true);
        }
        public IEnumerable<Product> GetAscendingOrderedByPriceProductsBasedOnCategoryId(int categoryId)
        {
            if (categoryId == 0) return null;

            return context.Products.Where(product => product.CategoryId == categoryId && product.Inventory != 0 && product.Available).OrderBy(p => p.Price);
        }

        public IEnumerable<Product> GetDescendingOrderedByPriceProductsBasedOnCategoryId(int categoryId)
        {

            if (categoryId == 0) return null;

            return context.Products.Where(product => product.CategoryId == categoryId && product.Inventory != 0 && product.Available).OrderByDescending(p => p.Price);
        }
    

        void IStoreService.SaveChanges()
        {
            context.SaveChanges();
        }

        public RentViewModel NewRent()
        {

            List<ShoppingCartItem> items = shoppingCartService.GetShoppingCartItems();
            int totalPrice = shoppingCartService.GetShoppingCartTotal();
           
            RentViewModel rent = new RentViewModel { ShoppingCartItems = items, TotalPrice = totalPrice};

            return rent;
        }

        public Boolean SaveRentAsync(String userName, RentViewModel rent)
        {

            // ellenőrizzük az annotációkat
            if (!Validator.TryValidateObject(rent, new ValidationContext(rent, null, null), null))
                return false;

            // a felhasználót a név alapján betöltjük
            Customer customer = context.Users.FirstOrDefault(c => c.UserName == userName);
                

            if (customer == null)
                return false;

            List<Product> products = new List<Product>();
            for (int i = 0; i < rent.ShoppingCartItems.Count; i++)
            {
                Product currentProduct = context.Products.FirstOrDefault(p => p.ModellNumber == rent.ShoppingCartItems[i].ProductModellNumber);
                products.Add(currentProduct);
            }
            int totalPrice = shoppingCartService.GetShoppingCartTotal();
            context.Rents.Add(new Rent
            {
                CustomerId = customer.Id,
                TotalPrice = totalPrice,
                Performed = false
            });
            
            try
            {
                context.SaveChanges();
                SaveRentForProducts(products,rent);
            }
            catch (Exception)
            {
                // mentéskor lehet hiba
                return false;
            }

            // ha idáig eljuttottunk, minden sikeres volt
            return true;
        }
        public void SaveRentForProducts(List<Product> products,RentViewModel rent)
        {
            Rent lastRent = context.Rents.LastOrDefault();
            for (int i = 0; i < products.Count; i++)
            {
                context.RentProductConnections.Add(new RentProductConnection
                {
                    RentId = lastRent.Id,
                    ProductModellNumber = products[i].ModellNumber,
                    CountProduct = rent.ShoppingCartItems[i].Quantity
                });
            }
            context.SaveChanges();
        }

       
    }
}
