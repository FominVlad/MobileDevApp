using MobileDevApp.RemoteProviders.Interfaces;
using MobileDevApp.RemoteProviders.Misc;
using MobileDevApp.RemoteProviders.Models;
using System;
using System.Net.Http;

namespace MobileDevApp.RemoteProviders.Implementations
{
    public class UserService : IUserService
    {
        private readonly IHttpProvider _httpProvider;

        public UserService(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider ?? throw new ArgumentNullException(nameof(httpProvider));
        }

        public UserInfo Info(int userId, string userAuthToken)
        {
            var requestMessage = new HttpRequestMessage(
                HttpMethod.Get, $"{Configuration.UserInfoIdGettingRoute}/{userId}");
            requestMessage.Headers.Add(Configuration.AuthHeaderKey, userAuthToken);

            return _httpProvider.SendRequest<UserInfo>(requestMessage);
        }

        public UserInfo Info(string userSearchInfo, string userAuthToken)
        {
            var requestMessage = new HttpRequestMessage(
                HttpMethod.Get, $"{Configuration.UserInfoNameGettingRoute}/{userSearchInfo}");
            requestMessage.Headers.Add(Configuration.AuthHeaderKey, userAuthToken);

            return _httpProvider.SendRequest<UserInfo>(requestMessage);
        }

        public UserInfo Register(UserRegister newUser)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Configuration.UserInfoRegisterRoute);
            requestMessage.AddStringContent(newUser);

            return _httpProvider.SendRequest<UserInfo>(requestMessage);
        }

        public UserInfo Login(UserLogin userLoginInfo)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Configuration.UserInfoLoginRoute);
            requestMessage.AddStringContent(userLoginInfo);

            return _httpProvider.SendRequest<UserInfo>(requestMessage);
        }

        public UserInfo Edit(UserEdit userEditInfo, string userAuthToken)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, Configuration.UserInfoEditRoute);
            requestMessage.Headers.Add(Configuration.AuthHeaderKey, userAuthToken);
            requestMessage.AddStringContent(userEditInfo);

            return _httpProvider.SendRequest<UserInfo>(requestMessage);
        }
    }
}
