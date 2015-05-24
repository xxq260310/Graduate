using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.ViewModels
{
    public class CommodityViewModel
    {
        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public string SubCategoryName { get; set; }
        public string ManufacturerName { get; set; }
        public Nullable<double> UnitPrice { get; set; }
        public Nullable<double> SubPrice { get; set; }
        public Nullable<double> DiscountPrice { get; set; }
        public string MakeCompany { get; set; }
        public string Degree { get; set; }
        public Nullable<int> Quantity { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public Nullable<bool> IsRecommended { get; set; }
        public Nullable<System.DateTime> ManufacturerDate { get; set; }
        public Nullable<System.DateTime> PromotionStartOn { get; set; }
        public Nullable<System.DateTime> PromotionEndOn { get; set; }
        public string Weight { get; set; }
        public string Component { get; set; }
        public byte[] ImageData { get; set; }
        public string ImageType { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public IEnumerable<CommodityInfoViewModel> CommodityInfos { get; set; }
    }
}