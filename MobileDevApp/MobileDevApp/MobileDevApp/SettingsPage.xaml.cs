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
    }
}