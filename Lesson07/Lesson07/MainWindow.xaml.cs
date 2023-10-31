using Lesson07.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lesson07
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly HttpClient _client;
        private const string UNIVERSITIES_API_URL = "http://universities.hipolabs.com/search?";
        private static string BITCOIN_API_URL = "https://api.coindesk.com/v1/bpi/currentprice.json";
        private Stopwatch stopwatch = new Stopwatch();
        private List<CoinDesk> bitcoins = new List<CoinDesk>();
        private CancellationToken token = new CancellationToken();
        private StringBuilder notesBuilder = new StringBuilder();

        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.Name = "UI Thread";

            _client = new HttpClient();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BeforeLoadingStockData();

                var country = CountryNameInput.Text;

                var source = new CancellationTokenSource(TimeSpan.FromSeconds(50));
                var result = await GetData(country, source);

                MessageBox.Show("Click finished " + Thread.CurrentThread.Name);

                UniversitiesDataGrid.ItemsSource = null;
                UniversitiesDataGrid.ItemsSource = result;

                int count = await Task.Run(() =>
                {
                    count = GetNumberOfSymbols(result);
                    WriteData(result);

                    //string name = Thread.CurrentThread.Name + Thread.CurrentThread.ManagedThreadId.ToString();
                    //MessageBox.Show(name);
                    return count;
                });

                //string name = Thread.CurrentThread.Name + Thread.CurrentThread.ManagedThreadId.ToString();
                //MessageBox.Show(name);

                notesBuilder.AppendLine($"Loaded {count} symbols for {country}.");
                Notes.Text = notesBuilder.ToString();
            }
            catch (Exception ex)
            {
                Notes.Text = ex.Message;
            }
            finally
            {
                AfterLoadingStockData();
            }
        }

        private async Task WriteDataAsync(List<University> universities)
        {
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var fullPath = path + $"\\files\\data-{DateTime.Now}.json";

            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    var json = JsonConvert.SerializeObject(universities);
                    await sw.WriteAsync(json);
                }
            }
        }

        private void WriteData(List<University> universities)
        {
            Thread.Sleep(2000);
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

            Directory.CreateDirectory(path + "\\files");
            var fullPath = path + $"\\files\\data-{DateTime.Now.Day}.json";

            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    var json = JsonConvert.SerializeObject(universities);
                    sw.Write(json);
                }
            }
        }

        private int GetNumberOfSymbols(List<University> universities)
        {
            int sum = 0;
            foreach (var university in universities)
            {
                if (!string.IsNullOrEmpty(university.name))
                {
                    sum += university.name.Length;
                }

                if (!string.IsNullOrEmpty(university.country))
                {
                    sum += university.country.Length;
                }

                if (!string.IsNullOrEmpty(university.stateprovince))
                {
                    sum += university.stateprovince.Length;
                }
            }

            return sum;
        }

        private async Task<List<University>> GetData(string country, CancellationTokenSource token)
        {
            // Fire and forget
            ThreadPool.QueueUserWorkItem(state => CalculateData());
            Task.Run(CalculateData).ConfigureAwait(false);
            MessageBox.Show("Get Data " + Thread.CurrentThread.Name);
            var result = await LoadUniversities(country, token);

            MessageBox.Show("Get Data Finished " + Thread.CurrentThread.Name);
            return result;
        }

        private void CalculateData()
        {
            MessageBox.Show("Calculate " + Thread.CurrentThread.Name);
            long result = 1;
            for (int i = 1; i < 1000_000_000; i++)
            {
                result *= i;
            }

            MessageBox.Show(Thread.CurrentThread.Name + " has finished calculate");
            // MessageBox.Show(result.ToString());
        }

        private void Bitcoin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    var coin = GetBitcoin();
                    bitcoins.Add(coin);
                    notesBuilder.AppendLine($"Bitcoin updated! {DateTime.Now}");

                    Dispatcher.Invoke(() =>
                    {
                        Coins.ItemsSource = null;
                        Coins.ItemsSource = bitcoins;
                        Notes.Text = notesBuilder.ToString();
                    });
                });
            }
            catch (Exception ex)
            {
                Notes.Text = ex.Message;
            }
            finally
            {
                // AfterLoadingStockData();
            }
        }

        private async Task<List<University>> LoadUniversities(string country, CancellationTokenSource token)
        {
            MessageBox.Show("Load universities " + Thread.CurrentThread.Name);
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

            //var thread = new Thread(async () =>
            //{
            //    var response = await Task.Run(() => _client.Send(request, token.Token));
            //    var streamReader = new StreamReader(response.Content.ReadAsStream());
            //});

            //thread.Priority = ThreadPriority.Highest;

            //var response = await Task.Run(() => _client.Send(request, token.Token));
            //var streamReader = new StreamReader(response.Content.ReadAsStream());

            var response = await _client.SendAsync(request, token.Token);
            var streamReader = new StreamReader(response.Content.ReadAsStream());

            var result = JsonConvert.DeserializeObject<List<University>>(streamReader.ReadToEnd());

            return result;
        }

        private CoinDesk GetBitcoin()
        {
            Thread.Sleep(1500);
            var url = BITCOIN_API_URL;
            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = _client.Send(request);
            var streamReader = new StreamReader(response.Content.ReadAsStream());

            var result = JsonConvert.DeserializeObject<CoinDesk>(streamReader.ReadToEnd());

            return result;
        }

        #region Multithreading with ThreadPool

        //private void Search_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        BeforeLoadingStockData();

        //        var country = CountryNameInput.Text;

        //        ThreadPool.QueueUserWorkItem((state) =>
        //        {
        //            var result = LoadUniversities(country);

        //            Dispatcher.Invoke(() =>
        //            {
        //                UniversitiesDataGrid.ItemsSource = null;
        //                UniversitiesDataGrid.ItemsSource = result;
        //            });
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        Notes.Text = ex.Message;
        //    }
        //    finally
        //    {
        //        AfterLoadingStockData();
        //    }
        //}

        //private void Bitcoin_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        ThreadPool.QueueUserWorkItem((state) =>
        //        {
        //            var coin = GetBitcoin();
        //            bitcoins.Add(coin);

        //            Dispatcher.Invoke(() =>
        //            {
        //                Coins.ItemsSource = null;
        //                Coins.ItemsSource = bitcoins;
        //            });
        //        });
        //    }
        //    catch(Exception ex)
        //    {
        //        Notes.Text = ex.Message;
        //    }
        //    finally
        //    {
        //        AfterLoadingStockData();
        //    }
        //}

        //private List<University> LoadUniversities(string country)
        //{
        //    Thread.Sleep(2500);
        //    string url = string.Empty;

        //    if (string.IsNullOrEmpty(country))
        //    {
        //        url = UNIVERSITIES_API_URL;
        //    }
        //    else
        //    {
        //        url = UNIVERSITIES_API_URL + "country=" + country;
        //    }

        //    var request = new HttpRequestMessage(HttpMethod.Get, url)
        //    {
        //        Content = new StringContent("", Encoding.UTF8, "application/json")
        //    };

        //    var response = _client.Send(request);
        //    var streamReader = new StreamReader(response.Content.ReadAsStream());

        //    var result = JsonConvert.DeserializeObject<List<University>>(streamReader.ReadToEnd());

        //    return result;
        //}

        //private CoinDesk GetBitcoin()
        //{
        //    Thread.Sleep(1500);
        //    var url = BITCOIN_API_URL;
        //    var request = new HttpRequestMessage(HttpMethod.Get, url)
        //    {
        //        Content = new StringContent("", Encoding.UTF8, "application/json")
        //    };

        //    var response = _client.Send(request);
        //    var streamReader = new StreamReader(response.Content.ReadAsStream());

        //    var result = JsonConvert.DeserializeObject<CoinDesk>(streamReader.ReadToEnd());

        //    return result;
        //}

        #endregion

        private void BeforeLoadingStockData()
        {
            stopwatch.Restart();
            StockProgress.Visibility = Visibility.Visible;
            StockProgress.IsIndeterminate = true;
        }

        private void AfterLoadingStockData()
        {
            StocksStatus.Text = $"Loaded universities in {stopwatch.ElapsedMilliseconds}ms";
            StockProgress.Visibility = Visibility.Hidden;
        }

        private void Close_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
