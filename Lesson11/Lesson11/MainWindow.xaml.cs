using Lesson11.Models;
using System.Collections.Concurrent;
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
        private ConcurrentBag<University> universities = new ConcurrentBag<University>();
        private readonly ApiService _apiService;
        private Stopwatch stopwatch = new Stopwatch();
        private CancellationToken token = new CancellationToken();

        public MainWindow()
        {
            InitializeComponent();
        }

        // Async
        private void Bitcoin_Click(object sender, RoutedEventArgs e)
        {
            BitcoinsList.ItemsSource = new List<CoinDesk>();
        }

        // Async -> continue with -> when all
        private void Search_Click(object sender, RoutedEventArgs e)
        {
            var country = CountryNameInput.Text;
            UniversitiesDataGrid.ItemsSource = new List<University>();
        }

        // new thread
        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            // universities.json


            MessageBox.Show("Data saved successfully.");
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
