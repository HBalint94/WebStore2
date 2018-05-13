using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using WebStore.Models;
using Xunit;

namespace WebShop.Test
{
    public class TestStartup
    {
        public TestStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<WebStoreContext>(options =>
                options.UseInMemoryDatabase("TestingDB"));


            services.AddIdentity<Customer, IdentityRole<int>>()
                .AddEntityFrameworkStores<WebStoreContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, WebStoreContext context)
        {
            app.UseAuthentication();
            app.UseMiddleware<AuthenticatedTestRequestMiddleware>(); // automatikus "bejelentkezés"

            app.UseMvc();
            
            
            TestDbInitializer.Initialize(context);
        }
    }
}
