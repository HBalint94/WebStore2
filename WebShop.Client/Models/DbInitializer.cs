using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Client.Models
{
    public static class DbInitializer
    {
        private static WebStoreContext context;
        public static void Initialize(IApplicationBuilder app)
        {

            context = app.ApplicationServices.GetRequiredService<WebStoreContext>();

            context.Database.EnsureCreated();
            

            if (context.Categories.Any())
            {
                return; // Az adatbázist már inicializáltnak vesszük, ha létezik kategoria
            }

            SeedCategories();
            SeedProducts();
        
        }

        private static void SeedCategories()
        {
            var categories = new Category[]
            {
                new Category {Name = "Telefon"},
                new Category {Name = "Mosógép"},
                new Category {Name = "Laptop"},
                new Category {Name = "Hangszoró"},
                new Category {Name = "TV"},
                new Category {Name = "Porszívó"},
                new Category {Name = "Kábelek"}
            };
            foreach (Category c in categories)
            {
                context.Categories.Add(c);
            }
            context.SaveChanges();
        }

        private static void SeedProducts()
        {
            var products = new Product[]
            {
                new Product
                {
                    Producer = "Apple",
                    ModellNumber = 124,
                    Description = "Iphone8",
                    CategoryId = 1,
                    Price = 200000,
                    Inventory = 10,
                    Available = true,
                },
                new Product
                {
                    Producer = "Apple",
                    ModellNumber = 125,
                    Description = "IphoneX",
                    CategoryId = 1,
                    Price = 300000,
                    Inventory = 5,
                    Available = true,
                },
                new Product
                {
                    Producer = "Apple",
                    ModellNumber = 126,
                    Description = "Iphone6S",
                    CategoryId = 1,
                    Price = 140000,
                    Inventory = 15,
                    Available = true,
                },
                new Product
                {
                    Producer = "Apple",
                    ModellNumber = 127,
                    Description = "IphoneSE",
                    CategoryId = 1,
                    Price = 90000,
                    Inventory = 20,
                    Available = true,
                },
                 new Product
{
Producer = "Apple",
ModellNumber = 128,
Description = "Iphone4",
CategoryId = 1,
Price = 30000,
Inventory = 10,
Available = true,
},
new Product
{
Producer = "Apple",
ModellNumber = 129,
Description = "Iphone4/S",
CategoryId = 1,
Price = 45000,
Inventory = 7,
Available = true,
},
new Product
{
Producer = "Apple",
ModellNumber = 130,
Description = "Iphone5",
CategoryId = 1,
Price = 50000,
Inventory = 15,
Available = true,
},
new Product
{
Producer = "Apple",
ModellNumber = 131,
Description = "Iphone5/S",
CategoryId = 1,
Price = 60000,
Inventory = 18,
Available = true,
},
new Product
{
Producer = "Apple",
ModellNumber = 132,
Description = "Iphone6",
CategoryId = 1,
Price = 75000,
Inventory = 20,
Available = true,
},
new Product
{
Producer = "Apple",
ModellNumber = 133,
Description = "Iphone7",
CategoryId = 1,
Price = 150000,
Inventory = 30,
Available = true,
},
new Product
{
Producer = "Samsung",
ModellNumber = 140,
Description = "Samsung Galaxy S4",
CategoryId = 1,
Price = 30000,
Inventory = 4,
Available = true,
},
new Product
{
Producer = "Samsung",
ModellNumber = 141,
Description = "Samsung Galaxy S5",
CategoryId = 1,
Price = 40000,
Inventory = 15,
Available = true,
},
new Product
{
Producer = "Samsung",
ModellNumber = 142,
Description = "Samsung Galaxy S5 Edge",
CategoryId = 1,
Price = 60000,
Inventory = 40,
Available = true,
},
new Product
{
Producer = "Samsung",
ModellNumber = 143,
Description = "Samsung Galaxy S6",
CategoryId = 1,
Price = 100000,
Inventory = 10,
Available = true,
},
new Product
{
Producer = "Samsung",
ModellNumber = 144,
Description = "Samsung Galaxy S6 Edge",
CategoryId = 1,
Price = 120000,
Inventory = 5,
Available = true,
},
new Product
{
Producer = "Samsung",
ModellNumber = 145,
Description = "Samsung Galaxy S7",
CategoryId = 1,
Price = 140000,
Inventory = 6,
Available = true,
},
new Product
{
Producer = "Samsung",
ModellNumber = 146,
Description = "Samsung Galaxy S7 Edge",
CategoryId = 1,
Price = 160000,
Inventory = 4,
Available = true,
},
new Product
{
Producer = "Samsung",
ModellNumber = 147,
Description = "Samsung Galaxy S8",
CategoryId = 1,
Price = 180000,
Inventory = 10,
Available = true,
},
new Product
{
Producer = "Samsung",
ModellNumber = 148,
Description = "Samsung Galaxy S8 Edge",
CategoryId = 1,
Price = 200000,
Inventory = 15,
Available = true,
},
new Product
{
Producer = "Sony",
ModellNumber = 160,
Description = "Sony XPeria Z",
CategoryId = 1,
Price = 60000,
Inventory = 5,
Available = true,
},
new Product
{
Producer = "Sony",
ModellNumber = 161,
Description = "Sony XPeria Z6",
CategoryId = 1,
Price = 70000,
Inventory = 10,
Available = true,
},
                new Product
                {
                    Producer = "Indesit",
                    ModellNumber = 10,
                    Description = "IWSC 51051 C ECO mosógép",
                    CategoryId = 2,
                    Price = 56000,
                    Inventory = 11,
                    Available = true,
                },
                new Product
                {
                    Producer = "Indesit",
                    ModellNumber = 11,
                    Description = "IWSC 10000 C ECO mosógép",
                    CategoryId = 2,
                    Price = 45000,
                    Inventory = 12,
                    Available = true,
                }

            };
            foreach(Product p in products)
            {
                context.Products.Add(p);
            }

            context.SaveChanges();
        }
        
        

    }
}
