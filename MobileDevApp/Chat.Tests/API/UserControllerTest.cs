using Chat.API.Controllers;
using Chat.API.Models;
using Chat.Business.Interfaces;
using Chat.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Security.Claims;

namespace Chat.Tests.API
{
    [TestFixture]
    public class UserControllerTest
    {
        private static readonly UserInfo _testUser = new UserInfo
        {
            UserID = 1,
            Name = "Test Name",
            PhoneNumber = "123456789",
            Email = "test@gmail.com",
            AccessToken = "11TOKEN11",
            Bio = "I am student of KPI)"
        };

        private readonly Mock<IUserManager> _mockUserManager;

        public UserControllerTest()
        {
            _mockUserManager = new Mock<IUserManager>();
        }

        [Test]
        public void InfoByUserIdTest()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserInfo(It.IsAny<int>()))
                .Returns(_testUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
           }, "mock"));

            var userController = new UserController(_mockUserManager.Object);
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            int userId = 1;
            ObjectResult result = userController.Info(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf(typeof(UserInfo), result.Value);

            UserInfo foundUser = result.Value as UserInfo;
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Name", foundUser.Name);
            Assert.AreEqual("123456789", foundUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", foundUser.Email);
            Assert.AreEqual("11TOKEN11", foundUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", foundUser.Bio);
            Assert.IsNull(foundUser.Image);
        }

        [Test]
        public void InfoByEmailTest()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserInfo(It.IsAny<string>()))
                .Returns(_testUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
           }, "mock"));

            var userController = new UserController(_mockUserManager.Object);
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            string email = "test@gmail.com";
            ObjectResult result = userController.Info(email);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf(typeof(UserInfo), result.Value);

            UserInfo foundUser = result.Value as UserInfo;
            Assert.IsNotNull(result);
            Assert.AreEqual("Test Name", foundUser.Name);
            Assert.AreEqual("123456789", foundUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", foundUser.Email);
            Assert.AreEqual("11TOKEN11", foundUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", foundUser.Bio);
            Assert.IsNull(foundUser.Image);
        }

        [Test]
        public void InfoByPhoneTest()
        {
            // Arrange
            _mockUserManager.Setup(um => um.GetUserInfo(It.IsAny<string>()))
                .Returns(_testUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
           }, "mock"));

            var userController = new UserController(_mockUserManager.Object);
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            string phone = "123456789";
            ObjectResult result = userController.Info(phone);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf(typeof(UserInfo), result.Value);

            UserInfo foundUser = result.Value as UserInfo;
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("Test Name", foundUser.Name);
            Assert.AreEqual("123456789", foundUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", foundUser.Email);
            Assert.AreEqual("11TOKEN11", foundUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", foundUser.Bio);
            Assert.IsNull(foundUser.Image);
        }

        [Test]
        public void RegisterTest()
        {
            // Arrange
            UserRegister newUser = new UserRegister
            {
                Name = "Test Name",
                Login = "123456789",
                LoginType = LoginType.PhoneNumber,
                PasswordHash = "password"
            };

            _mockUserManager.Setup(um => um.RegisterUser(It.IsAny<UserRegister>()))
                .Returns(_testUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var userController = new UserController(_mockUserManager.Object);
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            ObjectResult result = userController.Register(newUser);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf(typeof(UserInfo), result.Value);

            UserInfo registeredUser = result.Value as UserInfo;
            Assert.IsNotNull(registeredUser);
            Assert.AreEqual("Test Name", registeredUser.Name);
            Assert.AreEqual("123456789", registeredUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", registeredUser.Email);
            Assert.AreEqual("11TOKEN11", registeredUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", registeredUser.Bio);
            Assert.IsNull(registeredUser.Image);
        }

        [Test]
        public void LoginTest()
        {
            // Arrange
            UserLogin userLogin = new UserLogin
            {
                Login = "test@gmail.com",
                LoginType = LoginType.Email,
                PasswordHash = "password"
            };

            _mockUserManager.Setup(um => um.GetUserInfo(It.IsAny<UserLogin>()))
                .Returns(_testUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
            }, "mock"));

            var userController = new UserController(_mockUserManager.Object);
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            ObjectResult result = userController.Login(userLogin);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf(typeof(UserInfo), result.Value);

            UserInfo foundUser = result.Value as UserInfo;
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("Test Name", foundUser.Name);
            Assert.AreEqual("123456789", foundUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", foundUser.Email);
            Assert.AreEqual("11TOKEN11", foundUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", foundUser.Bio);
            Assert.IsNull(foundUser.Image);
        }

        [Test]
        public void LoginTestNotFound()
        {
            // Arrange
            UserLogin userLogin = new UserLogin
            {
                Login = "test@gmail.com",
                LoginType = LoginType.Email,
                PasswordHash = "password"
            };

            _mockUserManager.Setup(um => um.GetUserInfo(It.IsAny<UserLogin>()))
                .Returns(() => null);

            var userController = new UserController(_mockUserManager.Object);

            // Act
            ObjectResult result = userController.Login(userLogin);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

            ErrorMessage errorMessage = result.Value as ErrorMessage;
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual("User was not found", errorMessage.Message);
        }

        [Test]
        public void EditTest()
        {
            // Arrange
            UserEdit userEdit = new UserEdit
            {
                Name = "Test Name",
                Bio = "I am student of KPI)"
            };

            _mockUserManager.Setup(um => um.UpdateUserInfo(It.IsAny<UserInfo>()))
                .Returns(_testUser);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
           }, "mock"));

            var userController = new UserController(_mockUserManager.Object);
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            ObjectResult result = userController.Edit(userEdit);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(200, result.StatusCode);
            Assert.IsInstanceOf(typeof(UserInfo), result.Value);

            UserInfo foundUser = result.Value as UserInfo;
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("Test Name", foundUser.Name);
            Assert.AreEqual("123456789", foundUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", foundUser.Email);
            Assert.AreEqual("11TOKEN11", foundUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", foundUser.Bio);
            Assert.IsNull(foundUser.Image);
        }

        [Test]
        public void EditTestUnauthorized()
        {
            // Arrange
            UserEdit userEdit = new UserEdit
            {
                Name = "Test Name",
                Bio = "I am student of KPI)"
            };

            _mockUserManager.Setup(um => um.UpdateUserInfo(It.IsAny<UserInfo>()))
                .Returns(_testUser);

            var userController = new UserController(_mockUserManager.Object);

            // Act
            ObjectResult result = userController.Edit(userEdit);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(401, result.StatusCode);

            ErrorMessage errorMessage = result.Value as ErrorMessage;
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual("User was not authorized properly", errorMessage.Message);
        }

        [Test]
        public void EditTestInternalError()
        {
            // Arrange
            UserEdit userEdit = new UserEdit
            {
                Name = "Test Name",
                Bio = "I am student of KPI)"
            };

            _mockUserManager.Setup(um => um.UpdateUserInfo(It.IsAny<UserInfo>()))
                .Returns(() => null);

            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "1"),
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim("custom-claim", "example claim value"),
           }, "mock"));

            var userController = new UserController(_mockUserManager.Object);
            userController.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };

            // Act
            ObjectResult result = userController.Edit(userEdit);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(500, result.StatusCode);

            ErrorMessage errorMessage = result.Value as ErrorMessage;
            Assert.IsNotNull(errorMessage);
            Assert.AreEqual("Failed to edit the user info", errorMessage.Message);
        }
    }
}
