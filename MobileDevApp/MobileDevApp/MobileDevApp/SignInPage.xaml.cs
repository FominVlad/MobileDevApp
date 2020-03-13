using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            btnGoogleSignIn.ImageSource = ImageSource.FromResource("MobileDevApp.Resources.googleicon.png");
            imgNoInternetConnection.Source = ImageSource.FromResource("MobileDevApp.Resources.noInternetConnection.png");

            CheckConnection();
        }

        public void IsLoading(bool isEnabled)
        {
            loaderIndicator.IsEnabled = isEnabled;
            loaderIndicator.IsRunning = isEnabled;
            loaderIndicator.IsVisible = isEnabled;
        }

        private void btnSignIn_Clicked(object sender, EventArgs e)
        {
            IsLoading(true);
            (Application.Current).MainPage = new NavigationPage(new MainPage());
        }

        private void btnSignUp_Clicked(object sender, EventArgs e)
        {

        }

        private void btnGoogleSignIn_Clicked(object sender, EventArgs e)
        {
            IGoogleAuth tst = DependencyService.Get<IGoogleAuth>();
            tst.GetGoogle();
        }

        private void btnReload_Clicked(object sender, EventArgs e)
        {

        }
    }
}