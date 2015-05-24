using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class CommodityInShoppingTrolleyViewModel
    {
        public int UserId { get; set; }
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public string Description { get; set; }
        public string Degree { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public Nullable<double> Price { get; set; }
        public string Capacity { get; set; }
    }
}