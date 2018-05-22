using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace WebShop.Client.Models
{
    public class ShoppingCartService
    {
        private readonly WebStoreContext context;
        private readonly HttpContext httpContext;

        public string ShoppingCartId { get; set; }

        public string ShoppingCartUserName { get; set; }

        public List<ShoppingCartItem> ShoppingCartItems { get; set; }

        public ShoppingCartService(WebStoreContext context, IHttpContextAccessor httpContext)
        {
            this.context = context;
            this.httpContext = httpContext.HttpContext;
            ShoppingCartItems = new List<ShoppingCartItem>();
        }
        
        // Lekérem a sessionből a listát, belerakom az új productot és visszarakom a sessionbe.
        public void AddShoppingCartItem(int productModellNumber)
        {
            bool isIn = false;
            List<ShoppingCartItem> currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            if(currentShoppingCartItems == null)
            {
                SessionExtensions.Set<List<ShoppingCartItem>>(this.httpContext.Session, "shoppingCart", new List<ShoppingCartItem>());
                currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            }
            // megkell nézni, hogy volt e már a listában ilyen
            for (int i = 0; i < currentShoppingCartItems.Count; i++)
            {
                if(currentShoppingCartItems[i].ProductModellNumber == productModellNumber)
                {
                    currentShoppingCartItems[i].Quantity++;
                    isIn = true;
                }
            }
            if(isIn == false)
            {
                Product item = context.Products.FirstOrDefault(c => c.ModellNumber == productModellNumber);
                ShoppingCartItem scItem = new ShoppingCartItem(item.ModellNumber, 1, item.Description);
                currentShoppingCartItems.Add(scItem);
            }
            SessionExtensions.Set<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart", currentShoppingCartItems);
            
        }

        // Lekérem a sessionből az adott terméket
        public ShoppingCartItem GetShoppingCartItem(int productModellNumber)
        {
            List<ShoppingCartItem> currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            if (currentShoppingCartItems == null)
            {
                SessionExtensions.Set<List<ShoppingCartItem>>(this.httpContext.Session, "shoppingCart", new List<ShoppingCartItem>());
                currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            }
            for (int i = 0; i < currentShoppingCartItems.Count; i++)
            {
                if(currentShoppingCartItems[i].ProductModellNumber == productModellNumber)
                {
                    return currentShoppingCartItems[i];
                }
            }
            // ha nem volt ilyen termék
            return null;
        }

        void SaveChanges()
        {
            context.SaveChanges();
        }

        public void RemoveShoppingCartItem(int productModellNumber)
        {
            List<ShoppingCartItem> currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            // megkell nézni, hogy létezik e egyáltalán
            for (int i = 0; i < currentShoppingCartItems.Count; i++)
            {
                if (currentShoppingCartItems[i].ProductModellNumber == productModellNumber)
                {
                    currentShoppingCartItems[i].Quantity--;
                    if(currentShoppingCartItems[i].Quantity == 0)
                    {
                        currentShoppingCartItems.Remove(currentShoppingCartItems[i]);
                    }
                    
                }
            }
            SessionExtensions.Set<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart", currentShoppingCartItems);

        }

        public List<ShoppingCartItem> GetShoppingCartItems()
        {
            List<ShoppingCartItem> currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            if (currentShoppingCartItems == null)
            {
                SessionExtensions.Set<List<ShoppingCartItem>>(this.httpContext.Session, "shoppingCart", new List<ShoppingCartItem>());
                currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            }
            return currentShoppingCartItems;
        }

        public void ClearCart()
        {
            List<ShoppingCartItem> currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            currentShoppingCartItems.RemoveAll(c => c.Quantity != 0);
            SessionExtensions.Set<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart", currentShoppingCartItems);
        }

        public int GetShoppingCartTotal()
        {
            int total = 0;
            List<ShoppingCartItem> currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            if (currentShoppingCartItems == null)
            {
                SessionExtensions.Set<List<ShoppingCartItem>>(this.httpContext.Session, "shoppingCart", new List<ShoppingCartItem>());
                currentShoppingCartItems = SessionExtensions.Get<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart");
            }
            for (int i = 0; i < currentShoppingCartItems.Count; i++)
            {
                Product currentProduct = context.Products.SingleOrDefault(p => p.ModellNumber == currentShoppingCartItems[i].ProductModellNumber);
                total += currentProduct.Price*currentShoppingCartItems[i].Quantity;
            }
            return total;
        }

        public void SetShoppingCart(List<ShoppingCartItem> items)
        {
            SessionExtensions.Set<List<ShoppingCartItem>>(httpContext.Session, "shoppingCart", items);
        }

    }
}
