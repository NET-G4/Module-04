using Lesson01.Models.CoinDesk;
using Lesson01.Models.RandomUsers;
using Lesson01.Models.Unversity;
using Lesson01.Models.Zippopotam;
using Newtonsoft.Json;
using System.Text;

namespace Lesson01
{
    internal class Program
    {
       

        static void Main(string[] args)
        {

            #region RandomUsers
            //RandomUsers
            //var client = new HttpClient();

            //var webRequest = new HttpRequestMessage(HttpMethod.Get, "https://randomuser.me/api/")
            //{
            //    Content = new StringContent("", Encoding.UTF8, "application/json")
            //};

            //var response = client.Send(webRequest);

            //using var reader = new StreamReader(response.Content.ReadAsStream());
            //string json = reader.ReadToEnd();

            //RandomUsers result = JsonConvert.DeserializeObject<RandomUsers>(json);
            //foreach (var item in result.results)
            //{
            //    Console.WriteLine($"Name:{item.name.first}");
            //    Console.WriteLine($"email:{item.email}");
            //    Console.WriteLine($"Picture:"+item.picture.medium);

            //}

            #endregion

            #region CoinDeck
            //var client = new HttpClient();

            //var webRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.coindesk.com/v1/bpi/currentprice.json")
            //{
            //    Content = new StringContent("", Encoding.UTF8, "application/json")
            //};

            //var response = client.Send(webRequest);

            //using var reader = new StreamReader(response.Content.ReadAsStream());
            //string json = reader.ReadToEnd();

            //CoinDesk result = JsonConvert.DeserializeObject<CoinDesk>(json);


            ///// Uzbek sumini dollarga nisbatini olib kelamiz
            ///// 
            //var client1 = new HttpClient();

            //var webRequest1 = new HttpRequestMessage(HttpMethod.Get, "https://nbu.uz/uz/exchange-rates/json/")
            //{
            //    Content = new StringContent("", Encoding.UTF8, "application/json")
            //};

            //var response1 = client.Send(webRequest1);

            //using var reader1 = new StreamReader(response1.Content.ReadAsStream());
            //string json1 = reader1.ReadToEnd();

            //var result1 = JsonConvert.DeserializeObject <UzbNbu[]> (json1);
            //int t = 1;
            //List<decimal> dollar = new List<decimal>();
            //foreach(var valuta in result1)
            //{
            //    dollar.Add(Convert.ToDecimal(valuta.cb_price));

            //}
            //Console.WriteLine(dollar[23]);


            //Console.WriteLine($"BitcCoinni Oxirgi yangilangadan vaqti: {result.time.updatedISO}");
            //Console.WriteLine($"1 Bitcoin:{result.bpi.EUR.rate} Evro");
            //Console.WriteLine($"1 Bitcoin:{result.bpi.USD.rate} Dollar");
            //Console.WriteLine($"1 Bitcoin:{result.bpi.GBP.rate} Funt");
            //Console.WriteLine($"1 Bitcoin:{(Convert.ToDecimal(result.bpi.USD.rate) * dollar[23]):UZS .0000} So'm");


            #endregion

            #region UniversitiesList
            //var client = new HttpClient();
            //// var urlAdress = $"{}+{text}";
            //var webRequest = new HttpRequestMessage(HttpMethod.Get, "http://universities.hipolabs.com/search?country=China")
            //{
            //    Content = new StringContent("", Encoding.UTF8, "application/json")
            //};

            //var response = client.Send(webRequest);

            //using var reader = new StreamReader(response.Content.ReadAsStream());
            //string json = reader.ReadToEnd();

            //Unversity[] result = JsonConvert.DeserializeObject<Unversity[]>(json);
            //Console.WriteLine();
            ////Console.Write("Davlat nomini ingilizcha kiriting (Masalan:Uzbekistan):");
            ////string text = Console.ReadLine();
            //Console.WriteLine();
            //Console.WriteLine("1.Unversitetlar soni     2.Unversitet nomlari      3. Unversitet web sayt adreslari");
            //Console.WriteLine("                             0. chiqish                                            ");
            //int t = 0;
            //List<string> webAdress = new List<string>();
            //Console.Write("Enter:");
            //int select = int.Parse(Console.ReadLine());
            //switch (select)
            //{
            //    case 1:
            //        Console.WriteLine($"-----------------------Mamlakat:{result[0].country}-----------------------");
            //        Console.WriteLine("Unversitetlar soni:" + result.Length);
            //        break;

            //    case 2:
            //        Console.WriteLine($"-----------------------Mamlakat:{result[0].country}-----------------------");
            //        for (int i = 0; i < result.Length; i++)
            //        {
            //            Console.WriteLine($"{i + 1}:{result[i].name}");
            //        }
            //        break;
            //    // mamlakatdagi unversitetlar
            //    case 3:
            //        int j = 1;
            //        foreach (var web in result)
            //        {

            //            foreach (var i in web.web_pages)
            //            {
            //                Console.WriteLine($"{j++}:{i}");

            //            }
            //            Console.WriteLine("-------------------------");
            //        }
            //        break;

            //    default:
            //        Console.WriteLine("Xatolik bor");
            //        break;
            //}

            //Main(args);





            #endregion

            #region Zippopotam

            //var client = new HttpClient();
            //// var urlAdress = $"{}+{text}";
            //var webRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.zippopotam.us/us/33162")
            //{
            //    Content = new StringContent("", Encoding.UTF8, "application/json")
            //};

            //var response = client.Send(webRequest);

            //using var reader = new StreamReader(response.Content.ReadAsStream());
            //string json = reader.ReadToEnd();

            //Zippopotom result = JsonConvert.DeserializeObject<Zippopotom>(json);
            //foreach (var item in result.places)
            //{
            //    Console.WriteLine(item.latitude);
            //}


            #endregion
        }

    }
}