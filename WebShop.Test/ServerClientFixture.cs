using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using WebStore.Models;

namespace WebShop.Test
{

    public class ServerClientFixture : IDisposable
    {
        public TestServer Server { get; private set; }
        public HttpClient Client { get; private set; }
        public WebStoreContext Context { get; private set; }

        public ServerClientFixture()
        {
            // Arrange
            Server = new TestServer(new WebHostBuilder()
                .UseStartup<TestStartup>());

            Context = Server.Host.Services.GetRequiredService<WebStoreContext>();
            Client = Server.CreateClient();
        }

        public void Dispose()
        {
            Server?.Dispose();
            Client?.Dispose();
        }
    }
}
