using Exam.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
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
using static System.Net.WebRequestMethods;

namespace Exam
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string STOCKS_API_URL = "https://ps-async.fekberg.com/api/stocks/";
        public const string BITCOIN_API_URL = "https://api.coindesk.com/v1/bpi/currentprice.json";

        private List<Coins> list = new List<Coins>();
        private List<Kompaniya> _list = new List<Kompaniya>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
           
            //CountryNameInput
            var url = STOCKS_API_URL;

            if (string.IsNullOrEmpty(CountryNameInput.Text))
            {
                url = STOCKS_API_URL;
            }
            else
            {
                url = STOCKS_API_URL + "country=" + CountryNameInput.Text;
            }

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var request = await client.GetAsync(" ");
            var json = await request.Content.ReadAsStringAsync();
            var deJson = JsonConvert.DeserializeObject<Kompaniya>(json);

            //faylga yozish Kompanya
            await DataKompanya(deJson);

            StocksDataGrid.ItemsSource = new List<Kompaniya>();
            _list.Add(deJson);

            StocksDataGrid.ItemsSource = _list;

        }
        public async Task DataKompanya(Kompaniya data)
        {
            string path = @"C:\Users\shohb\OneDrive\Ishchi stol\modul-04\Module-04\Exam\Exam\data\data.json";
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path).Close();
            }
            using(StreamWriter  sw = new StreamWriter(path))
            {
                await sw.WriteLineAsync(data.change.ToString());
            }
        }
        private async void Bitcoin_Click(object sender, RoutedEventArgs e)
        {
            BitcoinsList.ItemsSource = new List<Coins>();
            var coinDesk = await RequestBitCoinsAsync();
           
            list.Add(coinDesk);

            BitcoinsList.ItemsSource = list;
        }
        public async Task<Coins> RequestBitCoinsAsync()
        {
            var url = BITCOIN_API_URL;
            
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            var request = await client.GetAsync("");
            var Json = await request.Content.ReadAsStringAsync();

            // faylga yozish bitcoins
            string path = @"C:\Users\shohb\OneDrive\Ishchi stol\modul-04\Module-04\Exam\Exam\data\coin.txt";
            if (!System.IO.File.Exists(path))
            {
                System.IO.File.Create(path).Close();
            }
            using (StreamWriter sw = new StreamWriter(path, true))
            {
                sw.WriteLine(Json.ToString());
            }


            var Deserializer = JsonConvert.DeserializeObject<Coins>(Json);
            return Deserializer;
        }
       

        private void CountryNameInput_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
