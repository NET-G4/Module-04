using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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

namespace Exam
{
    public partial class MainWindow : Window
    {
        Stopwatch stopwatch = new Stopwatch();
        private ApiService _api;
        private List<Coin> coins;
        public MainWindow()
        {
            InitializeComponent();
            _api = new ApiService();
            coins = new List<Coin>();
        }

        private async void Search_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BeforeLoad();
    
                string[]stocksName = CountryNameInput.Text.Split(' ');
                List<List<Stock>> tasks = new List<List<Stock>>();
                List<Stock> stoc = new List<Stock>();

                foreach (string stockName in stocksName)
                {
                    stockName.ToLower();

                    var task = await _api.GetStocksAsync(stockName);
                    tasks.Add(task);
                }

                foreach(var res in tasks)
                {
                    if(res != null)
                    {
                        foreach(var st in res)
                        {
                            stoc.Add(st);
                        }
                    }
                }

                string json = JsonConvert.SerializeObject(stoc);
                FileService.WriteStockData(json);
                
                StocksDataGrid.ItemsSource = stoc;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally 
            {
                AfterLoad();
            }
   
        }

        private async void Bitcoin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BeforeLoad();
                var coin = await _api.GetCoinAsync();
                coins.Add(coin);

                string json = JsonConvert.SerializeObject(coins);
                FileService.WriteCoinData(json);

                BitcoinsList.ItemsSource = coins;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                AfterLoad();
            }            
        }
        public void BeforeLoad()
        {
            stopwatch.Restart();
            StockProgress.Visibility = Visibility.Visible;
            StockProgress.IsIndeterminate = true;
        }
        public void AfterLoad()
        {
            StocksStatus.Text = $"Loaded stocks in {stopwatch.ElapsedMilliseconds}ms";
            StockProgress.Visibility = Visibility.Hidden;
        }
    }
}
