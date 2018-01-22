using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace LibraryApp.Common
{
    public class Client : HttpClient
    {
        const string baseAddress = "http://10.102.0.198:35090/api/";
        const string userName = "alex.turcan@outlook.com";
        const string password = "123456Aa!";


        public Client()
        {
            this.BaseAddress = new Uri(baseAddress);
            this.DefaultRequestHeaders.Accept.Clear();
            this.DefaultRequestHeaders.Accept.Add(MediaTypeWithQualityHeaderValue.Parse("application/json"));
            /*this.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");*/
            //GetAccessToken(userName, password);
            CurrentInstance = this;
        }

        //public void GetAccessToken()
        //{
        //    using (var client = new HttpClient())
        //    {
        //        var formContent = new FormUrlEncodedContent(new[]
        //        {
        //            new KeyValuePair<string, string>("grant_type", "password"),
        //            new KeyValuePair<string, string>("username", userName),
        //            new KeyValuePair<string, string>("password", password)
        //        });

        //        HttpResponseMessage response = client.PostAsync($"http://10.102.0.198:35090/token", formContent).Result;

        //        var responseJson = response.Content.ReadAsStringAsync().Result;
        //        var jObject = JObject.Parse(responseJson);
        //        var t = jObject.GetValue("access_token").ToString();
        //        this.DefaultRequestHeaders.Remove("Authorization");
        //        this.DefaultRequestHeaders.Add("Authorization", $"Bearer {t}");

        //    }
        //}

        public async Task<bool> GetAccessToken()
        {
            using (var client = new HttpClient())
            {
                try
                {
                    var formContent = new FormUrlEncodedContent(new[]
                    {
                        new KeyValuePair<string, string>("grant_type", "password"),
                        new KeyValuePair<string, string>("username", userName),
                        new KeyValuePair<string, string>("password", password)
                    });

                    HttpResponseMessage response = await client.PostAsync($"http://10.102.0.198:35090/token", formContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        var jObject = JObject.Parse(responseJson);
                        var t = jObject.GetValue("access_token").ToString();
                        this.DefaultRequestHeaders.Remove("Authorization");
                        this.DefaultRequestHeaders.Add("Authorization", $"Bearer {t}");
                        return true;
                    }
                    return false;

                }
                catch (Exception)
                {
                    return false;
                }

            }   
        }

        public static Client CurrentInstance { get; private set; }

        public async Task<HttpResponseMessage> Post(string uri, object model)
        {
            var jsonString = JsonConvert.SerializeObject(model);

            var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

            return await PostAsync(uri, content);
        }
    }
}
