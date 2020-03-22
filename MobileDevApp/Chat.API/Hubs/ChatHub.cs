using Chat.API.Models;
using Chat.Business.Interfaces;
using Chat.Business.Models;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Chat.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatsManager _chatsManager;

        public ChatHub(IChatsManager chatsManager)
        {
            _chatsManager = chatsManager ?? throw new ArgumentNullException(nameof(chatsManager));
        }

        public async Task Send(MessageInput message)
        {
            ErrorMessage error = new ErrorMessage
            {
                Message = "Failed to send message"
            };
            if (!int.TryParse(this.Context.UserIdentifier, out int senderID))
                await this.Clients.Users(new List<string> { this.Context.UserIdentifier }).SendAsync("Receive", null, error);

            var messageInfo = new MessageInfo
            {
                Text = message.Text,
                ChatID = message.ChatID,
                SenderID = senderID,
                ReceiverID = message.ReceiverID,
                ReceivedDate = DateTime.Now,
                IsRead = Clients.User(message.ReceiverID.ToString()) != null ? true : false
            };
            messageInfo = _chatsManager.StoreMessage(messageInfo);
            if (messageInfo == null)
                await this.Clients.Users(new List<string> { this.Context.UserIdentifier }).SendAsync("Receive", null, error);

            await this.Clients.Users(new List<string> { this.Context.UserIdentifier, message.ReceiverID.ToString() })
                .SendAsync("Receive", messageInfo, null);
        }
    }
}
