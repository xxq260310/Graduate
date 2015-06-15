using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Portal.ViewModels
{
    public class RequiredCommodityViewModel
    {
        public int Id { get; set; }
        public string CommodityName { get; set; }

        public string Content { get; set; }

        public string Price { get; set; }
    }
}