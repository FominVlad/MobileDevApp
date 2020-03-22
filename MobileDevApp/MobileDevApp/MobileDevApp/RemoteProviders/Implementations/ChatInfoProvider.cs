using MobileDevApp.RemoteProviders.Interfaces;
using MobileDevApp.RemoteProviders.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace MobileDevApp.RemoteProviders.Implementations
{
    public class ChatInfoProvider : IChatInfoProvider
    {
        private readonly IHttpProvider _httpProvider;

        public ChatInfoProvider(IHttpProvider httpProvider)
        {
            _httpProvider = httpProvider ?? throw new ArgumentNullException(nameof(httpProvider));
        }

        public List<ChatShortInfo> GetAllUserChats(string userAuthToken)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, Configuration.ChatInfoAllChatsRoute);
            requestMessage.Headers.Add(Configuration.AuthHeaderKey, userAuthToken);

            return _httpProvider.SendRequest<List<ChatShortInfo>>(requestMessage);
        }

        public List<MessageShortInfo> GetAllChatMesages(string userAuthToken, 
            int chatID,
            int? skipCount = null,
            int? takeCount = null)
        {
            var requestQuery = $"{Configuration.ChatInfoAllChatMessagesRoute}/{chatID}";

            string queryParamsDelimiter = "?";
            if (skipCount.HasValue)
            {
                requestQuery = $"{requestQuery}?skipCount={skipCount.Value}";
                queryParamsDelimiter = "&";
            }
            if (skipCount.HasValue)
                requestQuery = $"{requestQuery}{queryParamsDelimiter}takeCount={takeCount.Value}";

            var requestMessage = new HttpRequestMessage(HttpMethod.Get, requestQuery);
            requestMessage.Headers.Add(Configuration.AuthHeaderKey, userAuthToken);
            return _httpProvider.SendRequest<List<MessageShortInfo>>(requestMessage);
        }
    }
}
