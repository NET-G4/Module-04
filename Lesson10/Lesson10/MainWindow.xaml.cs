using Lesson10.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lesson10
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Stopwatch stopwatch = new Stopwatch();
        private List<CoinDesk> bitcoins = new List<CoinDesk>();
        private CancellationToken token = new CancellationToken();
        private List<Student> students = new();
        private List<Employee> employees = new();
        private List<Person> people = new();

        public MainWindow()
        {
            InitializeComponent();
            Thread.CurrentThread.Name = "UI Thread";

            _client = new HttpClient();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
        }

        private async Task WriteData(List<University> universities)
        {
            throw new Exception();
            var path = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            var fullPath = path + $"\\files\\data-{DateTime.Now.Day}-{DateTime.Now.Month}.json";

            Directory.CreateDirectory(path + "\\files2");

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
            throw new Exception();
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

    interface IRunnable
    {
        void Run();
    }

    class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual void DisplayInfo()
        {
            MessageBox.Show($"{FirstName} {LastName}");
        }
    }

    class Student : Person
    {
        public int Grade { get; set; }

        public override void DisplayInfo()
        {
            MessageBox.Show($"{FirstName} {LastName}, {Grade}");
        }
    }

    class Employee : Person
    {
        public decimal Salary { get; set; }
        public override void DisplayInfo()
        {
            MessageBox.Show($"{FirstName} {LastName}, {Salary}");
        }
    }
}
