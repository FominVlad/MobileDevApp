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
    public class UserManagerTest
    {
        private static readonly string _secretString = "SECRETKEYSTRINGSECRETKEYSTRINGSECRETKEYSTRING";

        private static readonly List<User> _testUsers = new List<User>
        {
            new User
            {
                UserID = 1,
                Name = "Test Name",
                PhoneNumber = "123456789",
                Email = "test@gmail.com",
                PasswordHash = "password",
                Token = "11TOKEN11",
                Bio = "I am student of KPI)"
            }
        };

        private readonly Mock<IChatUnitOfWork> _mockChatUnitOfWork;

        public UserManagerTest()
        {
            _mockChatUnitOfWork = new Mock<IChatUnitOfWork>();
        }

        [Test]
        public void GetUserInfoByIdTest()
        {
            // Arrange
            _mockChatUnitOfWork.Setup(uow => uow.UsersRepository.FirstOrDefault(
                It.IsAny<ExpressionSpecification<User>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .Returns(_testUsers.First());

            var userManager = new UserManager(_mockChatUnitOfWork.Object, _secretString);

            // Act
            int userId = 1;
            UserInfo foundUser = userManager.GetUserInfo(userId);

            // Assert
            Assert.IsNotNull(foundUser);
            


        }

        [Test]
        public void GetUserInfoByEmailTest()
        {
            // Arrange
            _mockChatUnitOfWork.Setup(uow => uow.UsersRepository.FirstOrDefault(
                It.IsAny<ExpressionSpecification<User>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .Returns(_testUsers.First());

            var userManager = new UserManager(_mockChatUnitOfWork.Object, _secretString);

            // Act
            string email = "test@gmail.com";
            UserInfo foundUser = userManager.GetUserInfo(email);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("Test Name", foundUser.Name);
            Assert.AreEqual("123456789", foundUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", foundUser.Email);
            Assert.AreEqual("11TOKEN11", foundUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", foundUser.Bio);
            Assert.IsNull(foundUser.Image);
        }

        [Test]
        public void GetUserInfoByPhoneTest()
        {
            // Arrange
            _mockChatUnitOfWork.Setup(uow => uow.UsersRepository.FirstOrDefault(
                It.IsAny<ExpressionSpecification<User>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .Returns(_testUsers.First());

            var userManager = new UserManager(_mockChatUnitOfWork.Object, _secretString);

            // Act
            string phoneNumber = "123456789";
            UserInfo foundUser = userManager.GetUserInfo(phoneNumber);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("Test Name", foundUser.Name);
            Assert.AreEqual("123456789", foundUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", foundUser.Email);
            Assert.AreEqual("11TOKEN11", foundUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", foundUser.Bio);
            Assert.IsNull(foundUser.Image);
        }

        [Test]
        public void GetUserInfoByPhoneAndPasswordTest()
        {
            // Arrange
            _mockChatUnitOfWork.Setup(uow => uow.UsersRepository.FirstOrDefault(
                It.IsAny<ExpressionSpecification<User>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .Returns(_testUsers.First());

            var userManager = new UserManager(_mockChatUnitOfWork.Object, _secretString);

            // Act
            UserLogin userLoginData = new UserLogin
            {
                Login = "123456789",
                LoginType = LoginType.PhoneNumber,
                PasswordHash = "password"
            };
            UserInfo foundUser = userManager.GetUserInfo(userLoginData);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("Test Name", foundUser.Name);
            Assert.AreEqual("123456789", foundUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", foundUser.Email);
            Assert.AreEqual("11TOKEN11", foundUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", foundUser.Bio);
            Assert.IsNull(foundUser.Image);
        }

        [Test]
        public void GetUserInfoByEmailAndPasswordTest()
        {
            // Arrange
            _mockChatUnitOfWork.Setup(uow => uow.UsersRepository.FirstOrDefault(
                It.IsAny<ExpressionSpecification<User>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .Returns(_testUsers.First());

            var userManager = new UserManager(_mockChatUnitOfWork.Object, _secretString);

            // Act
            UserLogin userLoginData = new UserLogin
            {
                Login = "test@gmail.com",
                LoginType = LoginType.Email,
                PasswordHash = "password"
            };
            UserInfo foundUser = userManager.GetUserInfo(userLoginData);

            // Assert
            Assert.IsNotNull(foundUser);
            Assert.AreEqual("Test Name", foundUser.Name);
            Assert.AreEqual("123456789", foundUser.PhoneNumber);
            Assert.AreEqual("test@gmail.com", foundUser.Email);
            Assert.AreEqual("11TOKEN11", foundUser.AccessToken);
            Assert.AreEqual("I am student of KPI)", foundUser.Bio);
            Assert.IsNull(foundUser.Image);
        }

        [Test]
        public void RegisterUserTest()
        {
            // Arrange
            UserRegister newUser = new UserRegister
            {
                Name = "New User",
                Login = "987654321",
                LoginType = LoginType.PhoneNumber,
                PasswordHash = "passwordHash"
            };
            User dbUser = new User
            {
                Name = newUser.Name,
                PhoneNumber = newUser.Login,
                PasswordHash = newUser.PasswordHash
            };

            int counter = -1;
            _mockChatUnitOfWork.Setup(uow => uow.UsersRepository.FirstOrDefault(
                It.IsAny<ExpressionSpecification<User>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .Returns(() => ++counter > 0 ? _testUsers.First() : null);
            _mockChatUnitOfWork.Setup(uow => uow.UsersRepository.Create(It.IsAny<User>()))
                .Callback(() => { dbUser.Token = "Token"; _testUsers.Add(dbUser); })
                .Returns(1);

            var userManager = new UserManager(_mockChatUnitOfWork.Object, _secretString);

            // Act
            UserInfo registeredUser = userManager.RegisterUser(newUser);

            // Assert
            Assert.IsNotNull(registeredUser.AccessToken);
            Assert.AreEqual(2, _testUsers.Count);
        }

        [Test]
        public void UpdateUserInfoTest()
        {
            // Arrange
            var newUser = new UserInfo
            {
                UserID = 1,
                Email = "test2@gmail.com",
                Bio = "I am student",
            };
            var userToUpdate = _testUsers.First();

            _mockChatUnitOfWork.Setup(uow => uow.UsersRepository.FirstOrDefault(
                It.IsAny<ExpressionSpecification<User>>(),
                It.IsAny<Func<IQueryable<User>, IQueryable<User>>>()))
                .Returns(userToUpdate);
            _mockChatUnitOfWork.Setup(uow => uow.UsersRepository.Update(It.IsAny<User>()))
                .Returns(1);

            var userManager = new UserManager(_mockChatUnitOfWork.Object, _secretString);

            // Act
            UserInfo updatedUser = userManager.UpdateUserInfo(newUser);

            // Assert
            Assert.IsNotNull(updatedUser);
            Assert.AreEqual("Test Name", updatedUser.Name);
            Assert.AreEqual("123456789", updatedUser.PhoneNumber);
            Assert.AreEqual("test2@gmail.com", updatedUser.Email);
            Assert.AreEqual("11TOKEN11", updatedUser.AccessToken);
            Assert.AreEqual("I am student", updatedUser.Bio);
            Assert.IsNull(updatedUser.Image);
        }
    }
}
