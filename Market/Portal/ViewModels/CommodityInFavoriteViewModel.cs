using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.ViewModels
{
    public class CommodityInFavoriteViewModel
    {
        public int UserId { get; set; }

        public SelectList CommodityId { get; set; }

        public string CommodityName { get; set; }

        public double UnitPrice { get; set; }
    }
}