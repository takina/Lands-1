using GalaSoft.MvvmLight.Command;
using Lands.Helpers;
using Lands.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    //Esta clase es la encargada de manejar la navegación en el Menú de la aplicación
    public class MenuItemViewModel
    {
        #region Properties
        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion
        #region command

        public ICommand NavigateCommand
        {
            get
            {
                return new RelayCommand(Navigate);
            }
            

        }

        private void Navigate()
        {
            //Para ocultar el menú
            App.Master.IsPresented = false;

            if (this.PageName == "LoginPage")
            {
                //Cuando cerramos sesión, borramos los token guardados en la persistencia o memoria del celular
                Settings.Token = string.Empty;
                Settings.TokenType = string.Empty;

                //Por seguridad, también se deben borrar los toquen guardados en memoria de la aplicación,
                // en este caso de los token que están guardados en la MainViewModel
                var mainViewModel = MainViewModel.GetInstance();
                mainViewModel.Token = string.Empty;
                mainViewModel.TokenType = string.Empty;

                //Ya después se redigire a la vista Login
                Application.Current.MainPage = new NavigationPage(
                    new LoginPage());
               
            }
            else if(this.PageName == "MyProfilePage")
            {
                MainViewModel.GetInstance().MyProfile = new MyProfileViewModel();
                App.Navigator.PushAsync(new MyProfilePage());
            }
        }
        #endregion
    }
}
