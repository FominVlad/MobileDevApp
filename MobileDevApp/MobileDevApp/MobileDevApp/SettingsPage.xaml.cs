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
            BackgroundColor = Color.FromHex(App.ColourScheme.PageColour);
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
            (Application.Current).MainPage = new NavigationPage(new MainPage());
        }

        private void btnTest_Clicked(object sender, EventArgs e)
        {
            DependencyService.Get<INotification>().CreateNotification("SPTutorials", "TEST TEXT");
        }
    }
}