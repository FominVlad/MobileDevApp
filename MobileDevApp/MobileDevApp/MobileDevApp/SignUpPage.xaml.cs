using MobileDevApp.Helpers;
using MobileDevApp.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public SignUpPage()
        {
            InitializeComponent();

            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged;

            SetComponentsProp();
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
            /*
            StringEncoder stringEncoder = new StringEncoder();

            byte[] imgBytes = stringEncoder.EncodeToBytes(googleUser.PhotoBytes);

            Stream stream = new MemoryStream(imgBytes);
            
            imgPhoto.Source = ImageSource.FromStream(() => { return stream; });
            imgPhoto.HeightRequest = 200;
            imgPhoto.WidthRequest = 200;
            */
        }

        private void btnSignUp_Clicked(object sender, System.EventArgs e)
        {
            if (ValidateName(entryName.Text) && ValidateLogin(entryLogin.Text) &&
                ValidatePassword(entryPassword.Text, entryConfirmPassword.Text))
            {
                DisplayCustomAlert("OKAY!", "All is OK!");
            }
        }

        private bool ValidateLogin(string login)
        {
            if (string.IsNullOrEmpty(login))
            {
                DisplayCustomAlert("Error!", "Login cannot be empty.");
                return false;
            }

            if (login.Length < 8)
            {
                DisplayCustomAlert("Error!", "Login must be longer than 8 characters.");
                return false;
            }

            return true;
        }

        private bool ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                DisplayCustomAlert("Error!", "Name cannot be empty.");
                return false;
            }

            if (name.Length == 0)
            {
                DisplayCustomAlert("Error!", "Name must be at least 1 character");
                return false;
            }

            return true;
        }

        private bool ValidatePassword(string password, string confirmPassword)
        {
            if (string.IsNullOrEmpty(password))
            {
                DisplayCustomAlert("Error!", "Password cannot be empty.");
                return false;
            }

            if (string.IsNullOrEmpty(confirmPassword))
            {
                DisplayCustomAlert("Error!", "Password confirm cannot be empty.");
                return false;
            }

            if (!password.Equals(confirmPassword))
            {
                DisplayCustomAlert("Error!", "Password must be the same as password confirmation.");
                return false;
            }

            if (password.Length < 8)
            {
                DisplayCustomAlert("Error!", "Password must be longer than 8 characters.");
                return false;
            }

            Regex hasNumber = new Regex(@"[0-9]+");
            Regex hasUpperChar = new Regex(@"[A-Z]+");
            Regex hasMiniMaxChars = new Regex(@".{8,15}");
            Regex hasLowerChar = new Regex(@"[a-z]+");
            Regex hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (!hasLowerChar.IsMatch(password))
            {
                DisplayCustomAlert("Error!", "Password should contain at least one lower case letter.");
                return false;
            }
            else if (!hasUpperChar.IsMatch(password))
            {
                DisplayCustomAlert("Error!", "Password should contain at least one upper case letter.");
                return false;
            }
            else if (!hasMiniMaxChars.IsMatch(password))
            {
                DisplayCustomAlert("Error!", "Password should not be lesser than 8 or greater than 15 characters.");
                return false;
            }
            else if (!hasNumber.IsMatch(password))
            {
                DisplayCustomAlert("Error!", "Password should contain at least one numeric value.");
                return false;
            }

            else if (!hasSymbols.IsMatch(password))
            {
                DisplayCustomAlert("Error!", "Password should contain at least one special case character.");
                return false;
            }

            return true;
        }

        private async void DisplayCustomAlert(string topic, string alertText)
        {
            await DisplayAlert(topic, alertText, "OK");
        }
    }
}