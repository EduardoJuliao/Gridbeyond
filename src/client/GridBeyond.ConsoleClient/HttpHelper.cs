using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GridBeyond.ConsoleClient
{
    public class HttpHelper
    {
        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        public static async Task<HttpResponseMessage> SendRecords(string[] marketData)
        {
            var json = JsonConvert.SerializeObject(marketData);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            
            HttpResponseMessage response = await Client.PostAsync(
                "marketdata", stringContent);
            response.EnsureSuccessStatusCode();

            // return URI of the created resource.
            return response;
        }
    }
}