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
    public class ChatsManager : IChatsManager
    {
        private readonly IChatUnitOfWork _chatUnitOfWork;

        public ChatsManager(IChatUnitOfWork chatUnitOfWork)
        {
            _chatUnitOfWork = chatUnitOfWork ?? throw new ArgumentNullException(nameof(chatUnitOfWork));
        }

        public List<ChatShortInfo> GetAllChats(int userID)
        {
            List <ChatEntity> userChats = _chatUnitOfWork.ChatsRepository.FindAll(
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
                        ChatID = chat.ChatID,
                        PartnerID = partner.UserID,
                        PartnerImage = partner.Image?.Image,
                        LastMessage = new MessageShortInfo 
                        { 
                            Text = lastMessage.Text,
                            ReceivedDate = lastMessage.ReceivedDate,
                            SenderID = lastMessage.SenderID,
                            IsRead = lastMessage.IsRead
                        }
                    });
            }

            return userChatsShortInfo;
        }

        public List<MessageShortInfo> GetChatMessages(int userID, int chatID,
            int? skipCount = null, int? takeCount = null)
        {
            List<Message> chatMessages = _chatUnitOfWork.MessagesRepository.TakeOrdered(
                new ExpressionSpecification<Message>(message => message.ChatID == chatID &&
                    message.Chat.Users.Any(u => u.UserID == userID)),
                message => message.Include(m => m.Chat).ThenInclude(c => c.Users),
                message => message.ReceivedDate,
                skipCount, takeCount, true);

            return chatMessages
                .Select(m => new MessageShortInfo
                    {
                        SenderID = m.SenderID,
                        Text = m.Text,
                        ReceivedDate = m.ReceivedDate,
                        IsRead = m.IsRead
                    })
                .ToList();
        }

        public MessageInfo StoreMessage(MessageInfo newMessage)
        {
            ExpressionSpecification<ChatEntity> findChatExp = newMessage.ChatID != null ?
                new ExpressionSpecification<ChatEntity>(c => c.ChatID == newMessage.ChatID) :
                new ExpressionSpecification<ChatEntity>(
                    c => c.Users.Any(u => u.UserID == newMessage.SenderID) &&
                         c.Users.Any(u => u.UserID == newMessage.ReceiverID));

            ChatEntity chat = _chatUnitOfWork.ChatsRepository.FirstOrDefault(findChatExp,
                chatEnt => chatEnt.Include(c => c.Users).Include(c => c.Messages));

            var dbMessage = new Message
            {
                Text = newMessage.Text,
                SenderID = newMessage.SenderID,
                ReceivedDate = newMessage.ReceivedDate,
                IsRead = newMessage.IsRead
            };

            if (chat == null)
            {
                chat = new ChatEntity
                {
                    SenderID = newMessage.SenderID,
                    ReceiverID = newMessage.ReceiverID
                };
                if (_chatUnitOfWork.ChatsRepository.Create(chat) <= 0)
                    return null;
                chat = _chatUnitOfWork.ChatsRepository.FirstOrDefault(
                    new ExpressionSpecification<ChatEntity>(
                        c => c.SenderID == newMessage.SenderID && c.ReceiverID == newMessage.ReceiverID),
                    chatEntity => chatEntity.Include(c => c.Users));
                if (chat == null)
                    return null;
                chat.Users.Add(new ChatUser { ChatID = chat.ChatID, UserID = newMessage.SenderID });
                chat.Users.Add(new ChatUser { ChatID = chat.ChatID, UserID = newMessage.ReceiverID });
                if (_chatUnitOfWork.ChatsRepository.Update(chat) <= 0)
                    return null;
            }

            dbMessage.ChatID = chat.ChatID;
            newMessage.ChatID = chat.ChatID;

            if (_chatUnitOfWork.MessagesRepository.Create(dbMessage) <= 0)
                return null;

            return newMessage;
        }
    }
}
