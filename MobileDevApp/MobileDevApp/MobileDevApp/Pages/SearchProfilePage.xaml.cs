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
    public class Test
    {
        public string name { get; set; }
        public ImageSource img { get; set; }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchProfilePage : ContentPage
    {
        public SearchProfilePage()
        {
            InitializeComponent();
            SetColourScheme();
            SetComponentsProp();
        }

        private void SetColourScheme()
        {
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)Application.Current.Resources["headerColor"];
            ((NavigationPage)Application.Current.MainPage).BarTextColor = (Color)Application.Current.Resources["textColor"];
        }

        private void SetComponentsProp()
        {
            btnSearch.Source = ImageSource.FromResource("MobileDevApp.Resources.search.png");
            btnScanQr.Source = ImageSource.FromResource("MobileDevApp.Resources.searchQR.png");
        }

        private async void btnSearch_Tapped(object sender, EventArgs e)
        {
            try
            {
                lwSearchREsults.ItemsSource = new List<Test>()
                {
                    new Test() { img = ImageSource.FromResource("MobileDevApp.Resources.search.png"), name = "Name testsss" }
                };
                //App.UserService.Info();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Unknown error...", "OK");
            }
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