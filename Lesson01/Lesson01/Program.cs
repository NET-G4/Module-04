using Lesson01.Models.CoinDesk;
using Newtonsoft.Json;
using System.Text;

namespace Lesson01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            #region Cat facts

            //var client = new HttpClient();

            //var webRequest = new HttpRequestMessage(HttpMethod.Get, "https://catfact.ninja/fact")
            //{
            //    Content = new StringContent("{ 'some': 'value' }", Encoding.UTF8, "application/json")
            //};

            //var response = client.Send(webRequest);

            //using var reader = new StreamReader(response.Content.ReadAsStream());
            //string json = reader.ReadToEnd();

            //FactModel result = JsonSerializer.Deserialize<FactModel>(json);

            //Console.WriteLine(result?.fact);

            #endregion

            #region CoinDesk

            var client = new HttpClient();

            var webRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.coindesk.com/v1/bpi/currentprice.json")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = client.Send(webRequest);

            using var reader = new StreamReader(response.Content.ReadAsStream());
            string json = reader.ReadToEnd();

            CoinDesk result = JsonConvert.DeserializeObject<CoinDesk>(json);


            Console.WriteLine($"Last time Bitcoin was updated: {result.time.updatedISO}");

            #endregion
        }
    }
}