namespace Lands.ViewModels
{
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;
    using Services;
    using Views;
    using Xamarin.Forms;
    using Helpers;
    using System;

    public class LoginViewModel : BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private string email;
        private string password;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public string Email
        {
            get { return this.email; }
            set { SetValue(ref this.email, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { SetValue(ref this.password, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public bool IsRemembered
        {
            get;
            set;
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Constructors
        public LoginViewModel()
        {
            this.apiService = new ApiService();
            this.dataService = new DataService();

            this.IsRemembered = true;
            this.IsEnabled = true;

            this.Email = "jzuluaga55@hotmail.com";
            this.Password = "123456";
        }
        #endregion

        #region Commands
        public ICommand LoginCommand
        {
            get
            {
                return new RelayCommand(Login);
            }
        }

        public ICommand RegisterCommand
        {
            get
            {
                return new RelayCommand(Register);
            }
        }

        private async void Register()
        {
            //Antes de yo instanciar la página, debo intanciar la view model que acompaña a la page
            MainViewModel.GetInstance().Register = new RegisterViewModel();

            
            await Application.Current.MainPage.Navigation.PushAsync(new RegisterPage());
        }

        private async void Login()
        {
            if (string.IsNullOrEmpty(this.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PasswordValidation,
                    Languages.Accept);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;

            var connection = await this.apiService.CheckConnection();

            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.Accept);
                return;
            }

            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var token = await this.apiService.GetToken(
                apiSecurity, 
                this.Email, 
                this.Password);

            if (token == null)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.SomethingWrong,
                    Languages.Accept);
                return;
            }

            if (string.IsNullOrEmpty(token.AccessToken))
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    token.ErrorDescription,
                    Languages.Accept);
                this.Password = string.Empty;
                return;
            }

            
            var user = await this.apiService.GetUserByEmail(
                apiSecurity,
                "/api",
                "/Users/GetUserByEmail",
                this.Email);

            var userLocal = Converter.ToUserLocal(user);

            var mainViewModel = MainViewModel.GetInstance();
            //guardando el token en memoria de la aplicación
            mainViewModel.Token = token.AccessToken;
            mainViewModel.TokenType = token.TokenType;
            mainViewModel.User = userLocal;

            if (this.IsRemembered)
            {
                //Guardando los token en persistencia, es decir en la memoria del teléfono
                Settings.Token = token.AccessToken;
                Settings.TokenType = token.TokenType;

                //Guardando los datos del usuario en local, para que se siga mostrando en la aplicación todos los datos
                //cuando estemos logueados, entonces borramos el usuario que haya creado e insertamos el usuario con el que nos loguiemos o
                //nos vayamos a loguear
                this.dataService.DeleteAllAndInsert(userLocal);
            }
        
            mainViewModel.Lands = new LandsViewModel();
            Application.Current.MainPage = new MasterPage();//Esta forma de navegación es para que no se pueda devolver con el
            //botón de atrás
            //await Application.Current.MainPage.Navigation.PushAsync(new LandsPage());

            this.IsRunning = false;
            this.IsEnabled = true;

            this.Email = string.Empty;
            this.Password = string.Empty;
        }
        #endregion
    }
}