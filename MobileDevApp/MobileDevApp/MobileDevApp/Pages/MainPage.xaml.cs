using System;
using System.ComponentModel;
using System.Linq;
using Xamarin.Forms;

namespace MobileDevApp
{
    [DesignTimeVisible(false)]
    public partial class MainPage : TabbedPage
    {
        public MainPage()
        {

            InitializeComponent();
            SetColourScheme();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void SetColourScheme()
        {
            //BackgroundColor = Color.FromHex(App.ColourScheme.PageColour);
            //BarBackgroundColor = Color.FromHex(App.ColourScheme.HeaderColour);
            //BarTextColor = Color.FromHex(App.ColourScheme.TextColour);
        }
    }
}
