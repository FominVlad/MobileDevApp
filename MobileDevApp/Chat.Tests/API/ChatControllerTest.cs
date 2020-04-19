using Chat.API.Controllers;
using Chat.API.Models;
using Chat.Business.Interfaces;
using Chat.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Chat.Tests.API
{
    [TestFixture]
    public class ChatControllerTest
    {
        private readonly Mock<IChatsManager> _mockChatsManager;

        public ChatControllerTest()
        {
            _mockChatsManager = new Mock<IChatsManager>();
        }

        [Test]
        public void GetAllChatsTest()
        {
            // Arrange
            List<ChatShortInfo> allChats = new List<ChatShortInfo>
            {
                new ChatShortInfo
                {
                    ChatID = 1,
                    PartnerID = 1,
                    LastMessage = new MessageShortInfo
                    {
                        Text = "Hi!",
                        ReceivedDate = DateTime.Now.AddHours(-3),
                        SenderID = 1,
                        IsRead = false
                    }
                },
                new ChatShortInfo
                {
                    ChatID = 2,
                    PartnerID = 2,
                    LastMessage = new MessageShortInfo
                    {
                        Text = "How are you?",
                        ReceivedDate = DateTime.Now.AddHours(-1),
                        SenderID = 2,
                        IsRead = true
                    }
                },
            };

            _mockChatsManager.Setup(cm => cm.GetAllChats(It.IsAny<int>()))
                .Returns(allChats);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var chatController = new ChatController(_mockChatsManager.Object);
            chatController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            ObjectResult result = chatController.GetAllChats();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf(typeof(List<ChatShortInfo>), result.Value);

            List<ChatShortInfo> resultValue = result.Value as List<ChatShortInfo>;
            Assert.AreEqual(2, resultValue.Count);

            ChatShortInfo firstChat = resultValue.First();
            Assert.AreEqual(1, firstChat.ChatID);
            Assert.AreEqual(1, firstChat.PartnerID);
            Assert.AreEqual("Hi!", firstChat.LastMessage.Text);
            Assert.AreEqual(1, firstChat.LastMessage.SenderID);
            Assert.IsFalse(firstChat.LastMessage.IsRead);

            ChatShortInfo secondChat = resultValue.Last();
            Assert.AreEqual(2, secondChat.ChatID);
            Assert.AreEqual(2, secondChat.PartnerID);
            Assert.AreEqual("How are you?", secondChat.LastMessage.Text);
            Assert.AreEqual(2, secondChat.LastMessage.SenderID);
            Assert.IsTrue(secondChat.LastMessage.IsRead);
        }

        [Test]
        public void GetAllChatsTestUnauthorized()
        {
            _mockChatsManager.Setup(cm => cm.GetAllChats(It.IsAny<int>()))
                .Returns(new List<ChatShortInfo>());

            var chatController = new ChatController(_mockChatsManager.Object);

            // Act
            ObjectResult result = chatController.GetAllChats();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);

            ErrorMessage errorMessage = result.Value as ErrorMessage;
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual("User was not authorized properly", errorMessage.Message);
        }

        [Test]
        public void GetAllChatMesagesTest()
        {
            List<MessageShortInfo> allChatMessages = new List<MessageShortInfo>
            {
                new MessageShortInfo
                {
                    Text = "Hi! How are you?",
                    SenderID = 1,
                    ReceivedDate = DateTime.Now.AddMinutes(-25),
                    IsRead = true
                },
                 new MessageShortInfo
                {
                    Text = "Hi! I'm fine)",
                    SenderID = 2,
                    ReceivedDate = DateTime.Now.AddMinutes(-23),
                    IsRead = false
                }
            };

            _mockChatsManager.Setup(cm => cm.GetChatMessages(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(allChatMessages);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var chatController = new ChatController(_mockChatsManager.Object);
            chatController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            ObjectResult result = chatController.GetAllChatMesages(1, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf(typeof(List<MessageShortInfo>), result.Value);

            List<MessageShortInfo> resultValue = result.Value as List<MessageShortInfo>;
            Assert.AreEqual(2, resultValue.Count);

            MessageShortInfo firstMessage = resultValue.First();
            Assert.AreEqual("Hi! How are you?", firstMessage.Text);
            Assert.AreEqual(1, firstMessage.SenderID);
            Assert.IsTrue(firstMessage.IsRead);

            MessageShortInfo secondMessage = resultValue.Last();
            Assert.AreEqual("Hi! I'm fine)", secondMessage.Text);
            Assert.AreEqual(2, secondMessage.SenderID);
            Assert.IsFalse(secondMessage.IsRead);
        }

        [Test]
        public void GetAllChatMesagesTestUnauthorized()
        {
            _mockChatsManager.Setup(cm => cm.GetChatMessages(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>()))
                .Returns(new List<MessageShortInfo>());

            var chatController = new ChatController(_mockChatsManager.Object);

            // Act
            ObjectResult result = chatController.GetAllChatMesages(1, null, null);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);

            ErrorMessage errorMessage = result.Value as ErrorMessage;
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual("User was not authorized properly", errorMessage.Message);
        }
    }
}
