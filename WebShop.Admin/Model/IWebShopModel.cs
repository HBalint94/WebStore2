using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Data;

namespace WebShop.Admin.Model
{
    public interface IWebShopModel
    {
        /// <summary>
        /// Kategóriák lekérdezése.
        /// </summary>
        IReadOnlyList<CategoryDTO> Categories { get; }

        /// <summary>
        /// Termékek lekérdezése.
        /// </summary>
        IReadOnlyList<ProductDTO> Products { get; }

        IReadOnlyList<RentDTO> Rents { get; }

        /// <summary>
        /// Bejelentkezettség lekérdezése.
        /// </summary>
        Boolean IsUserLoggedIn { get; }

        /// <summary>
        ///  Termékváltozás eseménye
        /// </summary>
        event EventHandler<ProductEventArgs> ProductChanged;


        event EventHandler<RentEventArgs> RentChanged;
        /// <summary>
        /// Product létrehozása.
        /// </summary>
        /// <param name="product">A product.</param>
        void CreateProduct(ProductDTO product);

        /// <summary>
        /// Product módosítása.
        /// </summary>
        /// <param name="product">A product.</param>
        void UpdateProduct(ProductDTO product);

        /// <summary>
        /// product törlése.
        /// </summary>
        /// <param name="product">A product.</param>
        void DeleteProduct(ProductDTO porduct);

        void FinalizeRent(RentDTO rent);

        /// <summary>
        /// Adatok betöltése.
        /// </summary>
        Task LoadAsync();

        /// <summary>
        /// Adatok mentése.
        /// </summary>
        Task SaveAsync();

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
