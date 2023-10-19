using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson01.Models.CoinDesk
{
    internal class UzbNbu
    {
        public string title { get; set; }
        public string code { get; set; }
        public string cb_price { get; set; }
        public string nbu_buy_price { get; set; }
        public string nbu_cell_price { get; set; }
        public string date { get; set; }
    }
    // Root myDeserializedClass = JsonConvert.DeserializeObject<List<Root>>(myJsonResponse);


}
