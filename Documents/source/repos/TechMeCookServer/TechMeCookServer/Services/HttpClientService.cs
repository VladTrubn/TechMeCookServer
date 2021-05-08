using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;

namespace TechMeCookServer.Services
{
    public class HttpClientService : IHttpClientService
    {
        public String apiKey = "bee92de871cb4b05ac3b51de0cd576c2";

        private HttpClient client;
        public HttpClient GetHttpClient()
        {
            if (client == null)
            {
                client = new HttpClient();
                client.BaseAddress = new Uri("https://api.spoonacular.com/");
            }
            return client;
        }

        public String GetApiKey()
        {
            return apiKey;
        }
    }
}
