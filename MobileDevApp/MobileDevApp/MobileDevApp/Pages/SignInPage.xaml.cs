using System;
using MobileDevApp.Helpers;
using MobileDevApp.RemoteProviders.Models;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SignInPage : ContentPage
    {
        public SignInPage()
        {
            InitializeComponent();
            SetComponentsProp();

            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;
            imgShowHidePass.Source = ImageSource.FromResource("MobileDevApp.Resources.show.png");
            frameShowHidePass.HeightRequest = 30;
            frameShowHidePass.WidthRequest = 30;
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

        private void SetComponentsProp()
        {
            btnSignUp.ImageSource = ImageSource.FromResource("MobileDevApp.Resources.signup.png");
            imgNoInternetConnection.Source = ImageSource.FromResource("MobileDevApp.Resources.noInternetConnection.png");
            imgAppLogo.Source = ImageSource.FromResource("MobileDevApp.Resources.chatLogo.png");

            CheckConnection();
        }

        public void IsLoading(bool isEnabled)
        {
            loaderIndicator.IsEnabled = isEnabled;
            loaderIndicator.IsRunning = isEnabled;
            loaderIndicator.IsVisible = isEnabled;
        }

        private async void btnSignIn_Clicked(object sender, EventArgs e)
        {
            try
            {
                Validator validator = new Validator();
                string exception = "";

                if (validator.ValidateString(entryLogin.Text, out exception) && validator.ValidateString(entryPassword.Text, out exception))
                {
                    IsLoading(true);

                    UserLogin userLogin = GetUserFromEntry();
                    UserInfo user = App.UserService.Login(userLogin);

                    if (user != null)
                    {
                        App.Database.AddUserIfNotExist(user);
                        DependencyService.Get<INotification>().CreateNotification("ZakritiyPredmetChat", 
                            $"User {user.Name} logined successfully!");

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

        private UserLogin GetUserFromEntry()
        {
            HashHelper hashHelper = new HashHelper();
            Validator validator = new Validator();
            string pwdHash = hashHelper.GenerateHash(entryPassword.Text);

            return new UserLogin()
            {
                Login = entryLogin.Text,
                PasswordHash = pwdHash,
                LoginType = validator.GetLoginType(entryLogin.Text)
            };
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            entryPassword.IsPassword = !entryPassword.IsPassword;

            imgShowHidePass.Source = entryPassword.IsPassword == true ?
                ImageSource.FromResource("MobileDevApp.Resources.show.png") :
                ImageSource.FromResource("MobileDevApp.Resources.hide.png");
        }
    }
}