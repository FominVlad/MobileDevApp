using System;
using System.Linq;
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
            SetSwitches();
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
            App.Database.DeleteUserIfExist();

            (Application.Current).MainPage = new NavigationPage(new SignInPage());
        }
    }
}