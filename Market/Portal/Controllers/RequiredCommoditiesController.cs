using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Portal;
using Portal.Models;
using Portal.ViewModels;

namespace Portal.Controllers
{
    [Authorize]
    [Portal.Filter.AuthorizationFilter]
    public class RequiredCommoditiesController : Controller
    {
        private MarketContext db = new MarketContext();

        public ActionResult Home()
        {
            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View();
        }

        [HttpPost]
        public ActionResult Home(FormCollection formcollection)
        {
            var content = formcollection["Content"].ToString();
            var commodityName = formcollection["CommodityName"].ToString();
            var price = formcollection["Price"].ToString();
            var unitPrice = 0;
            if (string.IsNullOrEmpty(price))
            {
                unitPrice = Convert.ToInt32(price);
            }

            if (User.Identity.Name != string.Empty)
            {
                var id = (from userProfile in this.db.UserProfiles
                          where userProfile.UserName == User.Identity.Name
                          select userProfile.UserId).FirstOrDefault();
                RequiredCommodity requiredCommodity = new RequiredCommodity() { 
                    UserId = id,
                    Content = content,
                    CreationDate = System.DateTime.Now,
                    Price = unitPrice
                };
                this.db.RequiredCommodities.Add(requiredCommodity);
                this.db.SaveChanges();
            }

            return RedirectToAction("SingleRequiredCommodity");
        }

        public ActionResult SingleRequiredCommodity()
        {
            var requiredCommodityList = (from requiredCommodity in this.db.RequiredCommodities
                                         where requiredCommodity.UserProfile.UserName == User.Identity.Name
                                         select requiredCommodity).ToList();
            List<RequiredCommodityViewModel> requiredCommodityViewModel = new List<RequiredCommodityViewModel>();
            foreach (var p in requiredCommodityList)
            {
                RequiredCommodityViewModel item = new RequiredCommodityViewModel();
                item.Content = p.Content;
                item.CommodityName = p.CommodityName;
                item.Price = p.Price.Value;
                item.Id = p.RequiredCommodityId;

                requiredCommodityViewModel.Add(item);
            }

            ViewBag.RequiredCommodityList = requiredCommodityViewModel;
            ViewBag.Count = requiredCommodityViewModel.Count;
            return View();
        }

        
        public ActionResult SingleEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RequiredCommodity requiredCommodity = this.db.RequiredCommodities.Find(id);
            if(requiredCommodity == null)
            {
                return HttpNotFound();
            }

            return View(requiredCommodity);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingleEdit(RequiredCommodity requiredCommodity)
        {
            requiredCommodity.UpdateDate = System.DateTime.Now;
            this.db.Entry(requiredCommodity).State = EntityState.Modified;
            this.db.SaveChanges();
            return RedirectToAction("SingleRequiredCommodity");
        }

        public ActionResult SingleDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RequiredCommodity requiredCommodity = db.RequiredCommodities.Find(id);
            if (requiredCommodity == null)
            {
                return HttpNotFound();
            }

            return View(requiredCommodity);
        }

        [HttpPost, ActionName("SingleDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SingleDeleteConfirmed(int id)
        {
            RequiredCommodity requiredCommodity = db.RequiredCommodities.Find(id);
            db.RequiredCommodities.Remove(requiredCommodity);
            db.SaveChanges();
            return RedirectToAction("SingleRequiredCommodity");
        }

        [Authorize(Roles="Administrator")]
        // GET: RequiredCommodities
        public ActionResult Index()
        {
            var requiredCommodities = db.RequiredCommodities.Include(r => r.UserProfile);
            return View(requiredCommodities.ToList());
        }

        [Authorize(Roles = "Administrator")]
        // GET: RequiredCommodities/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RequiredCommodity requiredCommodity = db.RequiredCommodities.Find(id);
            if (requiredCommodity == null)
            {
                return HttpNotFound();
            }

            return View(requiredCommodity);
        }

        [Authorize(Roles = "Administrator")]
        // GET: RequiredCommodities/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            return View();
        }

        // POST: RequiredCommodities/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RequiredCommodityId,UserId,Title,Content,CreationDate,UpdateDate")] RequiredCommodity requiredCommodity)
        {
            if (ModelState.IsValid)
            {
                requiredCommodity.CreationDate = DateTime.Now;
                db.RequiredCommodities.Add(requiredCommodity);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requiredCommodity);
        }

        [Authorize(Roles = "Administrator")]
        // GET: RequiredCommodities/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RequiredCommodity requiredCommodity = db.RequiredCommodities.Find(id);
            if (requiredCommodity == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", requiredCommodity.UserId);
            return View(requiredCommodity);
        }

        // POST: RequiredCommodities/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RequiredCommodityId,UserId,Title,Content,CreationDate,UpdateDate")] RequiredCommodity requiredCommodity)
        {
            if (ModelState.IsValid)
            {
                requiredCommodity.UpdateDate = DateTime.Now;
                db.Entry(requiredCommodity).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(requiredCommodity);
        }

        [Authorize(Roles = "Administrator")]
        // GET: RequiredCommodities/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            RequiredCommodity requiredCommodity = db.RequiredCommodities.Find(id);
            if (requiredCommodity == null)
            {
                return HttpNotFound();
            }

            return View(requiredCommodity);
        }

        // POST: RequiredCommodities/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RequiredCommodity requiredCommodity = db.RequiredCommodities.Find(id);
            db.RequiredCommodities.Remove(requiredCommodity);
            db.SaveChanges();
            return RedirectToAction("Index");
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
