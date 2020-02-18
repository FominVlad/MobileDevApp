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
            SetColourScheme();
        }

        private void SetColourScheme()
        {
            BackgroundColor = Color.FromHex(App.ColourScheme.PageColour);
            lblText.TextColor = Color.FromHex(App.ColourScheme.TextColour);
        }

        private void btnUpdate_Clicked(object sender, System.EventArgs e)
        {
            var a = App.Database.Settings.FirstOrDefault();
            a.ColourScheme = App.Database.ColourSchemes.Where(obj => obj.SchemeType == "Dark").FirstOrDefault();
            App.Database.Settings.Update(a);
            App.Database.SaveChanges();
            (Application.Current).MainPage = new NavigationPage(new MainPage());
        }
    }
}