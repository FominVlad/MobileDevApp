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
            NavigationPage.SetHasNavigationBar(this, false);
        }
    }
}
