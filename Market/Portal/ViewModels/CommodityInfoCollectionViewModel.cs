using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.ViewModels
{
    public class CommodityInfoCollectionViewModel
    {
        public SelectList Color { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }

        public byte[] ImageData { get; set; }

        public string ImageType { get; set; }

        public int CommodityInfoId { get; set; }

        public string Capacity { get; set; }
    }
}