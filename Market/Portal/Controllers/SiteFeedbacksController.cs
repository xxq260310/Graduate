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
    public class SiteFeedbacksController : Controller
    {
        private MarketContext db = new MarketContext();

        public ActionResult Home()
        {
            ViewBag.ShoppingTrolleysCount = GetViewBag.GetShoppingTrolleyViewBag(User.Identity.Name);
            return View();
        }

        [HttpPost]
        public ActionResult Home(FormCollection formcollection)
        {
            var title = formcollection["Title"].ToString();
            var content = formcollection["SiteFeedback"].ToString();
            if (User.Identity.Name != string.Empty)
            {
                var id = (from userProfile in this.db.UserProfiles
                          where userProfile.UserName == User.Identity.Name
                          select userProfile.UserId).FirstOrDefault();
                SiteFeedback siteFeedback = new SiteFeedback()
                {
                    UserId = id,
                    Title = formcollection["Title"].ToString(),
                    Content = formcollection["SiteFeedback"].ToString(),
                    CreationDate = System.DateTime.Now
                };
                this.db.SiteFeedbacks.Add(siteFeedback);
                this.db.SaveChanges();
                return RedirectToAction("SingleSiteFeedback");
            }

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return this.View();
        }

        public ActionResult SingleSiteFeedback()
        {
            var item = (from siteFeedback in this.db.SiteFeedbacks
                        where siteFeedback.UserProfile.UserName == User.Identity.Name
                        select siteFeedback).ToList();
            List<SiteFeedbackViewModel> siteFeedbackViewModel = new List<SiteFeedbackViewModel>();
            foreach (var p in item)
            {
                SiteFeedbackViewModel siteFeedbackViewModelItem = new SiteFeedbackViewModel();
                siteFeedbackViewModelItem.Id = p.SiteFeedbackId;
                siteFeedbackViewModelItem.Title = p.Title;
                siteFeedbackViewModelItem.Content = p.Content;

                siteFeedbackViewModel.Add(siteFeedbackViewModelItem);
            }

            ViewBag.SiteFeedbackList = siteFeedbackViewModel;
            ViewBag.Count = siteFeedbackViewModel.Count;

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View();
        }

        public ActionResult SingleEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SiteFeedback siteFeedback = this.db.SiteFeedbacks.Find(id);
            if (siteFeedback == null)
            {
                return HttpNotFound();
            }

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View(siteFeedback);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingleEdit(SiteFeedback siteFeedback)
        {
            siteFeedback.UpdateDate = System.DateTime.Now;
            this.db.Entry(siteFeedback).State = EntityState.Modified;
            this.db.SaveChanges();
            return RedirectToAction("SingleSiteFeedback");
        }

        public ActionResult SingleDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SiteFeedback siteFeedback = db.SiteFeedbacks.Find(id);
            if (siteFeedback == null)
            {
                return HttpNotFound();
            }

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View(siteFeedback);
        }

        [HttpPost, ActionName("SingleDelete")]
        [ValidateAntiForgeryToken]
        public ActionResult SingleDeleteConfirmed(int id)
        {
            SiteFeedback siteFeedback = db.SiteFeedbacks.Find(id);
            db.SiteFeedbacks.Remove(siteFeedback);
            db.SaveChanges();
            return RedirectToAction("Home");
        }

        // GET: SiteFeedbacks
        public ActionResult Index()
        {
            var siteFeedback = db.SiteFeedbacks.Include(s => s.UserProfile);
            return View(siteFeedback.ToList());
        }

        // GET: SiteFeedbacks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SiteFeedback siteFeedback = db.SiteFeedbacks.Find(id);
            if (siteFeedback == null)
            {
                return HttpNotFound();
            }

            return View(siteFeedback);
        }

        // GET: SiteFeedbacks/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            return View();
        }

        // POST: SiteFeedbacks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SiteFeedbackId,UserId,Title,Content,CreationDate,UpdateDate")] SiteFeedback siteFeedback)
        {
            if (ModelState.IsValid)
            {
                siteFeedback.CreationDate = DateTime.Now;
                db.SiteFeedbacks.Add(siteFeedback);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(siteFeedback);
        }

        // GET: SiteFeedbacks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SiteFeedback siteFeedback = db.SiteFeedbacks.Find(id);
            if (siteFeedback == null)
            {
                return HttpNotFound();
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", siteFeedback.UserId);
            return View(siteFeedback);
        }

        // POST: SiteFeedbacks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SiteFeedbackId,UserId,Title,Content,CreationDate,UpdateDate")] SiteFeedback siteFeedback)
        {
            if (ModelState.IsValid)
            {
                siteFeedback.UpdateDate = DateTime.Now;
                db.Entry(siteFeedback).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(siteFeedback);
        }

        // GET: SiteFeedbacks/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            SiteFeedback siteFeedback = db.SiteFeedbacks.Find(id);
            if (siteFeedback == null)
            {
                return HttpNotFound();
            }

            return View(siteFeedback);
        }

        // POST: SiteFeedbacks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SiteFeedback siteFeedback = db.SiteFeedbacks.Find(id);
            db.SiteFeedbacks.Remove(siteFeedback);
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
