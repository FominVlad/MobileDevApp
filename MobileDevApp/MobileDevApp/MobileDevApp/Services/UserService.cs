
using MobileDevApp.RemoteProviders.Models;
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

        public async Task<Models.UserInfo> RegisterUser(UserRegister user)
        {
            HttpClient client = GetClient();

            var response = await client.PostAsync($"{url}register/",
                new StringContent(
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Models.UserInfo>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<Models.UserInfo> LoginUser(UserLogin user)
        {
            HttpClient client = GetClient();

            var response = await client.PostAsync($"{url}login/",
                new StringContent(
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Models.UserInfo>(
                await response.Content.ReadAsStringAsync());
        }

        public async Task<Models.UserInfo> EditUser(RemoteProviders.Models.UserEdit user)
        {
            HttpClient client = GetClient();

            var response = await client.PostAsync($"{url}edit/",
                new StringContent(
                    JsonConvert.SerializeObject(user),
                    Encoding.UTF8, "application/json"));

            if (response.StatusCode != HttpStatusCode.OK)
                return null;

            return JsonConvert.DeserializeObject<Models.UserInfo>(
                await response.Content.ReadAsStringAsync());
        }
    }
}
