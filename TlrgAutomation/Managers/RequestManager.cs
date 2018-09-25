using Newtonsoft.Json.Linq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace TlrgAutomation.Managers
{
    public class RequestManager
    {
        public static HttpClient client;
        public string apiUrl = Properties.RunSettings.Default.Api_Url;

        public RequestManager()
        {
            client = new HttpClient();
        }

        public string InitializeLoadByGuid(string loadGuid)
        {
            var values = new Dictionary<string, string>
            {
                {"LoadGuid", loadGuid },
                {"UserName", "" }
            };

            var content = new FormUrlEncodedContent(values);
            var response = client.PostAsync("http://" + apiUrl + ":6014/loadboard/initiate", content).Result;
            var responseString = response.Content.ReadAsStringAsync().Result;
            JObject data = JObject.Parse(responseString);
            Assert.True(response.IsSuccessStatusCode);
            return data["loadKey"].ToString();
        }
    }
}
