using MobileDevApp.RemoteProviders.Implementations;
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
        ChatMessagingClient viewModel;
        public DialogPage()
        {
            InitializeComponent();
            viewModel = new ChatMessagingClient(App.UserInfo.AccessToken);
            this.BindingContext = viewModel;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.Connect();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            viewModel.Disconnect();
        }
    }
}