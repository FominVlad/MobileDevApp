using MobileDevApp.RemoteProviders.Implementations;
using MobileDevApp.RemoteProviders.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DialogPage : ContentPage
    {
        private ChatShortInfo chatInfo { get; set; }
        private List<MessageShortInfo> oldMessages { get; set; }
        private ChatMessagingClient messagingClient { get; set; }

        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        public DialogPage(ChatShortInfo chatInfo)
        {
            this.chatInfo = chatInfo;

            this.messagingClient = new ChatMessagingClient(App.UserInfo.AccessToken);

            if(chatInfo.ChatID != null)
            {
                oldMessages = App.ChatService.GetAllChatMesages(App.UserInfo.AccessToken, (int)chatInfo.ChatID);

                oldMessages = oldMessages.AsEnumerable().OrderBy(o => o.ReceivedDate).ToList();

                AddMessagesToClient(oldMessages);
            }

            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;

            InitializeComponent();

            SetColourScheme();

            if (chatInfo.PartnerImage != null)
            {
                Stream stream = new MemoryStream(chatInfo.PartnerImage);

                imgPartnerImage.Source = ImageSource.FromStream(() => { return stream; });
            }

            lblPartnerName.Text = chatInfo.PartnerName;
            btnSendMessage.Source = ImageSource.FromResource("MobileDevApp.Resources.sendLetter.png");
            framePartnerIcon.HeightRequest = ScreenHeight / 55;
            framePartnerIcon.WidthRequest = ScreenHeight / 55;
            frameSendMessageIcon.HeightRequest = ScreenHeight / 55;
            frameSendMessageIcon.WidthRequest = ScreenWidth / 15;
            this.BindingContext = messagingClient;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            messagingClient.Connect();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            messagingClient.Disconnect();
        }

        private void AddMessagesToClient(List<MessageShortInfo> messages)
        {
            foreach (MessageShortInfo message in messages)
            {
                messagingClient.Messages.Add(message.ToMessageInfo());
            }
        }

        private void SetColourScheme()
        {
            ((NavigationPage)Application.Current.MainPage).BarBackgroundColor = (Color)Application.Current.Resources["headerColor"];
            ((NavigationPage)Application.Current.MainPage).BarTextColor = (Color)Application.Current.Resources["textColor"];
        }

        private void ButtonSendMessage_Clicked(object sender, EventArgs e)
        {
            if (messagingClient.IsConnected)
            {
                messagingClient.Message = new MessageInput()
                {
                    Text = entryTextMess.Text,
                    ReceiverID = chatInfo.PartnerID
                };

                messagingClient.SendMessage();

                entryTextMess.Text = "";
            }
        }
    }
}