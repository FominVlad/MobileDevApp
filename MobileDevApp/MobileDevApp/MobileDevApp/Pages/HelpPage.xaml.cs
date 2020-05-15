using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HelpPage : ContentPage
    {
        public HelpPage()
        {
            InitializeComponent();

            SetColourScheme();

            lblInfo.Text = "Here you can ask any question, comment or suggestion to " +
                    "improve the quality of our application and get a response from technical support.";
        }

        private void SetColourScheme()
        {
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)Application.Current.Resources["headerColor"];
            ((NavigationPage)Application.Current.MainPage).BarTextColor = (Color)Application.Current.Resources["textColor"];
        }

        private void btnSendMessage_Clicked(object sender, EventArgs e)
        {
            DisplayAlert("Notification", "Thank you for your appeal! It will be processed shortly.", "OK");
            ClosePage();
        }

        private async void ClosePage()
        {
            await Navigation.PopAsync();
        }
    }
}