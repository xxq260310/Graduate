using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }

        public string ConsigneeName { get; set; }

        public string Payfor { get; set; }

        public DateTime CreationDate { get; set; }

        public string State { get; set; }

        public double Cost { get; set; }

        public IEnumerable<CommodityInOrderViewModel> CommodityInfoes { get; set; }
    }
}