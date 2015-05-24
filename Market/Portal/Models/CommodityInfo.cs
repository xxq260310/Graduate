using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class CommodityInfo
    {
        [Key]
        public int CommodityInfoId { get; set; }
        [ForeignKey("Commodity")]
        public int CommodityId { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageType { get; set; }
        public string Capacity { get; set; }
        public Nullable<int> Quantity { get; set; }

        public virtual Commodity Commodity { get; set; }
    }
}