using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Profile : ContentPage
    {
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        public Profile()
        {
            InitializeComponent();

            SetComponentsProp();
        }

        private void SetComponentsProp()
        {
            imgProfileIcon.Source = ImageSource.FromResource("MobileDevApp.Resources.personIcon.png");
            btnRedactProfile.Source = ImageSource.FromResource("MobileDevApp.Resources.pencil.png");
            btnSaveProfile.Source = ImageSource.FromResource("MobileDevApp.Resources.ready.png");
            btnHelp.ImageSource = ImageSource.FromResource("MobileDevApp.Resources.help.png");

            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;

            imgProfileIcon.WidthRequest = ScreenWidth / 8;
            imgProfileIcon.HeightRequest = ScreenWidth / 8;

            btnRedactProfile.WidthRequest = ScreenWidth / 25;
            btnRedactProfile.HeightRequest = ScreenWidth / 25;

            btnSaveProfile.WidthRequest = ScreenWidth / 25;
            btnSaveProfile.HeightRequest = ScreenWidth / 25;

            entryUserName.Text = "Test User";
            entryUserId.Text = "@TestUserId";
            entryUserPhoneNumber.Text = "+380123456789";
            editorUserDescription.Text = "Its test text about me. Its test text about me. " +
                "Its test text about me.";
        }

        private void btnRedactProfile_Clicked(object sender, System.EventArgs e)
        {
            entryUserName.IsEnabled = true;
            entryUserId.IsEnabled = true;
            entryUserPhoneNumber.IsEnabled = true;
            editorUserDescription.IsEnabled = true;

            btnRedactProfile.IsVisible = false;
            btnSaveProfile.IsVisible = true;
        }

        private void btnSaveProfile_Clicked(object sender, System.EventArgs e)
        {
            entryUserName.IsEnabled = false;
            entryUserId.IsEnabled = false;
            entryUserPhoneNumber.IsEnabled = false;
            editorUserDescription.IsEnabled = false;

            btnRedactProfile.IsVisible = true;
            btnSaveProfile.IsVisible = false;
        }

        private void btnHelp_Clicked(object sender, System.EventArgs e)
        {
            OpenHelpPage();
        }

        private async void OpenHelpPage()
        {
            HelpPage helpPage = new HelpPage();
            await Navigation.PushAsync(helpPage);
        }
    }
}