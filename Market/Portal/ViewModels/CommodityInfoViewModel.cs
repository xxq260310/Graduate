using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class CommodityInfoViewModel
    {
        public int CommodityInfoId { get; set; }
        public string Color { get; set; }
        public string Size { get; set; }
        public string Capacity { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageType { get; set; }
        public int CommodityId { get; set; }
        public Nullable<int> Quantity { get; set; }
    }
}