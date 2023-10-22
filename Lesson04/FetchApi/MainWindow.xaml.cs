using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FetchApi
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        static List<CoinDesk> coinDesks = new List<CoinDesk>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string country = inputTextBox.Text;

            try
            {
                Loader.Text = "Loading...";

                var list = await FetchUniversities(country).ConfigureAwait(false);

                Thread.Sleep(2000);

                Dispatcher.Invoke(() =>
                {
                    dataGrid.ItemsSource = list;
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error! {ex.Message}");
            }
            finally
            {
                Dispatcher.Invoke(() =>
                {
                    Loader.Text = "Finished loading";
                });
            }
        }

        private async Task<List<University>> FetchUniversities(string country)
        {
            HttpClient client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, $"http://universities.hipolabs.com/search?country={country}")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(request);
            using var streamReader = new StreamReader(response.Content.ReadAsStream());

            var result = JsonConvert.DeserializeObject<List<University>>(streamReader.ReadToEnd());

            return result;
        }

        private async Task<CoinDesk> FetchCoinAsync()
        {
            var client = new HttpClient();

            var webRequest = new HttpRequestMessage(HttpMethod.Get, "https://api.coindesk.com/v1/bpi/currentprice.json")
            {
                Content = new StringContent("", Encoding.UTF8, "application/json")
            };

            var response = await client.SendAsync(webRequest);

            using var reader = new StreamReader(response.Content.ReadAsStream());
            string json = reader.ReadToEnd();

            CoinDesk result = JsonConvert.DeserializeObject<CoinDesk>(json);

            return result;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                Loader1.Text = "Loading...";

                var result = await FetchCoinAsync();
                coinDesks.Add(result);

                dataGrid1.ItemsSource = null;
                dataGrid1.ItemsSource = coinDesks;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error! {ex.Message}");
            }
            finally
            {
                Loader1.Text = "Finished loading";
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

    internal class CoinDesk
    {
        public Time time { get; set; }
        public string disclaimer { get; set; }
        public string chartName { get; set; }
        public Bpi bpi { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Bpi
    {
        public USD USD { get; set; }
        public GBP GBP { get; set; }
        public EUR EUR { get; set; }
    }

    public class EUR
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public string rate { get; set; }
        public string description { get; set; }
        public double rate_float { get; set; }
    }

    public class GBP
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public string rate { get; set; }
        public string description { get; set; }
        public double rate_float { get; set; }
    }

    public class Time
    {
        public string updated { get; set; }
        public DateTime updatedISO { get; set; }
        public string updateduk { get; set; }
    }

    public class USD
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public string rate { get; set; }
        public string description { get; set; }
        public double rate_float { get; set; }
    }
}
