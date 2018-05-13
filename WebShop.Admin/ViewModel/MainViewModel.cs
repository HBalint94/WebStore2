using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebShop.Admin.Model;
using WebShop.Admin.Persistence;
using WebShop.Data;

namespace WebShop.Admin.ViewModel
{

    /// <summary>
    /// A nézetmodell típusa.
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private IWebShopModel _model;
        private ObservableCollection<ProductDTO> _products;
        private ObservableCollection<CategoryDTO> _categories;
        private ObservableCollection<RentDTO> _rents;
        private ProductDTO _selectedProduct;
        private RentDTO _selectedRent;
        private Boolean _isLoaded;

        /// <summary>
        /// Termékek lekérdezése.
        /// </summary>
        public ObservableCollection<ProductDTO> Products
        {
            get { return _products; }
            private set
            {
                if (_products != value)
                {
                    _products = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Kategóriák lekérdezése.
        /// </summary>
        public ObservableCollection<CategoryDTO> Categories
        {
            get { return _categories; }
            private set
            {
                if (_categories != value)
                {
                    _categories = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Kategóriák lekérdezése.
        /// </summary>
        public ObservableCollection<RentDTO> Rents
        {
            get { return _rents; }
            private set
            {
                if (_rents != value)
                {
                    _rents = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Betöltöttség lekérdezése.
        /// </summary>
        public Boolean IsLoaded
        {
            get { return _isLoaded; }
            private set
            {
                if (_isLoaded != value)
                {
                    _isLoaded = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Kijelölt termék lekérdezése, vagy beállítása.
        /// </summary>
        public ProductDTO SelectedProduct
        {
            get { return _selectedProduct; }
            set
            {
                if (_selectedProduct != value)
                {
                    _selectedProduct = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Kijelölt rendelés 
        /// </summary>
        public RentDTO SelectedRent
        {
            get { return _selectedRent; }
            set
            {
                if (_selectedRent != value)
                {
                    _selectedRent = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Szerkesztett épület lekérdezése.
        /// </summary>
        public ProductDTO EditedProduct { get; private set; }

        public DelegateCommand FinalizeRentCommand { get; private set; }

        /// <summary>
        /// Termék létrehozás parancsának lekérdezése.
        /// </summary>
        public DelegateCommand CreateProductCommand { get; private set; }
        
        /// <summary>
        /// Módosítás parancsának lekérdezése.
        /// </summary>
        public DelegateCommand UpdateProductCommand { get; private set; }

        /// <summary>
        /// Termék törlés parancsának lekérdezése.
        /// </summary>
        public DelegateCommand DeleteProductCommand { get; private set; }
        
        /// <summary>
        /// Változtatások mentése parancsának lekérdezése.
        /// </summary>
        public DelegateCommand SaveChangesCommand { get; private set; }

        /// <summary>
        /// Változtatások elvetése parancsának lekérdezése.
        /// </summary>
        public DelegateCommand CancelChangesCommand { get; private set; }

        /// <summary>
        /// Kilépés parancsának lekérdezése.
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// Betöltés parancsának lekérdezése.
        /// </summary>
        public DelegateCommand LoadCommand { get; private set; }

        /// <summary>
        /// Mentés parancsának lekérdezése.
        /// </summary>
        public DelegateCommand SaveCommand { get; private set; }

        /// <summary>
        /// Alkalmazásból való kilépés eseménye.
        /// </summary>
        public event EventHandler ExitApplication;

        /// <summary>
        /// Termék szerkesztés elindításának eseménye.
        /// </summary>
        public event EventHandler ProductEditingStarted;

        /// <summary>
        /// Termék szerkesztés befejeztének eseménye.
        /// </summary>
        public event EventHandler ProductEditingFinished;

        /// <summary>
        /// Rendelés véglegesítésének eseménye
        /// </summary>
        public event EventHandler RentFinalizeStarted;

        /// <summary>
        /// Nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell.</param>
        public MainViewModel(IWebShopModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            _model = model;
            _model.ProductChanged += Model_ProductChanged;
            _model.RentChanged += Model_RentChanged;
            _isLoaded = false;

            FinalizeRentCommand = new DelegateCommand(param => 
            {
                FinalizeRent(param as RentDTO);
            });
            CreateProductCommand = new DelegateCommand(param =>
            {
                EditedProduct = new ProductDTO();  // a szerkesztett termék új lesz
                OnProductEditingStarted();
            });
            UpdateProductCommand = new DelegateCommand(param => UpdateProduct(param as ProductDTO));
            DeleteProductCommand = new DelegateCommand(param => DeleteProduct(param as ProductDTO));
            SaveChangesCommand = new DelegateCommand(param => SaveChanges());
            CancelChangesCommand = new DelegateCommand(param => CancelChanges());
            LoadCommand = new DelegateCommand(param => LoadAsync());
            SaveCommand = new DelegateCommand(param => SaveAsync());
            ExitCommand = new DelegateCommand(param => OnExitApplication());
        }


        private void FinalizeRent(RentDTO rent)
        {
            if (rent == null)
                return;

            SelectedRent = rent;
            OnRentFinalizeStarted();
        }

        /// <summary>
        /// termék frissítése.
        /// </summary>
        /// <param name="product">A termék.</param>
        private void UpdateProduct(ProductDTO product)
        {
            if (product == null)
                return;

            EditedProduct = new ProductDTO
            {
                Id = product.Id,
                Description = product.Description,
                Price = product.Price,
                Inventory = product.Inventory,
                ModellNumber = product.ModellNumber,
                Available = product.Available,
                Producer = product.Producer,
                CategoryId = product.CategoryId
            }; // a szerkesztett épület adatait áttöltjük a kijelöltből

            OnProductEditingStarted();
        }

        /// <summary>
        /// Termék törlése.
        /// </summary>
        /// <param name="product">Az épület.</param>
        private void DeleteProduct(ProductDTO product)
        {
            if (product == null || !Products.Contains(product))
                return;

            Products.Remove(product);

            _model.DeleteProduct(product);
        }
       
        /// <summary>
        /// Változtatások mentése.
        /// </summary>
        private void SaveChanges()
        {
            // ellenőrzések
            if (String.IsNullOrEmpty(EditedProduct.Description))
            {
                OnMessageApplication("A termék leírása nincs megadva!");
                return;
            }
            if (EditedProduct.Price == null)
            {
                OnMessageApplication("A termék ára nincs megadva!");
                return;
            }
            if (EditedProduct.CategoryId == null)
            {
                OnMessageApplication("A termék kategóriája nincs megadva!");
                return;
            }


            // mentés
            if (EditedProduct.Id == 0) // ha új az épület
            {
                _model.CreateProduct(EditedProduct);
                Products.Add(EditedProduct);
                SelectedProduct = EditedProduct;
            }
            else // ha már létezik az épület
            {
                _model.UpdateProduct(EditedProduct);
            }

            EditedProduct = null;

            OnProductEditingFinished();
            
        }

        /// <summary>
        /// Változtatások elvetése.
        /// </summary>
        private void CancelChanges()
        {
            EditedProduct = null;
            OnProductEditingFinished();
        }

        /// <summary>
        /// Betöltés végrehajtása.
        /// </summary>
        private async void LoadAsync()
        {
            try
            {
                await _model.LoadAsync();
                Products = new ObservableCollection<ProductDTO>(_model.Products); // az adatokat egy követett gyűjteménybe helyezzük
                Categories = new ObservableCollection<CategoryDTO>(_model.Categories);
                Rents = new ObservableCollection<RentDTO>(_model.Rents);
                IsLoaded = true;
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A betöltés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        /// <summary>
        /// Mentés végreahajtása.
        /// </summary>
        private async void SaveAsync()
        {
            try
            {
                await _model.SaveAsync();
                OnMessageApplication("A mentés sikeres!");
            }
            catch (PersistenceUnavailableException)
            {
                OnMessageApplication("A mentés sikertelen! Nincs kapcsolat a kiszolgálóval.");
            }
        }

        /// <summary>
        /// termék megváltozásának eseménykezelése.
        /// </summary>
        private void Model_ProductChanged(object sender, ProductEventArgs e)
        {
            Int32 index = Products.IndexOf(Products.FirstOrDefault(product => product.ModellNumber == e.PorductModellNumber));
            Products.RemoveAt(index); // módosítjuk a kollekciót
            Products.Insert(index, _model.Products[index]);

            SelectedProduct = Products[index]; // és az aktuális terméket
        }

        private void Model_RentChanged(object sender, RentEventArgs e)
        {
            Int32 index = Rents.IndexOf(Rents.FirstOrDefault(rent => rent.Id == e.RentId));
            Rents.RemoveAt(index);
            Rents.Insert(index, _model.Rents[index]);

            SelectedRent = Rents[index];
        }

        /// <summary>
        /// Alkalmazásból való kilépés eseménykiváltása.
        /// </summary>
        private void OnExitApplication()
        {
            if (ExitApplication != null)
                ExitApplication(this, EventArgs.Empty);
        }

        /// <summary>
        /// Termék szerkesztés elindításának eseménykiváltása.
        /// </summary>
        private void OnProductEditingStarted()
        {
            if (ProductEditingStarted != null)
                ProductEditingStarted(this, EventArgs.Empty);
        }

        private void OnRentFinalizeStarted()
        {
            if (RentFinalizeStarted != null)
            {
                if (MessageBox.Show("Biztos, hogy véglegesíti", "WebShop", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    RentFinalizeStarted(this, EventArgs.Empty);
                    _model.FinalizeRent(SelectedRent);
                }
            }
        }

        /// <summary>
        /// Termék szerkesztés befejeztének eseménykiváltása.
        /// </summary>
        private void OnProductEditingFinished()
        {
            if (ProductEditingFinished != null)
                ProductEditingFinished(this, EventArgs.Empty);
        }
    }
}
