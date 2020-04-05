using MobileDevApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MobileDevApp.Services
{
    class UserService
    {
        private string url { get; set; }
        
        public UserService()
        {
            url = "http://192.168.1.180:3000/api/chat-user/";
        }

        private HttpClient GetClient()
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            return client;
        }

        public async Task<UserInfo> RefisterUser(UserRegister user)
        {
            HttpClient client = GetClient();

            var response = await client.PostAsync($"{url}register/",
                new StringContent(
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<UserInfo>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
