using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchProfilePage : ContentPage
    {
        public SearchProfilePage()
        {
            InitializeComponent();

            SetColourScheme();

            btnSearch.Source = ImageSource.FromResource("MobileDevApp.Resources.search.png");
            btnScanQr.Source = ImageSource.FromResource("MobileDevApp.Resources.searchQR.png");
        }

        private void SetColourScheme()
        {
            //BackgroundColor = Color.FromHex(App.ColourScheme.PageColour);
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)Application.Current.Resources["headerColor"];
            ((NavigationPage)Application.Current.MainPage).BarTextColor = (Color)Application.Current.Resources["textColor"];
        }

        private void btnSearch_Tapped(object sender, EventArgs e)
        {

        }

        private async void btnScanQr_Tapped(object sender, EventArgs e)
        {
            ZXingScannerPage scannerPage = new ZXingScannerPage();
            NavigationPage.SetHasNavigationBar(scannerPage, false);

            await Navigation.PushAsync(scannerPage);

            scannerPage.OnScanResult += (result) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Navigation.PopAsync();

                    ProfilePage profilePage = new ProfilePage(false, result.Text);

                    await Navigation.PushAsync(profilePage);
                });
            };
        }
    }
}