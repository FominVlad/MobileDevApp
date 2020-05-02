using MobileDevApp.RemoteProviders.Models;
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
                UserInfo userInfo = App.UserService.Info(entrySearchString.Text, App.UserInfo.AccessToken);

                if (userInfo != null)
                {
                    ProfilePage profilePage = new ProfilePage(userInfo);

                    await Navigation.PushAsync(profilePage);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Unknown error...", "OK");
            }
        }

        private async void btnScanQr_Tapped(object sender, EventArgs e)
        {
            try
            {
                ZXingScannerPage scannerPage = new ZXingScannerPage();
                NavigationPage.SetHasNavigationBar(scannerPage, false);

                await Navigation.PushAsync(scannerPage);

                scannerPage.OnScanResult += (result) =>
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Navigation.PopAsync();

                        if(!Int32.TryParse(result.Text, out int userId))
                        {
                            throw new Exception("Qr code is not user id!");
                        }

                        UserInfo userInfo = App.UserService.Info(userId, App.UserInfo.AccessToken);

                        if (userInfo != null)
                        {
                            ProfilePage profilePage = new ProfilePage(userInfo);

                            await Navigation.PushAsync(profilePage);
                        }
                        else
                        {
                            throw new Exception("userInfo is null!");
                        }
                    });
                };
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Unexpected error.", "OK");
            }
        }
    }
}