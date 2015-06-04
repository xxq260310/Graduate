using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class CommodityInOrderViewModel
    {
        public int OrderId { get; set; }

        public int CommodityId { get; set; }

        public int SellerId { get; set; }

        public double UnitPrice { get; set; }

        public int Quantity { get; set; }

        public string Color { get; set; }

        public string Size { get; set; }

        public string Capacity { get; set; }

        public string Degree { get; set; }

        public string CommodityName { get; set; }
    }
}