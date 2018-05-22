using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebShop.Client.Models;

namespace WebShop.Client
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
                .AddEntityFrameworkStores<WebStoreContext>() // EF haszn�lata a TravelAgencyContext entit�s kontextussal
                .AddDefaultTokenProviders(); // Alap�rtelmezett token gener�tor haszn�lata (pl. SecurityStamp-hez)

            services.Configure<IdentityOptions>(options =>
            {
                // Jelsz� komplexit�s�ra vonatkoz� konfigur�ci�
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = false;
                options.Password.RequiredUniqueChars = 3;

                // Hib�s bejelentkez�s eset�n az (ideiglenes) kiz�r�sra vonatkoz� konfigur�ci�
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // Felhaszn�l�kezel�sre vonatkoz� konfigur�ci�
                options.User.RequireUniqueEmail = true;
            });


            // Dependency injections
            services.AddTransient<IStoreService, StoreService>();
            services.AddSingleton<ApplicationState>();
            services.AddTransient<ShoppingCartService>();


            services.AddMvc();


            // Munkamenetkezel�s be�ll�t�sa
            services.AddDistributedMemoryCache();
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(15); // max. 15 percig �l a munkamenet
                options.Cookie.HttpOnly = true; // kliens oldali szkriptek ne f�rjenek hozz� a s�tihez 
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
            // Authentik�ci�s szolg�ltat�s haszn�lata
            app.UseAuthentication();
            // Munkamentek haszn�lata
            app.UseSession();
            // Statikus f�jlkiszolg�l�s haszn�lata
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
