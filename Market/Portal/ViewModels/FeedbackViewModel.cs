using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class FeedbackViewModel
    {
        public int FeedbackId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public int CommodityId { get; set; }
        public string Content { get; set; }
        public string Reply { get; set; }
        public Nullable<System.DateTime> CreationDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
    }
}