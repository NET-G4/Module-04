using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Exam
{
    internal class ApiService
    {
        public const string STOCKS_API_URL = "https://ps-async.fekberg.com/api/stocks/";
        public const string BITCOIN_API_URL = "https://api.coindesk.com/v1/bpi/currentprice.json";
        private HttpClient _client;

        public ApiService()
        {
            _client = new HttpClient();
        }

        public async Task<List<Stock>> GetStocksAsync(string StockName)
        {
            string url;
            if (string.IsNullOrEmpty(StockName))
            {
                url = STOCKS_API_URL;
            }
            url = STOCKS_API_URL + StockName;

            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = new StringContent("", Encoding.UTF8, "application/son")
            };
            var response = await _client.SendAsync(request);
            var streamReader = new StreamReader(response.Content.ReadAsStream());
            var result = JsonConvert.DeserializeObject<List<Stock>>(streamReader.ReadToEnd());

            return result;
        }
        public async Task<Coin> GetCoinAsync()
        {
            string url = BITCOIN_API_URL;
            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response =await _client.SendAsync(request);
            var streamReader = new StreamReader(response.Content.ReadAsStream());
            var result = JsonConvert.DeserializeObject<Coin>(streamReader.ReadToEnd());

            return result;
        }
    }
}
