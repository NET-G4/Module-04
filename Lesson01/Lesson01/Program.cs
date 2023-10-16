using Lesson01.Models.CoinDesk;
using Lesson01.Models.RandomUser;
using Lesson01.Universities_List;
using Lesson01.Zippopotam;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
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

            //var client = new HttpClient();

            //var webRequest = new HttpRequestMessage(HttpMethod.Get, "https://randomuser.me/api/")
            //{
            //    Content = new StringContent("", Encoding.UTF8, "application/json")
            //};

            //var response = client.Send(webRequest);

            //using var reader = new StreamReader(response.Content.ReadAsStream());
            //string json = reader.ReadToEnd();

            //CoinDesk result = JsonConvert.DeserializeObject<RandomUser>(json);


            //Console.WriteLine($"Last time Bitcoin was updated: {result}");

            #endregion

            #region RandomUser

            //var client = new HttpClient();
            //try
            //{
            //    var webRequest = new HttpRequestMessage(HttpMethod.Get, "https://randomuser.me/api/")
            //    {
            //        Content = new StringContent("", Encoding.UTF8, "application/json")
            //    };

            //    var response = client.Send(webRequest);

            //    using var reader = new StreamReader(response.Content.ReadAsStream());
            //    string json = reader.ReadToEnd();

            //    RandomUser result = JsonConvert.DeserializeObject<RandomUser>(json);

            //    foreach (var item in result.results)
            //    {
            //        Console.WriteLine($"Title:     {item.name.title}");
            //        Console.WriteLine($"First:     {item.name.first}");
            //        Console.WriteLine($"Last:      {item.name.last}");
            //        Console.WriteLine($"Street:    Number - {item.location.street.number}, Name - {item.location.street.name}");
            //        Console.WriteLine($"City:      {item.location.city}");
            //        Console.WriteLine($"State:     {item.location.state}");
            //        Console.WriteLine($"Country:   {item.location.country}");
            //        Console.WriteLine($"Pastcode:  {item.location.postcode}");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine(ex.Message);
            //}
            #endregion

            #region Zippopotam
            //var client = new HttpClient();
            //try
            //{
            //    var wepRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.zippopotam.us/us/33162")
            //    {
            //        Content = new StringContent("", Encoding.UTF8, "application/json")
            //    };
            //    var response = client.Send(wepRequest);
            //    using var reader = new StreamReader(response.Content.ReadAsStream());
            //    string json = reader.ReadToEnd();

            //    zippopotame result = JsonConvert.DeserializeObject<zippopotame>(json);

            //    Console.WriteLine($"Post code:              {result.postcode}");
            //    Console.WriteLine($"Country:                {result.country}");
            //    Console.WriteLine($"Country abbreviation:   {result.countryabbreviation}");

            //    foreach (var item in result.places)
            //    {
            //        Console.WriteLine($"Place name:             {item.placename}");
            //        Console.WriteLine($"Longitude:              {item.longitude}");
            //        Console.WriteLine($"State:                  {item.state}");
            //        Console.WriteLine($"State abbreviation:     {item.stateabbreviation}");
            //        Console.WriteLine($"Latitude:               {item.latitude}");

            //    }
            //}
            //catch ( Exception ex )
            //{
            //    Console.WriteLine(ex.Message );
            //}
            #endregion

            #region Universities List
            Console.Write("Enter counrty name: ");
            string country = Console.ReadLine();

            var client = new HttpClient();

            var wepRequest = new HttpRequestMessage(HttpMethod.Get, $" http://universities.hipolabs.com/search?country={country}")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var respons = client.Send(wepRequest);

            using var reader = new StreamReader(respons.Content.ReadAsStream());
            string json = reader.ReadToEnd();

            List<Root> universities=JsonConvert.DeserializeObject<List<Root>>(json);

            Console.WriteLine($"Universities in the country: {country}");

            foreach (var university in universities)
            {

                Console.WriteLine();               
                Console.WriteLine($"Name:      {university.name}");
                Console.WriteLine($"Country:   {university.country}");
                Console.WriteLine($"Web Pages: {string.Join(", ", university.web_pages)}");
                Console.WriteLine();
                Console.WriteLine("________________________________________________________");

            }
            #endregion
        }
    }
}