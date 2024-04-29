﻿using System;
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
        public string Date{ get; set; }
        public string Time { get; set; }
        public string ImageUrl { get; set; }
    }
}
