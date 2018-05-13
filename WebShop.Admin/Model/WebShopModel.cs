using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebShop.Admin.Persistence;
using WebShop.Data;

namespace WebShop.Admin.Model
{
    public class WebShopModel : IWebShopModel
    {
        
        /// <summary>
        /// Adat állapotjelzések.
        /// </summary>
        private enum DataFlag
        {
            Create,
            Update,
            Delete
        }

        private IWebShopPersistence _persistence;
        private List<ProductDTO> _products;
        private Dictionary<ProductDTO, DataFlag> _productFlags;
        private List<CategoryDTO> _categories;
        private List<RentDTO> _rents;

        /// <summary>
        /// Modell példányosítása.
        /// </summary>
        /// <param name="persistence">A perzisztencia.</param>
        public WebShopModel(IWebShopPersistence persistence)
        {
            if (persistence == null)
                throw new ArgumentNullException(nameof(persistence));

            IsUserLoggedIn = false;
            _persistence = persistence;
        }

        /// <summary>
        /// Kategóriák lekérdezése.
        /// </summary>
        public IReadOnlyList<CategoryDTO> Categories
        {
            get { return _categories; }
        }

        /// <summary>
        /// Productok lekérdezése.
        /// </summary>
        public IReadOnlyList<ProductDTO> Products
        {
            get { return _products; }
        }

        ///<summary>
        /// Rendelések lekérdezése.
        /// </summary>
        public IReadOnlyList<RentDTO> Rents
        {
            get { return _rents; }
        }

        /// <summary>
        /// Bejelentkezettség lekérdezése.
        /// </summary>
        public Boolean IsUserLoggedIn { get; private set; }

        /// <summary>
        /// Product változás eseménye.
        /// </summary>
        public event EventHandler<ProductEventArgs> ProductChanged;

        public event EventHandler<RentEventArgs> RentChanged;

        /// <summary>
        /// Product hozzáadása.
        /// </summary>
        /// <param name="product">A product.</param>
        public void CreateProduct(ProductDTO product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));
            if (_products.Contains(product))
                throw new ArgumentException("The product already in the collection.", nameof(product));

            product.ModellNumber = (_products.Count > 0 ? _products.Max(b => b.ModellNumber) : 0) + 1; // generálunk egy új, ideiglenes azonosítót (nem fog átkerülni a szerverre)
            _productFlags.Add(product, DataFlag.Create);
            _products.Add(product);
        }

        /// <summary>
        /// Product módosítása.
        /// </summary>
        /// <param name="product">A product.</param>
        public void UpdateProduct(ProductDTO product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            // keresés azonosító alapján
            ProductDTO productToModify = _products.FirstOrDefault(b => b.ModellNumber == product.ModellNumber);

            if (productToModify == null)
                throw new ArgumentException("The product does not exist.", nameof(product));

             
        productToModify.Producer = product.Producer;
        productToModify.Description = product.Description;
        productToModify.CategoryId = product.CategoryId;
        productToModify.Price = product.Price;
        productToModify.Inventory = product.Inventory;
        productToModify.Available = product.Available;
        
            // külön állapottal jelezzük, ha egy adat újonnan hozzávett
            if (_productFlags.ContainsKey(productToModify) && _productFlags[productToModify] == DataFlag.Create)
            {
                _productFlags[productToModify] = DataFlag.Create;
            }
            else
            {
                _productFlags[productToModify] = DataFlag.Update;
            }

            // jelezzük a változást
            OnProductChanged(product.ModellNumber);
        }

        public void FinalizeRent(RentDTO rentDTO)
        {
            if(rentDTO == null)
            {
                throw new ArgumentNullException(nameof(rentDTO));
            }
            RentDTO rentToModify = _rents.FirstOrDefault(r => r.Id == rentDTO.Id);
            if(rentToModify == null)
            {
                throw new ArgumentException("The product does not exist.", nameof(rentDTO));
            }

            rentToModify.Performed = true;

            _persistence.DoneRentAsync(rentToModify);

            // jelezzük a változást
            OnRentChanged(rentDTO.Id);
        }

        /// <summary>
        /// Product törlése.
        /// </summary>
        /// <param name="product">A product.</param>
        public void DeleteProduct(ProductDTO product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product));

            // keresés azonosító alapján
           ProductDTO productToDelete = _products.FirstOrDefault(b => b.ModellNumber == product.ModellNumber);

            if (productToDelete == null)
                throw new ArgumentException("The product does not exist.", nameof(product));

            // külön kezeljük, ha egy adat újonnan hozzávett (ekkor nem kell törölni a szerverről)
            if (_productFlags.ContainsKey(productToDelete) && _productFlags[productToDelete] == DataFlag.Create)
                _productFlags.Remove(productToDelete);
            else
                _productFlags[productToDelete] = DataFlag.Delete;

            _products.Remove(productToDelete);
        }
        

        /// <summary>
        /// Adatok betöltése.
        /// </summary>
        public async Task LoadAsync()
        {
            // adatok
            _products = (await _persistence.ReadProductsAsync()).ToList();
            _categories = (await _persistence.ReadCategoriesAsync()).ToList();
            _rents = (await _persistence.ReadRentsAsync()).ToList();

            // állapotjelzések
            _productFlags = new Dictionary<ProductDTO, DataFlag>();
        }

        /// <summary>
        /// Adatok mentése.
        /// </summary>
        public async Task SaveAsync()
        {
            // épületek
            List<ProductDTO> productsToSave = _productFlags.Keys.ToList();

            foreach (ProductDTO product in productsToSave)
            {
                Boolean result = true;

                // az állapotjelzőnek megfelelő műveletet végezzük el
                switch (_productFlags[product])
                {
                    case DataFlag.Create:
                        result = await _persistence.CreateProductAsync(product);
                        break;
                    case DataFlag.Delete:
                        result = await _persistence.DeleteProductAsync(product);
                        break;
                    case DataFlag.Update:
                        result = await _persistence.UpdateProductAsync(product);
                        break;
                }

                if (!result)
                    throw new InvalidOperationException("Operation " + _productFlags[product] + " failed on building " + product.ModellNumber);

                // ha sikeres volt a mentés, akkor törölhetjük az állapotjelzőt
                _productFlags.Remove(product);
            }
        }

           

        /// <summary>
        /// Bejelentkezés.
        /// </summary>
        /// <param name="userName">Felhasználónév.</param>
        /// <param name="userPassword">Jelszó.</param>
        public async Task<Boolean> LoginAsync(String userName, String userPassword)
        {
            IsUserLoggedIn = await _persistence.LoginAsync(userName, userPassword);
            return IsUserLoggedIn;
        }

        /// <summary>
        /// Kijelentkezés.
        /// </summary>
        public async Task<Boolean> LogoutAsync()
        {
            if (!IsUserLoggedIn)
                return true;

            IsUserLoggedIn = !(await _persistence.LogoutAsync());

            return IsUserLoggedIn;
        }

        private void OnProductChanged(int modellNumber)
        {
            if (ProductChanged != null)
                ProductChanged(this, new ProductEventArgs { PorductModellNumber = modellNumber });
        }

        private void OnRentChanged(int rentId)
        {
            if(RentChanged != null)
            {
                RentChanged(this, new RentEventArgs { RentId = rentId });
            }
        }
    }

}

