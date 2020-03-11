using Microsoft.AspNetCore.SignalR.Client;
using MobileDevApp.RemoteProviders.Models;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MobileDevApp.RemoteProviders.Implementations
{
    public class ChatMessagingClient : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private readonly HubConnection _hubConnection;

        public MessageInput Message { get; set; }

        public ObservableCollection<MessageInfo> Messages { get; }

        public ObservableCollection<ErrorMessage> Errors { get; }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                if (_isBusy != value)
                {
                    _isBusy = value;
                    OnPropertyChanged("IsBusy");
                }
            }
        }

        private bool _isConnected;
        public bool IsConnected
        {
            get => _isConnected;
            set
            {
                if (_isConnected != value)
                {
                    _isConnected = value;
                    OnPropertyChanged("IsConnected");
                }
            }
        }

        public Command SendMessageCommand { get; private set; }

        public ChatMessagingClient(string userAuthToken)
        {
            // создание подключения
            _hubConnection = new HubConnectionBuilder()
                .WithUrl(Configuration.ChatLiveMessagingUrl, httpOptions => 
                {
                    httpOptions.Headers.Add(Configuration.AuthHeaderKey, userAuthToken);
                })
                .Build();

            Messages = new ObservableCollection<MessageInfo>();

            IsConnected = false;
            IsBusy = false;
            SendMessageCommand = new Command(async () => await SendMessage(), () => IsConnected);

            _hubConnection.Closed += async (error) =>
            {
                IsConnected = false;
                await Task.Delay(5000);
                await Connect();
            };

            _hubConnection.On<MessageInfo, ErrorMessage>("Receive", (message, error) =>
            {
                SendLocalMessage(message, error);
            });
        }

        // подключение к чату
        public async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await _hubConnection.StartAsync();
                IsConnected = true;
            }
            catch (Exception ex)
            {
            }
        }

        // Отключение от чата
        public async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await _hubConnection.StopAsync();
            IsConnected = false;
        }

        // Отправка сообщения
        async Task SendMessage()
        {
            try
            {
                IsBusy = true;
                await _hubConnection.InvokeAsync("Send", Message);
            }
            catch (Exception ex)
            {
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Добавление сообщения
        private void SendLocalMessage(MessageInfo message, ErrorMessage error)
        {
            if(message != null)
                Messages.Insert(0, message);
            if (error != null)
                Errors.Insert(0, error);
        }

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
