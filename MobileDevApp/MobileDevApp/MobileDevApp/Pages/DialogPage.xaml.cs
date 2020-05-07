using MobileDevApp.RemoteProviders.Implementations;
using MobileDevApp.RemoteProviders.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

            //lblPartnerName.Text = chatInfo.PartnerName;

            InitializeComponent();
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

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (messagingClient.IsConnected)
            {
                messagingClient.Message = new MessageInput()
                {
                    Text = textMess.Text,
                    ReceiverID = chatInfo.PartnerID
                };

                messagingClient.SendMessage();
            }
        }
    }
}