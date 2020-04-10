using Newtonsoft.Json;
using System.Net.Http;
using System.Text;

namespace MobileDevApp.RemoteProviders.Misc
{
    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage AddStringContent<TContent>(this HttpRequestMessage requestMessage, TContent content)
        {
            string json = JsonConvert.SerializeObject(content);
            requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            return requestMessage;
        }
    }
}
