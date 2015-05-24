using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class ShoppingTrolleyViewModel
    {
        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Capacity { get; set; }
    }
}