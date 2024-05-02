using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiisMenu
{
    public class Order
    {
        public int TableNumber { get; set; }
        public string MenuName { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string TimeStamp { get; set; }

    }
}
