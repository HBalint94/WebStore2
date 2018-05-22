using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebShop.Client.Models
{
    public class WebStoreContext : IdentityDbContext<Customer, IdentityRole<int>, int>
    {
        public WebStoreContext(DbContextOptions<WebStoreContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<Customer>().ToTable("Customers");
            // A felhasználói tábla alapértelemezett neve AspNetUsers lenne az adatbázisban, de ezt felüldefiniálhatjuk.
        }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Rent> Rents { get; set; }
    public DbSet<RentProductConnection> RentProductConnections { get; set; }
    
    }
}
