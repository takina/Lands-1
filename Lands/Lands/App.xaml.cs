namespace Lands
{
    using Xamarin.Forms;
    using Views;
    using Lands.Helpers;
    using Lands.ViewModels;
    using Lands.Services;
    using Models;

    public partial class App : Application
	{
        #region Properties
        //Esta propiedad se pone acá, para que pueda usarse desde cualquier
        //parte de la aplicación
        public static NavigationPage Navigator
        {
            get;
            internal set;
        }
        #endregion

        #region Constructor
        public App ()
		{
			InitializeComponent();
            //Cuando inicie la aplicación, se va a verificar si hay o no token
            //si no hay, lo redirige a la página de Login , para que se loguie la persona
            if (string.IsNullOrEmpty(Settings.Token))
            {
                this.MainPage = new NavigationPage(new LoginPage());
            }
            else
            {
                //Si hay token en la persistencia del telefono, entonces ponemos los token en memoria del teléfono
                //y redirigimos de una ves al contenido de la aplicación sin tener que loguearse
                var dataService = new DataService();
                var user = dataService.First<UserLocal>(false);
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = Settings.Token;
                mainViewModel.TokenType = Settings.TokenType;
                mainViewModel.User = user;
                mainViewModel.Lands = new LandsViewModel();
                Application.Current.MainPage = new MasterPage();
            }
            
        }
        #endregion

        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
