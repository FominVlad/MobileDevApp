using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZXing.Net.Mobile.Forms;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Messages : ContentPage
    {
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        public Messages()
        {
            InitializeComponent();
            SetColourScheme();

            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;

            btnAddMessage.Source = ImageSource.FromResource("MobileDevApp.Resources.add.png");
            btnAddMessage.WidthRequest = ScreenWidth / 15;
            btnAddMessage.HeightRequest = ScreenWidth / 15;

            //string[] tmp = new string[50];

            //for(int i = 0; i < 50; i++)
            //{
            //    tmp[i] = i.ToString();
            //}

            //phonesList.ItemsSource = tmp;
        }

        private void SetColourScheme()
        {
            //BackgroundColor = Color.FromHex(App.ColourScheme.PageColour);
        }

        private async void btnAddMessage_Clicked(object sender, System.EventArgs e)
        {
            //await Navigation.PushAsync(new SearchProfilePage());
            await Navigation.PushAsync(new DialogPage());
        }
    }
}