using MobileDevApp.RemoteProviders.Implementations;
using MobileDevApp.RemoteProviders.Models;
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
    public partial class DialogPage : ContentPage
    {
        private ChatShortInfo chatInfo { get; set; }
        private List<MessageShortInfo> oldMessages { get; set; }
        private ChatMessagingClient messagingClient {get; set;}

        public DialogPage(ChatShortInfo chatInfo)
        {
            this.chatInfo = chatInfo;

            this.messagingClient = new ChatMessagingClient(App.UserInfo.AccessToken);

            // поменять хардкод ид чата 7
            oldMessages = App.ChatService.GetAllChatMesages(App.UserInfo.AccessToken, 7);

            oldMessages = oldMessages.AsEnumerable().OrderBy(o => o.ReceivedDate).ToList();

            AddMessagesToClient(oldMessages);
            


            InitializeComponent();
            this.BindingContext = messagingClient;
        }

        private void AddMessagesToClient(List<MessageShortInfo> messages)
        {
            foreach (MessageShortInfo message in messages)
            {
                messagingClient.Messages.Add(message as MessageInfo);
            }
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (App.MessagingClient.IsConnected)
            {
                App.MessagingClient.Message = new RemoteProviders.Models.MessageInput()
                {
                    Text = textMess.Text,
                    ReceiverID = chatInfo.PartnerID
                };

                App.MessagingClient.SendMessage();
            }
        }
    }
}