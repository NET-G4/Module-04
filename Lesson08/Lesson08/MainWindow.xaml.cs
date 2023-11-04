using Lesson08.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Lesson05
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

                string[] countries = CountryNameInput.Text.Split(' ');

                var source = new CancellationTokenSource(TimeSpan.FromSeconds(50));
                List<University> loadedUniversities = new();
                List<Task<List<University>>> tasks = new List<Task<List<University>>>();

                foreach(var country in countries)
                {
                    var task = GetData(country, source);
                    tasks.Add(task);

                    //var res = await GetData(country, source);
                    //loadedUniversities.AddRange(res);

                    //UniversitiesDataGrid.ItemsSource = null;
                    //UniversitiesDataGrid.ItemsSource = loadedUniversities;
                }

                // Thread.Sleep(2000);

                var result = await Task.WhenAny(tasks);
                var r = result.Result;

                loadedUniversities.AddRange(r);

                UniversitiesDataGrid.ItemsSource = null;
                UniversitiesDataGrid.ItemsSource = loadedUniversities;
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

        private async Task WriteData(List<University> universities)
        {
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var fullPath = path + $"\\files\\data-{DateTime.Now.Day}-{DateTime.Now.Month}.json";

            Directory.CreateDirectory(path + "\\files");

            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    var json = JsonConvert.SerializeObject(universities);
                    await sw.WriteLineAsync(json);
                }
            }

            Dispatcher.Invoke(()
                => Notes.Text = $"Fetched data saved to file {System.IO.Path.GetFileName(fullPath)}");
        }

        private async Task<List<University>> GetData(string country, CancellationTokenSource token)
        {
            Thread.Sleep(3000);
            var result = await LoadUniversities(country, token);
            return result;
        }

        private void Bitcoin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    var coin = GetBitcoin();
                    bitcoins.Add(coin);

                    Dispatcher.Invoke(() =>
                    {
                        Coins.ItemsSource = null;
                        Coins.ItemsSource = bitcoins;
                    });
                });
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

        private async Task<List<University>> LoadUniversities(string country, CancellationTokenSource token)
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

        private void OpenFile_Click(object sender, RoutedEventArgs e)
        {
            var fullPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName + "\\files";

            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.InitialDirectory = fullPath;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            if (result is not true)
            {
                return;
            }

            // Open document
            string filename = dialog.FileName;

            using var reader = new StreamReader(filename);
            var data = reader.ReadToEnd();
            Dispatcher.Invoke(() => Notes.Text = data);
        }
    }

}
