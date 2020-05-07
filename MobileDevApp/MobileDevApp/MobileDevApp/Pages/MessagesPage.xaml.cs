using MobileDevApp.RemoteProviders.Models;
using System.Collections.Generic;
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

        public List<ChatShortInfo> chatInfoList { get; set; }

        public Messages()
        {
            InitializeComponent();

            chatInfoList = App.ChatService.GetAllUserChats(App.UserInfo.AccessToken);


            this.BindingContext = this;

            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;

            btnAddMessage.Source = ImageSource.FromResource("MobileDevApp.Resources.add.png");
            btnAddMessage.WidthRequest = ScreenWidth / 15;
            btnAddMessage.HeightRequest = ScreenWidth / 15;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.MessagingClient.Connect();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            App.MessagingClient.Disconnect();
        }

        private async void btnAddMessage_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SearchProfilePage());
            //await Navigation.PushAsync(new DialogPage());
        }

        private async void listMessages_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ChatShortInfo selectedChat = e.SelectedItem as ChatShortInfo;
            listMessages.SelectedItem = null;

            if (selectedChat != null)
            {
                await Navigation.PushAsync(new DialogPage(selectedChat));
            }
        }
    }
}