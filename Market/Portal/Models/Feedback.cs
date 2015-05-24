using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Portal.Models
{
    public class Feedback
    {
        [Key]
        public int FeedbackId { get; set; }
        public int UserId { get; set; }
        public int CommodityId { get; set; }
        public string Content { get; set; }
        public string Reply { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }

        public virtual Commodity Commodity { get; set; }
        public virtual UserProfile UserProfile { get; set; }
    }
}