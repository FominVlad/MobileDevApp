using MobileDevApp.Helpers;
using MobileDevApp.Models;
using System;
using System.Json;
using System.Linq;
using Xamarin.Auth;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Settings : ContentPage
    {
        public Settings()
        {
            InitializeComponent();
            SetColourScheme();
            SetSwitches();
        }

        private void SetColourScheme()
        {
            //BackgroundColor = Color.FromHex(App.ColourScheme.PageColour);
        }

        private void SetSwitches()
        {
            if (App.ColourScheme.SchemeType == "Light")
            {
                switchColourScheme.IsToggled = false;
            }
            else
            {
                switchColourScheme.IsToggled = true;
            }

            switchColourScheme.Toggled += switchColourScheme_Toggled;
        }

        private void switchColourScheme_Toggled(object sender, ToggledEventArgs e)
        {
            Models.Settings settings = App.Database.Settings.FirstOrDefault();

            if (switchColourScheme.IsToggled)
            {
                settings.ColourScheme = App.Database.ColourSchemes
                    .Where(obj => obj.SchemeType == "Dark").FirstOrDefault();
            }
            else
            {
                settings.ColourScheme = App.Database.ColourSchemes
                    .Where(obj => obj.SchemeType == "Light").FirstOrDefault();
            }

            App.Database.Settings.Update(settings);
            App.Database.SaveChanges();

            if(!UpdateResources(settings))
            {
                // Write exception!
            }
        }

        private bool UpdateResources(Models.Settings settings)
        {
            try
            {
                Application.Current.Resources["textColor"] = Color.FromHex(settings.ColourScheme.TextColour);
                Application.Current.Resources["headerColor"] = Color.FromHex(settings.ColourScheme.HeaderColour);
                Application.Current.Resources["buttonColor"] = Color.FromHex(settings.ColourScheme.ButtonColour);
                Application.Current.Resources["pageColor"] = Color.FromHex(settings.ColourScheme.PageColour);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void btnSignOut_Clicked(object sender, EventArgs e)
        {
            if (App.Database.userInfo.Any())
            {
                App.Database.userInfo.Remove(App.Database.userInfo.FirstOrDefault());
                App.Database.SaveChanges();

                (Application.Current).MainPage = new NavigationPage(new SignInPage());
            }
            else
            {
                throw new Exception("User not authorized!");
            }
        }
    }
}