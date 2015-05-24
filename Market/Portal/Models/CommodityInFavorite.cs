using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class CommodityInFavorite
    {
        [Key]
        public int UserId { get; set; }
        [Key]
        public int CommodityId { get; set; }
    
        public virtual Commodity Commodity { get; set; }
        public virtual Favorite Favorite { get; set; }
    }
}