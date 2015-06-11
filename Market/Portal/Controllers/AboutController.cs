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
            ViewBag.ShoppingTrolleysCount = GetViewBag.GetShoppingTrolleyViewBag(User.Identity.Name);
            return View();
        }
    }
}