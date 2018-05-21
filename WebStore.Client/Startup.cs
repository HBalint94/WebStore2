using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.Models;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace WebStore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));


            services.AddIdentity<Customer, IdentityRole<int>>()
                .AddEntityFrameworkStores<WebStoreContext>() // EF használata a TravelAgencyContext entitás kontextussal
                .AddDefaultTokenProviders(); // Alapértelmezett token generátor használata (pl. SecurityStamp-hez)

            services.Configure<IdentityOptions>(options =>
            {
                // Jelszó komplexitására vonatkozó konfiguráció
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 3;

                // Hibás bejelentkezés esetén az (ideiglenes) kizárásra vonatkozó konfiguráció
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // Felhasználókezelésre vonatkozó konfiguráció
                options.User.RequireUniqueEmail = true;
            });


            // Dependency injections
            services.AddTransient<IStoreService,StoreService>();
            services.AddSingleton<ApplicationState>();
            services.AddTransient<ShoppingCartService>();

            
            services.AddMvc();


            // Munkamenetkezelés beállítása
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15); // max. 15 percig él a munkamenet
                options.Cookie.HttpOnly = true; // kliens oldali szkriptek ne férjenek hozzá a sütihez 
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            // Authentikációs szolgáltatás használata
            app.UseAuthentication();
            // Munkamentek használata
            app.UseSession();
            // Statikus fájlkiszolgálás használata
            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            DbInitializer.Initialize(app);

        }
    }
}
