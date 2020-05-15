using MobileDevApp.RemoteProviders.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MobileDevApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Messages : ContentPage
    {
        public int ScreenHeight { get; private set; }
        public int ScreenWidth { get; private set; }

        public ObservableCollection<ChatShortInfo> chatInfoList { get; set; }

        public Messages()
        {
            InitializeComponent();

            chatInfoList = GetFilledCollection(App.ChatService.GetAllUserChats(App.UserInfo.AccessToken));

            this.BindingContext = this;

            ScreenHeight = (int)DeviceDisplay.MainDisplayInfo.Height;
            ScreenWidth = (int)DeviceDisplay.MainDisplayInfo.Width;

            btnAddMessage.Source = ImageSource.FromResource("MobileDevApp.Resources.add.png");

            btnAddMessage.WidthRequest = ScreenWidth / 15;
            btnAddMessage.HeightRequest = ScreenWidth / 15;
        }

        private ObservableCollection<ChatShortInfo> GetFilledCollection(List<ChatShortInfo> chatList)
        {
            ObservableCollection<ChatShortInfo> resultList = new ObservableCollection<ChatShortInfo>();

            foreach (ChatShortInfo chat in chatList)
            {
                resultList.Add(chat);
            }

            return resultList;
        }

        private async void btnAddMessage_Clicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SearchProfilePage());
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

        private void listMessages_Refreshing(object sender, System.EventArgs e)
        {
            List<ChatShortInfo> exceptList = GetFilledCollection(App.ChatService.
                GetAllUserChats(App.UserInfo.AccessToken)).Except(chatInfoList, new ChatShortInfoComparer()).ToList();
            foreach (ChatShortInfo chat in exceptList)
            {
                chatInfoList.Add(chat);
            }
            
            listMessages.IsRefreshing = false;
        }
    }
}