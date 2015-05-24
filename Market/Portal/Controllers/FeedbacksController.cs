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

namespace Portal.Controllers
{
    [Authorize]
    [Portal.Filter.AuthorizationFilter]
    public class FeedbacksController : Controller
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

        // GET: Feedbacks
        public ActionResult Index()
        {
            var feedback = db.Feedbacks.Include(f => f.Commodity).Include(f => f.UserProfile);
            return View(feedback.ToList());
        }

        // GET: Feedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // GET: Feedbacks/Create
        public ActionResult Create()
        {
            ViewBag.CommodityId = new SelectList(db.Commodities, "CommodityId", "CommodityName");
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            return View();
        }

        // POST: Feedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.CreationDate = DateTime.Now;
                db.Feedbacks.Add(feedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CommodityId = new SelectList(db.Commodities, "CommodityId", "CommodityName", feedback.CommodityId);
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", feedback.UserId);
            return View(feedback);
        }

        // GET: Feedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            ViewBag.CommodityId = new SelectList(db.Commodities, "CommodityId", "CommodityName", feedback.CommodityId);
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", feedback.UserId);
            return View(feedback);
        }

        // POST: Feedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                feedback.UpdateDate = DateTime.Now;
                db.Entry(feedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CommodityId = new SelectList(db.Commodities, "CommodityId", "CommodityName", feedback.CommodityId);
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", feedback.UserId);
            return View(feedback);
        }

        // GET: Feedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Feedback feedback = db.Feedbacks.Find(id);
            if (feedback == null)
            {
                return HttpNotFound();
            }
            return View(feedback);
        }

        // POST: Feedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Feedback feedback = db.Feedbacks.Find(id);
            db.Feedbacks.Remove(feedback);
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
