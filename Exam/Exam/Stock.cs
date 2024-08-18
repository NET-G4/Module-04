using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exam
{
    internal class Stock
    {
        public string ticker { get; set; }
        public string identifier { get; set; }
        public DateTime tradeDate { get; set; }
        public object open { get; set; }
        public object high { get; set; }
        public object low { get; set; }
        public object close { get; set; }
        public int volume { get; set; }
        public double change { get; set; }
        public double changePercent { get; set; }
    }
}
