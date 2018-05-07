using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Data;

namespace WebShop.Admin.Persistence
{
    public interface IWebShopPersistence
    {
        /// <summary>
        /// Produktok olvasása.
        /// </summary>
        Task<IEnumerable<ProductDTO>> ReadProductsAsync();

        /// <summary>
        /// Kategóriák olvasása.
        /// </summary>
        Task<IEnumerable<CategoryDTO>> ReadCategoriesAsync();

        /// <summary>
        /// Rendelések olvasása.
        /// </summary>
        Task<IEnumerable<RentDTO>> ReadRentsAsync();

        /// <summary>
        /// product létrehozása.
        /// </summary>
        /// <param name="product">Product.</param>
        Task<Boolean> CreateProductAsync(ProductDTO product);

        /// <summary>
        /// Product módosítása.
        /// </summary>
        /// <param name="product">product.</param>
        Task<Boolean> UpdateProductAsync(ProductDTO product);

        /// <summary>
        /// Product törlése.
        /// </summary>
        /// <param name="product">Product.</param>
        Task<Boolean> DeleteProductAsync(ProductDTO product);

        /// <summary>
        /// Rent törlése.
        /// </summary>
        /// <param name="rent">Rent.</param>
        Task<bool> DoneRentAsync(RentDTO rent);

        /// <summary>
        /// Bejelentkezés.
        /// </summary>
        /// <param name="userName">Felhasználónév.</param>
        /// <param name="userPassword">Jelszó.</param>
        Task<Boolean> LoginAsync(String userName, String userPassword);

        /// <summary>
        /// Kijelentkezés.
        /// </summary>
        Task<Boolean> LogoutAsync();
    }

}
