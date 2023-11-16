using System;
using System.Collections.Generic;
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
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public const string STOCKS_API_URL = "https://ps-async.fekberg.com/api/stocks/";
        public const string BITCOIN_API_URL = "https://api.coindesk.com/v1/bpi/currentprice.json";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            // StocksDataGrid.ItemsSource = 
        }

        private void Bitcoin_Click(object sender, RoutedEventArgs e)
        {
            // BitcoinsList.ItemsSource = 
        }
    }
}
