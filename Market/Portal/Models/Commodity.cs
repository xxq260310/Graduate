using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Commodity
    {
        public Commodity()
        {
            this.Feedbacks = new HashSet<Feedback>();
            this.UserProfileCommoditys = new HashSet<UserProfileCommodity>();
            this.CommodityInShoppingTrolleys = new HashSet<CommodityInShoppingTrolley>();
            this.CommodityInFavorites = new HashSet<CommodityInFavorite>();
            this.CommodityInfos = new HashSet<CommodityInfo>();
            this.CommodityInOrders = new HashSet<CommodityInOrder>();
        }

        public int CommodityId { get; set; }
        public string CommodityName { get; set; }
        public int SubCategoryId { get; set; }
        public int ManufacturerId { get; set; }
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

        public virtual SubCategory SubCategory { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<UserProfileCommodity> UserProfileCommoditys { get; set; }
        public virtual ICollection<CommodityInShoppingTrolley> CommodityInShoppingTrolleys { get; set; }
        public virtual ICollection<CommodityInFavorite> CommodityInFavorites { get; set; }
        public virtual ICollection<CommodityInfo> CommodityInfos { get; set; }
        public virtual ICollection<CommodityInOrder> CommodityInOrders { get; set; }
    }
}