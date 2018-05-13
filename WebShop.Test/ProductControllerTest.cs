using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using WebShop.Data;
using WebStore.Models;
using Xunit;

namespace WebShop.Test
{
    public class ProductControllerTest : IClassFixture<ServerClientFixture>
    {
        private readonly ServerClientFixture _fixture;

        public ProductControllerTest(ServerClientFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async void Test_ProductItems_WithInvalidCategoryId_ReturnsEmptyList()
        {
            int id = 45000;

            var response = await _fixture.Client.GetAsync("api/Product/" + id);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
        [Fact]
        public async void Test_ListProductItems()
        {
            var response = await _fixture.Client.GetAsync("api/Product");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(responseString);

            Assert.NotNull(responseObject);
            Assert.True(responseObject.Any());
        }

        [Fact]    
        public async void Test_PostProduct()
        {
            ProductDTO newprod = new ProductDTO
            {
                Producer = "Apple",
                ModellNumber = 1000,
                Description = "IphoneX2",
                CategoryId = 1,
                Price = 205000,
                Inventory = 15,
                Available = true,
            };

            // Act
            var content = new StringContent(JsonConvert.SerializeObject(newprod), Encoding.UTF8, "application/json");
            var response = await _fixture.Client.PostAsync("api/Product", content);

            // Assert
            response.EnsureSuccessStatusCode();

            Assert.NotNull(_fixture.Context.Products.FirstOrDefault(i => i.ModellNumber == 1000));
        }
    }
}
