using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Favorite
    {
        public Favorite()
        {
            this.CommodityInFavorites = new HashSet<CommodityInFavorite>();
        }

        [Key]
        [ForeignKey("UserProfile")]
        public int UserId { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    
        public virtual ICollection<CommodityInFavorite> CommodityInFavorites { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}