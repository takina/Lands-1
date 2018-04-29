namespace Lands
{
    using Xamarin.Forms;
    using Views;

    public partial class App : Application
	{
        #region Properties
        //Esta propiedad se pone acá, para que pueda usarse desde cualquier
        //parte de la aplicación
        public static object Navigator
        {
            get;
            internal set;
        }
        #endregion

        #region Constructor
        public App ()
		{
			InitializeComponent();

			this.MainPage = new MasterPage();
            //this.MainPage = new NavigationPage(new LoginPage());
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
