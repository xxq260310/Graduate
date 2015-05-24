using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class ShoppingTrolley
    {
        public ShoppingTrolley()
        {
            this.CommodityInShoppingTrolleys = new HashSet<CommodityInShoppingTrolley>();
        }

        [Key]
        [ForeignKey("UserProfile")]
        public int UserId { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        public virtual ICollection<CommodityInShoppingTrolley> CommodityInShoppingTrolleys { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}