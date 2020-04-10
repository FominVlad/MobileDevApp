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
        private readonly Mock<IChatUnitOfWork> _mockChatUnitOfWork;

        public ChatsManagerTest()
        {
            _mockChatUnitOfWork = new Mock<IChatUnitOfWork>();
        }

        [Test]
        public void GetAllChatsTest()
        {
            List<ChatEntity> testChatEntities = new List<ChatEntity>
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
                }
            };

            _mockChatUnitOfWork.Setup(uow => uow.ChatsRepository.FindAll(
                It.IsAny<ExpressionSpecification<ChatEntity>>(), 
                It.IsAny<Func<IQueryable<ChatEntity>, IQueryable<ChatEntity>>>()))
                .Returns()
        }
    }
}
