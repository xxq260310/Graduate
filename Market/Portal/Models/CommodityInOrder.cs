using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class CommodityInOrder
    {
        [Key]
        public int CommodityId { get; set; }
        [Key]
        public int OrderId { get; set; }
        public Nullable<int> SellerId { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Capacity { get; set; }

        public virtual Commodity Commodity { get; set; }
        public virtual Order Order { get; set; }
    }
}