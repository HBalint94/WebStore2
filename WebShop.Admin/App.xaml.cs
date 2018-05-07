using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WebShop.Admin.Model;
using WebShop.Admin.Persistence;
using WebShop.Admin.View;
using WebShop.Admin.ViewModel;

namespace WebShop.Admin
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IWebShopModel _model;
        private LoginViewModel _loginViewModel;
        private LoginWindow _loginView;
        private MainViewModel _mainViewModel;
        private MainWindow _mainView;
        private ProductEditor _editorView;

        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
            Exit += new ExitEventHandler(App_Exit);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new WebShopModel(new WebShopPersistence("http://localhost:63644/")); // megadjuk a szolgáltatás címét

            _loginViewModel = new LoginViewModel(_model);
            _loginViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _loginViewModel.LoginSuccess += new EventHandler(ViewModel_LoginSuccess);
            _loginViewModel.LoginFailed += new EventHandler(ViewModel_LoginFailed);

            _loginView = new LoginWindow();
            _loginView.DataContext = _loginViewModel;
            _loginView.Show();
        }

        public async void App_Exit(object sender, ExitEventArgs e)
        {
            if (_model.IsUserLoggedIn) // amennyiben be vagyunk jelentkezve, kijelentkezünk
            {
                await _model.LogoutAsync();
            }
        }

        private void ViewModel_LoginSuccess(object sender, EventArgs e)
        {
            _mainViewModel = new MainViewModel(_model);
            _mainViewModel.MessageApplication += new EventHandler<MessageEventArgs>(ViewModel_MessageApplication);
            _mainViewModel.ProductEditingStarted += new EventHandler(MainViewModel_ProductEditingStarted);
            _mainViewModel.ProductEditingFinished += new EventHandler(MainViewModel_ProductEditingFinished);
            _mainViewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);
            _mainViewModel.RentFinalizeStarted += new EventHandler(ViewModel_FinalizeRent);

            _mainView = new MainWindow();
            _mainView.DataContext = _mainViewModel;
            _mainView.Show();

            _loginView.Close();
        }

        private void ViewModel_LoginFailed(object sender, EventArgs e)
        {
            MessageBox.Show("A bejelentkezés sikertelen!", "WebShop", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void ViewModel_MessageApplication(object sender, MessageEventArgs e)
        {
            MessageBox.Show(e.Message, "WebShop", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }

        private void MainViewModel_ProductEditingStarted(object sender, EventArgs e)
        {
            _editorView = new ProductEditor(); // külön szerkesztő dialógus az épületekre
            _editorView.DataContext = _mainViewModel;
            _editorView.Show();
        }

        private void MainViewModel_ProductEditingFinished(object sender, EventArgs e)
        {
            _editorView.Close();
        }

        private void ViewModel_ExitApplication(object sender, System.EventArgs e)
        {
            Shutdown();
        }

        private void ViewModel_FinalizeRent(object sender, System.EventArgs e)
        {
            MessageBox.Show("A rendelést sikeresen elvégeztük", "WebShop", MessageBoxButton.OK, MessageBoxImage.Asterisk);
        }
    }
}
