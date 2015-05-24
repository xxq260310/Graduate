using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class RequiredCommodity
    {
        [Key]
        public int RequiredCommodityId { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string CommodityName { get; set; }
        public Nullable<double> Price { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}