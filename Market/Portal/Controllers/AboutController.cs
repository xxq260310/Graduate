using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class AboutController : Controller
    {
        private MarketContext db = new MarketContext();
        // GET: About
        public ActionResult Index()
        {
            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View();
        }
    }
}