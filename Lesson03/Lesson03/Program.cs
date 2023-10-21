using Newtonsoft.Json;
using System.Diagnostics;
using System.Text;

namespace Lesson03
{
    internal class Program
    {
        static int numberOfUniversities = 0;
        static object locker_1 = new object();

        static Stopwatch _stopwatch = new Stopwatch();
        static object locker = new object();

        static int counter = 0;
        static Semaphore semaphore = new Semaphore(1, 6);

        static void Main(string[] args)
        {
            //Console.Write("Enter country name: ");
            //string input = Console.ReadLine();
            //Console.WriteLine($"Requesting universities for {input}...");

            // new Thread(() => Increment(input)).Start();
            // new Thread(new ParameterizedThreadStart(Increment));
            // new Thread(() => Increment(input)).Start();

            // new Thread(() => IncrementWithMonitor(input)).Start();
            // new Thread(() => FetchCountries()).Start();

            //for (int i = 0; i < 5; i++)
            //{
            //    var thread = new Thread(FetchWithSemaphore);
            //    thread.Name = $"Thread {i}";
            //    thread.Start();
            //}

            //Console.ReadKey();
            //Console.Clear();

            //for (int i = 0; i < 5; i++)
            //{
            //    var thread = new Thread(FetchWithSemaphore);
            //    thread.Name = $"Thread {i}";
            //    thread.Start();
            //}

            //Console.ReadKey();
            //Console.Clear();

            for (int i = 0; i < 100; i++)
            {
                ThreadPool.QueueUserWorkItem((state) => FetchWithSemaphore());
                Console.WriteLine(ThreadPool.ThreadCount);
            }

            Console.ReadKey();

            Main(args);
        }

        static void Increment(object param)
        {
            lock (locker)
            {
                BeforeRequest();
                try
                {
                    string country = param as string;
                    HttpClient client = new HttpClient();
                    var request = new HttpRequestMessage(HttpMethod.Get, $"http://universities.hipolabs.com/search?country={country}")
                    {
                        Content = new StringContent("", Encoding.UTF8, "application/json")
                    };

                    var response = client.Send(request);
                    using var streamReader = new StreamReader(response.Content.ReadAsStream());

                    var result = JsonConvert.DeserializeObject<List<University>>(streamReader.ReadToEnd());

                    Console.WriteLine($"There are {result.Count} number of univerisites in {country}.");
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"There was request error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unknown error: {ex.Message}");
                }
                finally
                {
                    AfterRequest();
                }
            }
        }

        static void IncrementWithMonitor(object param)
        {
            try
            {
                Monitor.Enter(locker);
                BeforeRequest();

                string country = param as string;
                HttpClient client = new HttpClient();
                var request = new HttpRequestMessage(HttpMethod.Get, $"http://universities.hipolabs.com/search?country={country}")
                {
                    Content = new StringContent("", Encoding.UTF8, "application/json")
                };

                Thread.Sleep(3000);

                var response = client.Send(request);
                using var streamReader = new StreamReader(response.Content.ReadAsStream());

                var result = JsonConvert.DeserializeObject<List<University>>(streamReader.ReadToEnd());

                Console.WriteLine($"There are {result.Count} number of univerisites in {country}.");
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"There was request error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unknown error: {ex.Message}");
            }
            finally
            {
                AfterRequest();
                Monitor.Exit(locker);
            }
        }

        static void BeforeRequest()
        {
            _stopwatch.Start();
        }

        static void AfterRequest()
        {
            lock (locker_1)
            {
                _stopwatch.Stop();

                Console.ForegroundColor = ConsoleColor.DarkMagenta;
                Console.WriteLine($"Your request took {_stopwatch.ElapsedMilliseconds} ms.");
                Console.ResetColor();

                _stopwatch.Reset();
                numberOfUniversities = 0;
            }
        }

        static void FetchCountries()
        {
            lock (locker_1)
            {
                var client = new HttpClient();
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri("https://ajayakv-rest-countries-v1.p.rapidapi.com/rest/v1/all"),
                    Headers =
                {
        { "X-RapidAPI-Key", "4efdd8354bmshcd559b2bb03e316p1f0184jsn7a58ce7b97b3" },
        { "X-RapidAPI-Host", "ajayakv-rest-countries-v1.p.rapidapi.com" }
                },
                };


                lock (locker)
                {
                    Console.WriteLine(_stopwatch.ElapsedMilliseconds);
                }
            }
        }

        static void FetchWithSemaphore()
        {
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name}: {++counter}");
                Thread.Sleep(100);
            }
        }
    }

    class University
    {
        [JsonProperty("state-province")]
        public string stateprovince { get; set; }
        public string country { get; set; }
        public List<string> domains { get; set; }
        public List<string> web_pages { get; set; }
        public string alpha_two_code { get; set; }
        public string name { get; set; }
    }

    class Country
    {
        public string Name { get; set; }
    }
}