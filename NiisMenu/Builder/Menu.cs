using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NiisMenu.Builder
{
    public class Menu
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public string Category { get; set; }
        public bool IsAvailable { get; set; }
        public string TimeStamp { get; set; }
    }
}
