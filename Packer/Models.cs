﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.mobiquity.packer.Models
{

    public class PackageItem
    {
        public int Index { get; set; }
        public decimal Weight { get; set; }
        public decimal Cost { get; set; }

        public PackageItem(int index, decimal weight, decimal cost)
        {
            Index = index;
            Weight = weight;
            Cost = cost;
        }
    }

}
