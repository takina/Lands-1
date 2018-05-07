//Se agrega este ensamblado , para que la aplicación sepa a que clase llamar dependiendo el sistema operativo
//en que se esté ejecutando
[assembly: Xamarin.Forms.Dependency(typeof(Lands.Droid.Implementations.Config))]

namespace Lands.Droid.Implementations
{
    using Interfaces;
    using SQLite.Net.Interop;

    public class Config : IConfig
    {
        private string directoryDB;
        private ISQLitePlatform platform;

        public string DirectoryDB
        {
            get
            {
                if (string.IsNullOrEmpty(directoryDB))
                {
                    //Obteniendo el directorio
                    directoryDB = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
                }

                return directoryDB;
            }
        }

        public ISQLitePlatform Platform
        {
            get
            {
                if (platform == null)
                {
                    //Obteniendo las librerías
                    platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
                }

                return platform;

            }
        }
    }
}
