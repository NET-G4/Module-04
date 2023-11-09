using Lesson11.Models;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows;

namespace Lesson11
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
        }

        private void Bitcoin_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {

        }

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
