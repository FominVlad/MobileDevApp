using Chat.Business.Interfaces;
using Chat.Business.Models;
using Chat.DAL.Interfaces;
using Chat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Patterns.Specification.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Business.Implementations
{
    public class ChatInfoProvider : IChatInfoProvider
    {
        private readonly IChatUnitOfWork _chatUnitOfWork;

        public ChatInfoProvider(IChatUnitOfWork chatUnitOfWork)
        {
            _chatUnitOfWork = chatUnitOfWork ?? throw new ArgumentNullException(nameof(chatUnitOfWork));
        }

        public List<ChatShortInfo> GetAllChats(int userID)
        {
            List<ChatEntity> userChats = _chatUnitOfWork.ChatsRepository.FindAll(
                new ExpressionSpecification<ChatEntity>(chat => chat.Users.Any(u => u.UserID == userID)),
                chat => chat.Include(c => c.Users).ThenInclude(m => m.User).ThenInclude(u => u.Image)
                            .Include(c => c.Messages));

            List<ChatShortInfo> userChatsShortInfo = new List<ChatShortInfo>(userChats.Count);
            User partner;
            int lastMessageID;
            Message lastMessage;
            foreach(ChatEntity chat in userChats)
            {
                partner = chat.Users.First(u => u.UserID != userID).User;
                lastMessageID = chat.Messages.Max(m => m.MessageID);
                lastMessage = chat.Messages.First(m => m.MessageID == lastMessageID);
                userChatsShortInfo.Add(
                    new ChatShortInfo
                    {
                        PartnerID = partner.UserID,
                        PartnerImage = partner.Image.Image,
                        LastMessage = lastMessage.Text,
                        LastMessageDate = lastMessage.ReceivedDate
                    });
            }

            return userChatsShortInfo;
        }

        public List<MessageInfo> GetChatMessages(int userID, int chatID, int? messagesCount = null)
        {
            List<Message> chatMessages = _chatUnitOfWork.MessagesRepository.TakeOrdered(
                new ExpressionSpecification<Message>(message => message.ChatID == chatID &&
                    message.Chat.Users.Any(u => u.UserID == userID)),
                message => message.Include(m => m.Chat).ThenInclude(c => c.Users),
                message => message.ReceivedDate,
                messagesCount,
                true);

            return chatMessages
                .Select(m => new MessageInfo
                    {
                        SenderID = m.SenderID,
                        Text = m.Text,
                        ReceivedDate = m.ReceivedDate
                    })
                .ToList();
        }
    }
}
