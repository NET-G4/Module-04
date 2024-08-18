using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Exam
{
    internal class FileService
    {
        private static string StockPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
        private static string CoinPath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;

        public static async void WriteStockData(string json)
        {
            string file = StockPath + $@"\files\stocData.json";
            if (!Directory.Exists(file))
            {
                Directory.CreateDirectory(StockPath + $@"\files");
            }

            using FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            using StreamWriter writer = new StreamWriter(fs);

            await writer.WriteLineAsync(json);
            MessageBox.Show("Stock yozildi !!!");
        }
        public static async void WriteCoinData(string json)
        {
            string file = StockPath + @$"\files\CoinData.json";
            if (!Directory.Exists(file))
            {
                Directory.CreateDirectory(CoinPath + @$"files");
            }

            using FileStream fs = new FileStream(file, FileMode.OpenOrCreate);
            using StreamWriter sw = new StreamWriter(fs);

            await sw.WriteLineAsync(json);
            MessageBox.Show("Coin yozildi !!!");
        }
        
    }
}
