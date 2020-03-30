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

        private async void btnSignUp_Clicked(object sender, System.EventArgs e)
        {

        }
    }
}