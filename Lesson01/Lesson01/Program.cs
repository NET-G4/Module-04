using Lesson01.Models.CoinDesk;
using Newtonsoft.Json;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace Lesson01
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ShowMenu();
            Main(args);      
        }
        #region BitCoin
        static void ThreadMakerBitcoin()
        {
            Thread thread1 = new Thread(DownloadBitcoinData);
            thread1.Start();
        }
        static void DownloadBitcoinData()
        {
            var coinDesk = HttpRequest<CoinDesk>("https://api.coindesk.com/v1/bpi/currentprice.json");

            FileManager<CoinDesk>.Save(coinDesk);
        }
        #endregion
        #region University
        static void ThreadMakerUniversities()
        {
            Thread thread2 = new Thread(DownloadUniversitiesData);
            thread2.Start();
        }
        static void DownloadUniversitiesData()
        {
            List<University> universities = HttpRequest<List<University>>("http://universities.hipolabs.com/search?country=United+States");

            FileManager<List<University>>.Save(universities);
        }
        #endregion
        #region Joke
        static void ThreadMakerJoke()
        {
            Thread thread3 = new Thread(DownloadJokeData);
            thread3.Start();
        }
        static void DownloadJokeData()
        {
            Joke joke = HttpRequest<Joke>("https://official-joke-api.appspot.com/random_joke");

            FileManager<Joke>.Save(joke);
        }
        #endregion 
      
        static void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine("1.Get old informations BitCoin");
            Console.WriteLine("2.Get old information Universities in USA");
            Console.WriteLine("3.Get old any jokes");
            Console.WriteLine("4.Update informations Bitcoin");
            Console.WriteLine("5.Update information Universities in USA");
            Console.WriteLine("6.Update any jokes");

            Menu();
        }
        static int ChooseMenu()
        {
            Console.Write("Choose the menu : ");

            int.TryParse(Console.ReadLine(), out int choose);
            Console.Clear();
            return choose;
        }
        static void Menu()
        {
            switch (ChooseMenu())
            {
                case 4: ThreadMakerBitcoin();break;
                case 5: ThreadMakerUniversities();break;;
                case 6: ThreadMakerJoke();break;
                case 1: Bitcoin();break;
                case 2: Universities();break;
                case 3: Jokes();break;
                default: break;
            }
        }
        static void Jokes()
        {
            Console.Clear();
            try
            {
                var joke = FileManager<Joke>.Load();
                Console.WriteLine($" {joke.punchline}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Fayl bo'sh {ex.Message}");
            }
            Console.ReadKey();
        }
        static void Universities()
        {
            try
            {

                var universities = FileManager<List<University>>.Load();

                Console.WriteLine("Universities in USA : ");

                for (int i = 0; i<universities.Count; i++)
                {
                    Console.WriteLine((i+1) + universities[i].name);
                }
                Console.WriteLine("Choose the University ID number");

                int.TryParse(Console.ReadLine(), out int choose);

                GetInformationUniverity(universities[--choose]);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Nimadir xato" + ex.Message);
            }

        }
        static void GetInformationUniverity(University university)
        {
            Console.WriteLine($"Name : {university.name}");
            Console.WriteLine($"Name : {university.country}");
            Console.WriteLine($"Name : {university.alpha_two_code}");
            Console.WriteLine($"Name : {university.stateprovince}");

            Console.WriteLine("Web pages : ");
            foreach(var i in university.web_pages)
            {
                Console.WriteLine(i);
            }
            
            Console.WriteLine("Domains : ");
            foreach (var i in university.domains)
            {
                Console.WriteLine(i);
            }
            Console.ReadKey();
        }
        static void Bitcoin()
        {
            var coinDesk = FileManager<CoinDesk>.Load();

            Console.WriteLine("1.Bitcoin in USD");
            Console.WriteLine("2.Bitcoin in EUR");
            Console.WriteLine("3.Bitcoin in GBP");

            switch (ChooseMenu())
            {
                case 1: BitcoinUSD(coinDesk.bpi); break;
                case 2: BitcoinEUR(coinDesk.bpi); break;
                case 3: BitcoinGBP(coinDesk.bpi); break;
                default:return;                  
            }
            Console.ReadKey();
        }
        static void BitcoinEUR(Bpi bpi)
        {
            Console.WriteLine($"Code : {bpi.EUR.code}");
            Console.WriteLine($"Rate : {bpi.EUR.rate}");
            Console.WriteLine($"Description {bpi.EUR.description}");
            Console.WriteLine($"Symbol : {bpi.EUR.symbol}");
            Console.WriteLine($"Rate float : {bpi.EUR.rate_float}");
        }
        static void BitcoinUSD(Bpi bpi)
        {
            Console.WriteLine($"Code : {bpi.USD.code}");
            Console.WriteLine($"Rate : {bpi.USD.rate}");
            Console.WriteLine($"Description {bpi.USD.description}");
            Console.WriteLine($"Symbol : {bpi.USD.symbol}");
            Console.WriteLine($"Rate float : {bpi.USD.rate_float}");
        }
        static void BitcoinGBP(Bpi bpi)
        {
            Console.WriteLine($"Code : {bpi.GBP.code}");
            Console.WriteLine($"Rate : {bpi.GBP.rate}");
            Console.WriteLine($"Description {bpi.GBP.description}");
            Console.WriteLine($"Symbol : {bpi.GBP.symbol}");
            Console.WriteLine($"Rate float : {bpi.GBP.rate_float}");
        }
        static T HttpRequest<T>(string webPath)
        {
            var client = new HttpClient();
            var webrequest = new HttpRequestMessage(HttpMethod.Get, webPath)
            {
                Content  = new StringContent("", Encoding.UTF8, "application/json")
            };
            var response = client.Send(webrequest);


            using var reader = new StreamReader(response.Content.ReadAsStream());
            string json = reader.ReadToEnd();

            T result = JsonConvert.DeserializeObject<T>(json);
            return result;

        }
    }
}