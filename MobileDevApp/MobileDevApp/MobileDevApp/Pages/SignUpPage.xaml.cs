using MobileDevApp.Helpers;
using MobileDevApp.Models;
using MobileDevApp.RemoteProviders.Implementations;
using MobileDevApp.RemoteProviders.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileDevApp
{
    public delegate void SetGoogleUserInfo(GoogleUser googleUser);

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignUpPage : ContentPage
    {
        private byte[] profileImage { get; set; }

        public SignUpPage()
        {
            InitializeComponent();
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            SetColourScheme();
            SetComponentsProp();
        }

        private void SetColourScheme()
        {
            //((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)Application.Current.Resources["headerColor"];
            //((NavigationPage)Application.Current.MainPage).BarTextColor = (Color)Application.Current.Resources["textColor"];
        }

        private void SetComponentsProp()
        {
            imgNoInternetConnection.Source = ImageSource.FromResource("MobileDevApp.Resources.noInternetConnection.png");
            btnGoogleSignUp.ImageSource = ImageSource.FromResource("MobileDevApp.Resources.googleicon.png");
            imgAppLogo.Source = ImageSource.FromResource("MobileDevApp.Resources.chatLogo.png");

            CheckConnection();
        }

        private void Current_ConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            CheckConnection();
        }

        private void CheckConnection()
        {
            grdInternetConn.IsVisible = !CrossConnectivity.Current.IsConnected;
            grdContent.IsVisible = CrossConnectivity.Current.IsConnected;
        }

        private void btnGoogleSignUp_Clicked(object sender, EventArgs e)
        {
            IsLoading(true);
            IGoogleAuth tst = DependencyService.Get<IGoogleAuth>();
            tst.GetGoogleUser(SetInfoFromGoogle);
        }

        public void IsLoading(bool isEnabled)
        {
            loaderIndicator.IsEnabled = isEnabled;
            loaderIndicator.IsRunning = isEnabled;
            loaderIndicator.IsVisible = isEnabled;
        }

        public void SetInfoFromGoogle(GoogleUser googleUser)
        {
            IsLoading(false);
            this.entryLogin.Text = googleUser.Login;
            this.entryName.Text = googleUser.UserName;

            StringEncoder stringEncoder = new StringEncoder();

            profileImage = stringEncoder.EncodeToBytes(googleUser.PhotoBytes);
        }

        private async void btnSignUp_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                Validator validator = new Validator();
                string exception = "";

                if (validator.ValidateName(entryName.Text, out exception) && validator.ValidateLogin(entryLogin.Text, out exception) &&
                    validator.ValidatePasswordsEquals(entryPassword.Text, entryConfirmPassword.Text, out exception) 
                    && validator.ValidatePassword(entryPassword.Text, out exception))
                {
                    IsLoading(true);

                    UserRegister user = GetUserFromEntry();
                    UserInfo createdUser = App.UserService.Register(user);

                    if (createdUser != null)
                    {
                        App.Database.AddUserIfNotExist(createdUser);
                        DependencyService.Get<INotification>().CreateNotification("ZakritiyPredmetChat", $"User {createdUser.Name} created successfully!");
                        
                        (Application.Current).MainPage = new NavigationPage(new MainPage());
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    await DisplayAlert("Error!", exception, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Unknown error...", "OK");
                IsLoading(false);
            }
        }

        private byte[] GetDefaultPhotoBytes()
        {
            var assembly = this.GetType().GetTypeInfo().Assembly;
            byte[] bytes = null;

            using (System.IO.Stream s = assembly.GetManifestResourceStream("MobileDevApp.Resources.personIcon.png"))
            {
                if (s != null)
                {
                    long length = s.Length;
                    bytes = new byte[length];
                    s.Read(bytes, 0, (int)length);
                }
            }

            return bytes;
        }

        private UserRegister GetUserFromEntry()
        {
            HashHelper hashHelper = new HashHelper();
            Validator validator = new Validator();
            string pwdHash = hashHelper.GenerateHash(entryPassword.Text);

            return new UserRegister()
            {
                Name = entryName.Text,
                Login = entryLogin.Text,
                PasswordHash = pwdHash,
                LoginType = validator.GetLoginType(entryLogin.Text),
                Image = profileImage == null ? GetDefaultPhotoBytes() : profileImage
            };
        }
    }
}