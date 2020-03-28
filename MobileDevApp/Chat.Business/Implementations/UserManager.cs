using Chat.Business.Interfaces;
using Chat.Business.Models;
using Chat.DAL.Interfaces;
using Chat.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Patterns.Specification.Implementations;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.Business.Implementations
{
    public class UserManager : IUserManager
    {
        private readonly IChatUnitOfWork _chatUnitOfWork;
        private readonly string _secret;

        public UserManager(IChatUnitOfWork chatUnitOfWork, string secret)
        {
            _chatUnitOfWork = chatUnitOfWork ?? throw new ArgumentNullException(nameof(chatUnitOfWork));
            _secret = secret ?? throw new ArgumentNullException(nameof(secret));
        }

        public UserInfo GetUserInfo(string userName)
        {
            if (userName == null)
                throw new ArgumentNullException(nameof(userName));

            var expression = new ExpressionSpecification<User>(
                user => user.Name == userName);

            User registeredUser = _chatUnitOfWork.UsersRepository.FirstOrDefault(
                expression,
                user => user.Include(u => u.Image));

            return ConvertToUserInfo(registeredUser);
        }

        public UserInfo GetUserInfo(UserLogin userLoginData)
        {
            if (userLoginData == null)
                throw new ArgumentNullException(nameof(userLoginData));

            var expression = new ExpressionSpecification<User>(
                user => user.Email == userLoginData.Login
                        && userLoginData.PasswordHash == user.PasswordHash);

            User registeredUser = _chatUnitOfWork.UsersRepository.FirstOrDefault(
                expression,
                user => user.Include(u => u.Image));

            return ConvertToUserInfo(registeredUser);
        }

        public UserInfo RegisterUser(UserRegister newUser)
        {
            if (newUser == null)
                throw new ArgumentNullException(nameof(newUser));

            UserInfo registeredUserInfo = GetUserInfo(newUser);

            if (registeredUserInfo != null)
                return registeredUserInfo;

            User registeredUser = new User
            {
                Name = newUser.Name,
                PasswordHash = newUser.PasswordHash,
                QRCode = newUser.QRCode
            };
            string loginClaimType = null;
            if (newUser.LoginType == LoginType.PhoneNumber)
            {
                registeredUser.PhoneNumber = newUser.Login;
                loginClaimType = ClaimTypes.MobilePhone;
            }
            else
            {
                registeredUser.Email = newUser.Login;
                loginClaimType = ClaimTypes.Email;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_secret);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, newUser.Name),
                    new Claim(loginClaimType, newUser.Login)
                },
                "Token",
                ClaimTypes.Name,
                string.Empty),
                Expires = DateTime.UtcNow.AddDays(365),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            registeredUser.Token = tokenHandler.WriteToken(token);

            if (_chatUnitOfWork.UsersRepository.Create(registeredUser) <= 0)
                return null;

            return GetUserInfo(newUser);
        }

        public UserInfo UpdateUserInfo(UserInfo newUserInfo)
        {
            if(newUserInfo == null)
                throw new ArgumentNullException(nameof(newUserInfo));

            User registeredUser = _chatUnitOfWork.UsersRepository.FirstOrDefault(
                new ExpressionSpecification<User>(user => user.UserID == newUserInfo.UserID),
                user => user.Include(u => u.Image));

            if (registeredUser == null)
                return null;

            registeredUser.Name = newUserInfo.Name ?? registeredUser.Name;
            registeredUser.PhoneNumber = newUserInfo.PhoneNumber ?? registeredUser.PhoneNumber;
            registeredUser.Email = newUserInfo.Email ?? registeredUser.Email;
            registeredUser.Bio = newUserInfo.Bio ?? registeredUser.Bio;
            if(newUserInfo.Image != null)
            {
                if (registeredUser.Image == null)
                    registeredUser.Image = new UserImage { Image = newUserInfo.Image };
                else
                    registeredUser.Image.Image = newUserInfo.Image;
            }

           if(_chatUnitOfWork.UsersRepository.Update(registeredUser) <= 0)
                return null;

            return ConvertToUserInfo(registeredUser);
        }

        private UserInfo ConvertToUserInfo(User user)
        {
            if (user == null)
                return null;

            return new UserInfo
            {
                UserID = user.UserID,
                AccessToken = user.Token,
                Name = user.Name,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Bio = user.Bio,
                Image = user.Image?.Image
            };
        }
    }
}
