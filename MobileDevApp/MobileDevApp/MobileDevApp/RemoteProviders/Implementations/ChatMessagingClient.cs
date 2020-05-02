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

        public MessageInput Message { get; set; }

        public ObservableCollection<MessageInfo> Messages { get; private set; }

        public ObservableCollection<ErrorMessage> Errors { get; private set; }

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
            _hubConnection.Closed += async (error) =>
            {
                IsConnected = false;
                await Task.Delay(5000);
                Connect();
            };
            _hubConnection.On<MessageInfo, ErrorMessage>("Receive", (message, error) =>
            {
                ReceiveMessage(message, error);
            });

            Messages = new ObservableCollection<MessageInfo>();
            Errors = new ObservableCollection<ErrorMessage>();
            IsConnected = false;
            IsBusy = false;

            SendMessageCommand = new Command(() => SendMessage(), () => IsConnected);
        }

        // подключение к чату
        public async void Connect()
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
                Errors.Add(new ErrorMessage { Message = ex.Message });
            }
        }

        // Отключение от чата
        public async void Disconnect()
        {
            if (!IsConnected)
                return;

            try
            {
                await _hubConnection.StopAsync();
                IsConnected = false;
            }
            catch (Exception ex)
            {
                Errors.Add(new ErrorMessage { Message = ex.Message });
            }
        }

        // Отправка сообщения
        public async void SendMessage()
        {
            try
            {
                IsBusy = true;
                await _hubConnection.InvokeAsync("Send", Message);
            }
            catch (Exception ex)
            {
                Errors.Add(new ErrorMessage { Message = ex.Message });
            }
            finally
            {
                IsBusy = false;
            }
        }

        // Добавление сообщения
        private void ReceiveMessage(MessageInfo message, ErrorMessage error)
        {
            if(message != null)
                Messages.Add(message);
            if (error != null)
                Errors.Add(error);
        }

        public void OnPropertyChanged(string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
