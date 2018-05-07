using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using WebShop.Data;

namespace WebShop.Admin.Persistence
{
    public class WebShopPersistence : IWebShopPersistence
    {

        private HttpClient _client;

        public WebShopPersistence(String baseAddress)
        {
            _client = new HttpClient(); // a szolgáltatás kliense
            _client.BaseAddress = new Uri(baseAddress); // megadjuk neki a címet
        }

        public async Task<bool> CreateProductAsync(ProductDTO product)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/product/", product); // az értékeket azonnal JSON formátumra alakítjuk
                product.ModellNumber = (await response.Content.ReadAsAsync<ProductDTO>()).ModellNumber; // a válaszüzenetben megkapjuk a végleges azonosítót
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> DeleteProductAsync(ProductDTO product)
        {
            try
            {
                HttpResponseMessage response = await _client.DeleteAsync("api/product/" + product.ModellNumber);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> LoginAsync(string userName, string userPassword)
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/account/login/" + userName + "/" + userPassword);
                return response.IsSuccessStatusCode; // a művelet eredménye megadja a bejelentkezés sikeressségét
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/account/logout");
                return !response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<CategoryDTO>> ReadCategoriesAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/category/");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<CategoryDTO>>();
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<ProductDTO>> ReadProductsAsync()
        {
            try
            {
                // a kéréseket a kliensen keresztül végezzük
                HttpResponseMessage response = await _client.GetAsync("api/product/");
                if (response.IsSuccessStatusCode) // amennyiben sikeres a művelet
                {
                    IEnumerable<ProductDTO> products = await response.Content.ReadAsAsync<IEnumerable<ProductDTO>>();
                    // a tartalmat JSON formátumból objektumokká alakítjuk

                    return products;
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<IEnumerable<RentDTO>> ReadRentsAsync()
        {
            try
            {
                HttpResponseMessage response = await _client.GetAsync("api/rent/");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<IEnumerable<RentDTO>>();
                }
                else
                {
                    throw new PersistenceUnavailableException("Service returned response: " + response.StatusCode);
                }
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        public async Task<bool> UpdateProductAsync(ProductDTO product)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/product/", product);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }

        // Véglegesítem a rendelést ( updatelem ) 
        public async Task<bool> DoneRentAsync(RentDTO rent)
        {
            try
            {
                HttpResponseMessage response = await _client.PutAsJsonAsync("api/rent/", rent);
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                throw new PersistenceUnavailableException(ex);
            }
        }
    }
}
