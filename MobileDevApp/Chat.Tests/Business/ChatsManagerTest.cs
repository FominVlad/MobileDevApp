using Chat.Business.Implementations;
using Chat.Business.Models;
using Chat.DAL.Interfaces;
using Chat.DAL.Models;
using Moq;
using NUnit.Framework;
using Patterns.Specification.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Chat.Tests.Business
{
    [TestFixture]
    public class ChatsManagerTest
    {
        private static readonly List<ChatEntity> _testChatEntities = 
            new List<ChatEntity>
            {
                new ChatEntity
                {
                    ChatID = 1,
                    SenderID = 1,
                    ReceiverID = 2,
                    Users = new List<ChatUser>
                    {
                        new ChatUser { ChatID = 1, UserID = 1, User = new User { UserID = 1 }},
                        new ChatUser { ChatID = 1, UserID = 2, User = new User { UserID = 2 }}
                    },
                    Messages = new List<Message>
                    {
                        new Message { ChatID = 1, MessageID = 1, Text = "Hi!", 
                            ReceivedDate = DateTime.Now.AddMinutes(-30), SenderID = 1, IsRead = true },
                        new Message { ChatID = 1, MessageID = 2, Text = "Hello!",
                            ReceivedDate = DateTime.Now.AddMinutes(-25), SenderID = 2, IsRead = true },
                    }
                },
                new ChatEntity
                {
                    ChatID = 2,
                    SenderID = 2,
                    ReceiverID = 3,
                    Users = new List<ChatUser>
                    {
                        new ChatUser { ChatID = 2, UserID = 2, User = new User { UserID = 2 }},
                        new ChatUser { ChatID = 2, UserID = 3, User = new User { UserID = 3 }}
                    },
                    Messages = new List<Message>
                    {
                        new Message { ChatID = 2, MessageID = 1, Text = "Hi!",
                            ReceivedDate = DateTime.Now.AddMinutes(-36), SenderID = 3, IsRead = true },
                        new Message { ChatID = 2, MessageID = 2, Text = "How are you?",
                            ReceivedDate = DateTime.Now.AddMinutes(-35), SenderID = 3, IsRead = true },
                        new Message { ChatID = 2, MessageID = 3, Text = "Fine)",
                            ReceivedDate = DateTime.Now.AddMinutes(-33), SenderID = 2, IsRead = false },
                    }
                }
            };

        private readonly Mock<IChatUnitOfWork> _mockChatUnitOfWork;

        public ChatsManagerTest()
        {
            _mockChatUnitOfWork = new Mock<IChatUnitOfWork>();
        }

        [Test]
        public void GetAllChatsTest()
        {
            // Arrange
            _mockChatUnitOfWork.Setup(uow => uow.ChatsRepository.FindAll(
                It.IsAny<ExpressionSpecification<ChatEntity>>(),
                It.IsAny<Func<IQueryable<ChatEntity>, IQueryable<ChatEntity>>>()))
                .Returns(_testChatEntities);

            var chatsManager = new ChatsManager(_mockChatUnitOfWork.Object);

            // Act
            int searchUserId = 2;
            List<ChatShortInfo> allChats = chatsManager.GetAllChats(searchUserId);

            // Assert
            Assert.AreEqual(2, allChats.Count);

            ChatShortInfo firstChatInfo = allChats.FirstOrDefault(c => c.ChatID == 1);
            Assert.IsNotNull(firstChatInfo);
            Assert.AreEqual(1, firstChatInfo.PartnerID);
            Assert.AreEqual("Hello!", firstChatInfo.LastMessage.Text);
            Assert.AreEqual(2, firstChatInfo.LastMessage.SenderID);
            Assert.IsTrue(firstChatInfo.LastMessage.IsRead);

            ChatShortInfo secondChatInfo = allChats.FirstOrDefault(c => c.ChatID == 2);
            Assert.IsNotNull(secondChatInfo);
            Assert.AreEqual(3, secondChatInfo.PartnerID);
            Assert.AreEqual("Fine)", secondChatInfo.LastMessage.Text);
            Assert.AreEqual(2, secondChatInfo.LastMessage.SenderID);
            Assert.IsFalse(secondChatInfo.LastMessage.IsRead);
        }

        [Test]
        public void GetChatMessagesTest()
        {
            // Arrange
            int userId = 2;
            int chatId = 2;
            _mockChatUnitOfWork.Setup(uow => uow.MessagesRepository.TakeOrdered(
                It.IsAny<ExpressionSpecification<Message>>(),
                It.IsAny<Func<IQueryable<Message>, IQueryable<Message>>>(),
                It.IsAny<Func<Message,DateTime>>(),
                It.IsAny<int?>(),
                It.IsAny<int?>(),
                It.IsAny<bool>()))
                .Returns(_testChatEntities.First(c => c.ChatID == chatId).Messages.ToList());

            var chatsManager = new ChatsManager(_mockChatUnitOfWork.Object);

            // Act
            List<MessageShortInfo> allChatMessages = chatsManager.GetChatMessages(userId, chatId);

            // Assert
            Assert.AreEqual(3, allChatMessages.Count);

            MessageShortInfo firstMessage = allChatMessages.First();
            Assert.AreEqual("Hi!", firstMessage.Text);
            Assert.AreEqual(3, firstMessage.SenderID);
            Assert.IsTrue(firstMessage.IsRead);

            MessageShortInfo lastMessage = allChatMessages.Last();
            Assert.AreEqual("Fine)", lastMessage.Text);
            Assert.AreEqual(2, lastMessage.SenderID);
            Assert.IsFalse(lastMessage.IsRead);
        }

        [Test]
        public void StoreMessageTest()
        {
            // Arrange
            ChatEntity testChat = _testChatEntities.First();
            MessageInfo message = new MessageInfo
            {
                ReceiverID = 2,
                Text = "How are you?",
                ReceivedDate = DateTime.Now.AddMinutes(-20),
                SenderID = 1,
                IsRead = true
            };
            Message dbMessage = new Message
            {
                MessageID = 3,
                Text = message.Text,
                ReceivedDate = message.ReceivedDate,
                IsRead = message.IsRead,
                SenderID = message.SenderID
            };

            _mockChatUnitOfWork.Setup(uow => uow.ChatsRepository.FirstOrDefault(
                It.IsAny<ExpressionSpecification<ChatEntity>>(),
                It.IsAny<Func<IQueryable<ChatEntity>, IQueryable<ChatEntity>>>()))
                .Returns(testChat);
            _mockChatUnitOfWork.Setup(uow => uow.MessagesRepository.Create(It.IsAny<Message>()))
                .Callback(() => testChat.Messages.Add(dbMessage))
                .Returns(1);

            var chatsManager = new ChatsManager(_mockChatUnitOfWork.Object);

            // Act
            message = chatsManager.StoreMessage(message);

            // Assert
            Assert.AreEqual(1, message.ChatID);
            Assert.AreEqual(3, testChat.Messages.Count);
        }
    }
}
