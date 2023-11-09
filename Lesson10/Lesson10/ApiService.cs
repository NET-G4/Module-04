using Lesson10.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lesson10
{
    internal class ApiService
    {
        private readonly HttpClient _client;
        private const string UNIVERSITIES_API_URL = "http://universities.hipolabs.com/search?";
        private static string BITCOIN_API_URL = "https://api.coindesk.com/v1/bpi/currentprice.json";

        public ApiService()
        {
            _client = new HttpClient();
        }

        public async Task<List<University>> GetUniverisitesByCountryAsync(string country)
        {
            string url;

            if (string.IsNullOrEmpty(country))
            {
                url = UNIVERSITIES_API_URL;
            }
            else
            {
                url = UNIVERSITIES_API_URL + "country=" + country;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            var streamReader = new StreamReader(response.Content.ReadAsStream());

            var result = JsonConvert.DeserializeObject<List<University>>(streamReader.ReadToEnd());

            return result ?? new List<University>();
        }

        public List<University> GetUniversitiesByCountry(string country)
        {
            string url;

            if (string.IsNullOrEmpty(country))
            {
                url = UNIVERSITIES_API_URL;
            }
            else
            {
                url = UNIVERSITIES_API_URL + "country=" + country;
            }

            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = _client.Send(request);
            var streamReader = new StreamReader(response.Content.ReadAsStream());

            var result = JsonConvert.DeserializeObject<List<University>>(streamReader.ReadToEnd());

            return result ?? new List<University>();
        }

        public async Task<CoinDesk> GetBitcoinAsync()
        {
            var url = BITCOIN_API_URL;
            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await _client.SendAsync(request).ConfigureAwait(false);
            var streamReader = new StreamReader(response.Content.ReadAsStream());

            var result = JsonConvert.DeserializeObject<CoinDesk>(streamReader.ReadToEnd());

            return result;
        }

        public CoinDesk GetBitcoin()
        {
            var url = BITCOIN_API_URL;
            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = _client.SendAsync(request);
            var streamReader = new StreamReader(response.Content.ReadAsStream());

            var result = JsonConvert.DeserializeObject<CoinDesk>(streamReader.ReadToEnd());

            return result;
        }
    }
}
