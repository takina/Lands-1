using GalaSoft.MvvmLight.Command;
using Lands.Helpers;
using Lands.Models;
using Lands.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Lands.ViewModels
{
    public class MyProfileViewModel: BaseViewModel
    {
        #region Services
        private ApiService apiService;
        private DataService dataService;
        #endregion

        #region Attributes
        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;//Este atributo de la clase ImageSource es necesario para poder manejar las fotos
        private MediaFile file;//Cada vez que tomemos una foto, se nos va a generar un archivo MediaFile
        #endregion



        #region Properties
        public UserLocal User
        {
            get;
            set;
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { SetValue(ref this.imageSource, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { SetValue(ref this.isRunning, value); }
        }

        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();//Inicializamos la librería de imágenes

            //Si tiene galerías y tiene fotos
            if (CrossMedia.Current.IsCameraAvailable &&
                CrossMedia.Current.IsTakePhotoSupported)
            {
                //Preguntamos de donde queremos tomar la imágen, si la queremos tomar de la galería de fotos del celu
                //o si la queremos tomar en ese momento de la cámara
                var source = await Application.Current.MainPage.DisplayActionSheet(
                    Languages.SourceImageQuestion,
                    Languages.Cancel,
                    null,
                    Languages.FromGallery,
                    Languages.FromCamera);

                //Si la persona no tomó ninguna de las dos opciones y oprimió cancelar
                //entonces el archivo de la foto queda nulo
                if (source == Languages.Cancel)
                {
                    this.file = null;
                    return;
                }

                //Si oprmimos tomarnos una foto en ese momento desde nuestra cámara
                if (source == Languages.FromCamera)
                {
                    this.file = await CrossMedia.Current.TakePhotoAsync(//Tomamos la foto y la guardamos en el atributo file
                        new StoreCameraMediaOptions
                        {
                            Directory = "Sample",//directorio en donde se guarda la foto que nos tomamos
                            Name = "test.jpg",//el nombre con el que queda nuestra foto
                            PhotoSize = PhotoSize.Small,//el tamaño con el que va a quedar nuestra foto
                        }
                    );
                }
                else
                {
                    //Sino, agarramos la foto desde la galería de imágenes de nuestro celu
                    this.file = await CrossMedia.Current.PickPhotoAsync();
                }
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>//Leemos la imágen y la cargamos en la aplicación
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }
        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            //Validamos que haya ingresado nombres
            if (string.IsNullOrEmpty(this.User.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.FirstNameValidation,
                    Languages.Accept);
                return;
            }
            //validamos que haya ingresado apellidos
            if (string.IsNullOrEmpty(this.User.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.LastNameValidation,
                    Languages.Accept);
                return;
            }
            //validamos que haya ingresado un email
            if (string.IsNullOrEmpty(this.User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.Accept);
                return;
            }
            //validamos que el email sea válido
            if (!RegexUtilities.IsValidEmail(this.User.Email))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation2,
                    Languages.Accept);
                return;
            }
            //validamos que haya ingresado un numero de teléfono
            if (string.IsNullOrEmpty(this.User.Telephone))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.PhoneValidation,
                    Languages.Accept);
                return;
            }
           
     
            this.IsRunning = true;
            this.IsEnabled = false;

            var checkConnetion = await this.apiService.CheckConnection();
            if (!checkConnetion.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    checkConnetion.Message,
                    Languages.Accept);
                return;
            }

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var userDomain = Converter.ToUserDomain(this.User, imageArray);

            //Estamos actualizando la información de nuestro perfil desde la aplicación
            //Le enviamos el tipo de token y el token para mayor seguridad, es decir,
            //para que la persona tenga que estar logueada para poder editar la info
            var apiSecurity = Application.Current.Resources["APISecurity"].ToString();
            var response = await this.apiService.Put(
                apiSecurity,
                "/api",
                "/Users",
                MainViewModel.GetInstance().TokenType,
                MainViewModel.GetInstance().Token,
                userDomain);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.Accept);
                return;
            }
            var userApi = await this.apiService.GetUserByEmail(
               apiSecurity,
               "/api",
               "/Users/GetUserByEmail",
               MainViewModel.GetInstance().TokenType,
               MainViewModel.GetInstance().Token,
               this.User.Email);

            var userLocal = Converter.ToUserLocal(userApi);
            MainViewModel.GetInstance().User = userLocal;
            this.dataService.Update(userLocal);//Actualizando en base de datos

            this.IsRunning = false;
            this.IsEnabled = true;


     
            await App.Navigator.PopAsync();
        }
        #endregion

        #region Constructor
        public MyProfileViewModel()
        {
            //Los servicio siempre s einstancian en el constructor
            this.apiService = new ApiService();
            this.dataService = new DataService();

            this.User = MainViewModel.GetInstance().User;
            this.ImageSource = this.User.ImageFullPath;
            this.IsEnabled = true;
            

        }
        #endregion
    }
}
