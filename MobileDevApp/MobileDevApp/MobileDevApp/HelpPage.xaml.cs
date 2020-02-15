using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            lblInfo.Text = "Here you can ask any question, comment or suggestion to " +
                "improve the quality of our application and get a response from technical support.";
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