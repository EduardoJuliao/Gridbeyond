using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GridBeyond.ConsoleClient.Models;
using Newtonsoft.Json;

namespace GridBeyond.ConsoleClient
{
    public class HttpHelper
    {
        private static readonly HttpClient Client = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5000")
        };

        public static async Task<InsertedDataModel> SendRecords(string[] marketData)
        {
            var json = JsonConvert.SerializeObject(marketData);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
            
            HttpResponseMessage response = await Client.PostAsync(
                "marketdata", stringContent);
            response.EnsureSuccessStatusCode();

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<InsertedDataModel>(responseJson);
        }

        public static async Task<ReportDataModel> GetReportData(DateTime? start = null, DateTime? end = null)
        {
            var query = "";
            if (start.HasValue)
            {
                query += "/" + start.Value.ToLongDateString();
                if(end.HasValue)
                    query += "/" + end.Value.ToLongDateString();
            }

            HttpResponseMessage response = await Client.GetAsync(
                "marketdata/report" + query);

            var responseJson = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<ReportDataModel>(responseJson);
        }
    }
}