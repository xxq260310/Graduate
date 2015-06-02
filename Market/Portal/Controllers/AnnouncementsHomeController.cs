using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Portal.Models;

namespace Portal.Controllers
{
    public class AnnouncementsHomeController : Controller
    {
        private MarketContext db = new MarketContext();

        // GET: AnnouncementsHome
        public ActionResult Home(string sortOrder, string currentFilter, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CreationDate = string.IsNullOrEmpty(sortOrder) ? "CreationDate_desc" : "";
            var announcement = from announcementItem in this.db.Announcements
                               select announcementItem;

            switch (sortOrder)
            {
                case "CreationDate_desc":
                    announcement = announcement.OrderByDescending(s => s.CreationDate);
                    break;
                default:
                    announcement = announcement.OrderBy(s => s.CreationDate);
                    break;
            }

            int pageSize = 3;
            int pageNumber = page ?? 1;

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return this.View(announcement.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AnnouncementDetails(string Title)
        {
            Announcement model = new Announcement();
            if (!string.IsNullOrEmpty(Title))
            {
                var announcement = (from announcements in this.db.Announcements
                                    where announcements.Title == Title
                                    select announcements).FirstOrDefault();
                
                model.Title = announcement.Title;
                model.Content = announcement.Content;
                model.CreationDate = announcement.CreationDate;
                model.UpdateDate = announcement.UpdateDate;
                
            }

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}