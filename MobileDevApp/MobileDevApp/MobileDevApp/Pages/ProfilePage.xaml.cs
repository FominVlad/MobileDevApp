using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Essentials;
using System;
using System.Linq;
using MobileDevApp.Models;
using Plugin.Media;
using Plugin.Media.Abstractions;
using MobileDevApp.RemoteProviders.Models;
using MobileDevApp.Helpers;
using System.IO;
using System.Text.RegularExpressions;

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

            SetComponentsProp();

            SetUserInfo();
        }

        public ProfilePage(bool IsOwner, string id = "@TestUserId")
        {
            this.IsOwner = IsOwner;

            this.Id = id;

            InitializeComponent();

            SetComponentsProp();
        }

        private void SetComponentsProp()
        {
            if(App.UserInfo.Image == null)
            {
                imgProfileIcon.Source = ImageSource.FromResource("MobileDevApp.Resources.personIcon.png");
            }

            btnRedactProfile.Source = ImageSource.FromResource("MobileDevApp.Resources.pencil.png");
            btnSaveProfile.Source = ImageSource.FromResource("MobileDevApp.Resources.ready.png");
            btnHelp.ImageSource = ImageSource.FromResource("MobileDevApp.Resources.help.png");
            btnMyQr.ImageSource = ImageSource.FromResource("MobileDevApp.Resources.scanQR.png");
            //btnRedactImage.Source = ImageSource.FromResource("MobileDevApp.Resources.personChangeIcon.png");

            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;

            btnRedactProfile.WidthRequest = ScreenWidth / 25;
            btnRedactProfile.HeightRequest = ScreenWidth / 25;

            btnSaveProfile.WidthRequest = ScreenWidth / 25;
            btnSaveProfile.HeightRequest = ScreenWidth / 25;

            frameProfileIcon.WidthRequest = ScreenWidth / 5;
            frameProfileIcon.HeightRequest = ScreenWidth / 5;

            //frameRedactImage.WidthRequest = ScreenWidth / 5;
            //frameRedactImage.HeightRequest = ScreenWidth / 5;

            if (!IsOwner)
            {
                btnRedactProfile.IsVisible = false;
                btnSaveProfile.IsVisible = false;
                btnHelp.IsVisible = false;
                btnMyQr.IsVisible = false;
                btnWriteMessage.IsVisible = true;
            }
        }

        private void SetUserInfo()
        {
            UserInfo userInfo = App.Database.userInfo.FirstOrDefault();

            if (userInfo != null)
            {
                entryUserName.Text = userInfo.Name;
                entryUserId.Text = userInfo.Email;
                entryUserPhoneNumber.Text = userInfo.PhoneNumber;
                editorUserDescription.Text = userInfo.Bio;

                if (userInfo.Image != null)
                {
                    Stream stream = new MemoryStream(userInfo.Image);

                    imgProfileIcon.Source = ImageSource.FromStream(() => { return stream; });
                }
            }
        }

        private void btnRedactProfile_Clicked(object sender, System.EventArgs e)
        {
            iconByteStr = null;
            entryUserName.IsEnabled = true;
            entryUserId.IsEnabled = true;
            entryUserPhoneNumber.IsEnabled = true;
            editorUserDescription.IsEnabled = true;

            btnRedactProfile.IsVisible = false;
            btnSaveProfile.IsVisible = true;

            imgProfileIcon.Source = ImageSource.FromResource("MobileDevApp.Resources.personChangeIcon.png");

            //frameProfileIcon.IsVisible = false;
            //frameRedactImage.IsVisible = true;
        }

        private async void btnSaveProfile_Clicked(object sender, System.EventArgs e)
        {
            try
            {
                EditUserInfo();

                entryUserName.IsEnabled = false;
                entryUserId.IsEnabled = false;
                entryUserPhoneNumber.IsEnabled = false;
                editorUserDescription.IsEnabled = false;

                btnRedactProfile.IsVisible = true;
                btnSaveProfile.IsVisible = false;

                if(iconByteStr == null)
                {
                    imgProfileIcon.Source = ImageSource.FromResource("MobileDevApp.Resources.personIcon.png");
                }

                //imgProfileIcon.Source = ImageSource.FromResource("MobileDevApp.Resources.personIcon.png");

                //frameProfileIcon.IsVisible = true;
                //frameRedactImage.IsVisible = false;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Unknown error...", "OK");
            }
            IsLoading(false);
        }

        private async void EditUserInfo()
        {
            try
            {
                Validator validator = new Validator();
                string exception = "";

                if (validator.ValidateEmail(entryUserId.Text, out exception) && /*ValidatePhoneNumber(entryUserPhoneNumber.Text) &&*/ validator.ValidateName(entryUserName.Text, out exception))
                {
                    IsLoading(true);

                    UserEdit userEdit = GetUserInfo();
                    UserInfo editedUser = App.UserService.Edit(userEdit, App.UserInfo.AccessToken);

                    if (editedUser != null)
                    {
                        AddUserToDb(editedUser);
                        DependencyService.Get<INotification>().CreateNotification("ZakritiyPredmetChat", $"User {editedUser.Name} edited successfully!");
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    await DisplayAlert("Error!", exception, "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error!", "Unknown error...", "OK");
                //throw new Exception();
            }
        }

        private void AddUserToDb(UserInfo user)
        {
            if (App.Database.userInfo.Any())
            {
                App.Database.userInfo.Add(user);
                App.Database.SaveChanges();
            }
            else
            {
                throw new Exception("User is not logged in!");
            }
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

        public void IsLoading(bool isEnabled)
        {
            loaderIndicator.IsEnabled = isEnabled;
            loaderIndicator.IsRunning = isEnabled;
            loaderIndicator.IsVisible = isEnabled;
        }
    }
}