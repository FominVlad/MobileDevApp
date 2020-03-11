using MobileDevApp.RemoteProviders;
using MobileDevApp.RemoteProviders.Interfaces;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading;

namespace FoodDelivery.Business.Implementations
{
    public class HttpProvider : IHttpProvider
    {
        private readonly HttpClient _client;

        public HttpProvider(HttpClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public TResult SendRequest<TResult>(HttpRequestMessage requestMessage)
        {
            TimeSpan currentWaitingTime;
            for (int i = 0; i < Configuration.HttpRetryCount; i++)
            {
                try
                {
                    HttpResponseMessage response = _client.SendAsync(requestMessage).Result;

                    if (!response.IsSuccessStatusCode)
                    {
                        Thread.Sleep(Configuration.HttpWaitMs);
                        continue;
                    }

                    string responseStr = response.Content.ReadAsStringAsync().Result;

                    return JsonConvert.DeserializeObject<TResult>(responseStr);
                }
                catch
                {
                    var sw = Stopwatch.StartNew();
                    currentWaitingTime = TimeSpan.FromMilliseconds(Configuration.HttpWaitMs) - TimeSpan.FromMilliseconds(sw.ElapsedMilliseconds);                  
                    if(currentWaitingTime.TotalMilliseconds > 0)
                        Thread.Sleep(currentWaitingTime);                 
                    continue;
                }
            }
            return default;
        }
    }
}
