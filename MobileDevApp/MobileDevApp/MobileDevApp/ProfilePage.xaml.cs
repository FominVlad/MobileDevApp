using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System;
using System.Linq;
using MobileDevApp.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using MobileDevApp.Services;
using MobileDevApp.RemoteProviders.Models;
using MobileDevApp.Helpers;
using System.IO;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfilePage : ContentPage
    {
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        private bool IsOwner { get; set; } = true;

        public string Id { get; set; }

        private byte[] iconByteStr { get; set; }

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
            btnRedactImage.Source = ImageSource.FromResource("MobileDevApp.Resources.personChangeIcon.png");

            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;

            btnRedactProfile.WidthRequest = ScreenWidth / 25;
            btnRedactProfile.HeightRequest = ScreenWidth / 25;

            btnSaveProfile.WidthRequest = ScreenWidth / 25;
            btnSaveProfile.HeightRequest = ScreenWidth / 25;

            frameProfileIcon.WidthRequest = ScreenWidth / 5;
            frameProfileIcon.HeightRequest = ScreenWidth / 5;

            frameRedactImage.WidthRequest = ScreenWidth / 5;
            frameRedactImage.HeightRequest = ScreenWidth / 5;

            if (!IsOwner)
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
            Models.UserInfo userInfo = App.Database.userInfo.FirstOrDefault();

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

            frameProfileIcon.IsVisible = false;
            frameRedactImage.IsVisible = true;
        }

        private void btnSaveProfile_Clicked(object sender, System.EventArgs e)
        {
            entryUserName.IsEnabled = false;
            entryUserId.IsEnabled = false;
            entryUserPhoneNumber.IsEnabled = false;
            editorUserDescription.IsEnabled = false;

            btnRedactProfile.IsVisible = true;
            btnSaveProfile.IsVisible = false;

            frameProfileIcon.IsVisible = true;
            frameRedactImage.IsVisible = false;

            EditUserInfo();
        }

        private void EditUserInfo()
        {
            UserService userService = new UserService();

            UserEdit userEdit = GetUserInfo();

            userService.EditUser(userEdit);
        }

        private UserEdit GetUserInfo()
        {
            return new UserEdit()
            {
                Name = entryUserName.Text,
                PhoneNumber = entryUserPhoneNumber.Text,
                Email = entryUserId.Text,
                Bio = editorUserDescription.Text,
                Image = iconByteStr
            };
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

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    StringEncoder stringEncoder = new StringEncoder();
                    MediaFile photo = await CrossMedia.Current.PickPhotoAsync();
                    imgProfileIcon.Source = ImageSource.FromFile(photo.Path);
                    iconByteStr = File.ReadAllBytes(photo.Path);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}