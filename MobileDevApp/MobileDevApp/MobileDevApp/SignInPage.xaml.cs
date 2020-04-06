using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using MobileDevApp.Helpers;
using MobileDevApp.Models;
using MobileDevApp.Services;
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
                if (ValidateString(entryLogin.Text) && ValidateString(entryPassword.Text))
                {
                    IsLoading(true);
                    (Application.Current).MainPage = new NavigationPage(new MainPage());

                    UserService userService = new UserService();

                    UserLogin userLogin = GetUserFromEntry();

                    UserInfo user = await userService.LoginUser(userLogin);

                    if (user != null)
                    {
                        AddUserToDb(user);
                        DependencyService.Get<INotification>().CreateNotification("ZakritiyPredmetChat", $"User {user.Name} logined successfully!");

                        (Application.Current).MainPage = new NavigationPage(new MainPage());
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                DisplayCustomAlert("Error!", "Unknown error...");
                IsLoading(false);
            }
        }

        private void AddUserToDb(UserInfo user)
        {
            if (!App.Database.userInfo.Any())
            {
                App.Database.userInfo.Add(user);
                App.Database.SaveChanges();
            }
            else
            {
                throw new Exception("User is already logged in!");
            }
        }

        private UserLogin GetUserFromEntry()
        {
            HashHelper hashHelper = new HashHelper();
            string pwdHash = hashHelper.GenerateHash(entryPassword.Text);

            return new UserLogin()
            {
                Login = entryLogin.Text,
                PasswordHash = pwdHash,
                LoginType = GetLoginType(entryLogin.Text)
            };
        }

        private LoginType GetLoginType(string login)
        {
            Regex emailRegex = new Regex(@"^\w.+@[a-zA-Z_]+?\.[a-zA-Z]{2,3}$");

            if (emailRegex.IsMatch(login))
            {
                return LoginType.Email;
            }
            else
            {
                return LoginType.PhoneNumber;
            }
        }

        private bool ValidateString(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                DisplayCustomAlert("Error!", "Login cannot be empty.");
                return false;
            }

            return true;
        }

        private async void DisplayCustomAlert(string topic, string alertText)
        {
            await DisplayAlert(topic, alertText, "OK");
        }

        private async void btnSignUp_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SignUpPage());
        }
    }
}