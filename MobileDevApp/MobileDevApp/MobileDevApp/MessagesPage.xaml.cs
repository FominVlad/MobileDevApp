using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Messages : ContentPage
    {
        public Messages()
        {
            InitializeComponent();
            SetColourScheme();
        }

        private void SetColourScheme()
        {
            BackgroundColor = Color.FromHex(App.ColourScheme.PageColour);
            lblMessageList.TextColor = Color.FromHex(App.ColourScheme.TextColour);
        }
    }
}