using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System;
using System.Linq;
using MobileDevApp.Models;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        private bool IsOwner { get; set; } = true;

        public string Id { get; set; }

        public ProfilePage()
        {
            InitializeComponent();

            SetColourScheme();

            SetUserInfo();

            SetComponentsProp();
        }

        public ProfilePage(bool IsOwner, string id = "@TestUserId")
        {
            this.IsOwner = IsOwner;

            this.Id = id;

            InitializeComponent();

            SetColourScheme();

            SetComponentsProp();
        }

        private void SetComponentsProp()
        {
            imgProfileIcon.Source = ImageSource.FromResource("MobileDevApp.Resources.personIcon.png");
            btnRedactProfile.Source = ImageSource.FromResource("MobileDevApp.Resources.pencil.png");
            btnSaveProfile.Source = ImageSource.FromResource("MobileDevApp.Resources.ready.png");
            btnHelp.ImageSource = ImageSource.FromResource("MobileDevApp.Resources.help.png");
            btnMyQr.ImageSource = ImageSource.FromResource("MobileDevApp.Resources.scanQR.png");

            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;

            imgProfileIcon.WidthRequest = ScreenWidth / 8;
            imgProfileIcon.HeightRequest = ScreenWidth / 8;

            btnRedactProfile.WidthRequest = ScreenWidth / 25;
            btnRedactProfile.HeightRequest = ScreenWidth / 25;

            btnSaveProfile.WidthRequest = ScreenWidth / 25;
            btnSaveProfile.HeightRequest = ScreenWidth / 25;

           

            if(!IsOwner)
            {
                btnRedactProfile.IsVisible = false;
                btnSaveProfile.IsVisible = false;
                btnHelp.IsVisible = false;
                btnMyQr.IsVisible = false;
                btnWriteMessage.IsVisible = true;
            }
        }

        private void SetColourScheme()
        {
            BackgroundColor = Color.FromHex(App.ColourScheme.PageColour);
            lblID.TextColor = Color.FromHex(App.ColourScheme.TextColour);
            entryUserName.TextColor = Color.FromHex(App.ColourScheme.TextColour);
            entryUserId.TextColor = Color.FromHex(App.ColourScheme.TextColour);
            lblPhoneNum.TextColor = Color.FromHex(App.ColourScheme.TextColour);
            entryUserPhoneNumber.TextColor = Color.FromHex(App.ColourScheme.TextColour);
            lblDescription.TextColor = Color.FromHex(App.ColourScheme.TextColour);
            editorUserDescription.TextColor = Color.FromHex(App.ColourScheme.TextColour);
            btnHelp.TextColor = Color.FromHex(App.ColourScheme.TextColour);
        }

        private void SetUserInfo()
        {
            UserInfo userInfo = App.Database.userInfo.FirstOrDefault();

            if(userInfo != null)
            {
                entryUserName.Text = userInfo.Name;
                entryUserId.Text = userInfo.Email;
                entryUserPhoneNumber.Text = userInfo.PhoneNumber;
                editorUserDescription.Text = userInfo.Bio;
            }
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
            await Navigation.PushAsync(new HelpPage());
        }

        private void btnWriteMessage_Clicked(object sender, EventArgs e)
        {

        }

        private async void btnMyQr_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new QrCodePage("Test QR Code User ID"));
        }
    }
}