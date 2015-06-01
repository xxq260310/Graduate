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
    public class OrdersController : Controller
    {
        private MarketContext db = new MarketContext();

        public ActionResult Home(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = this.db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }

            var parentCategory = (from p in this.db.ParentCategories
                                  select p).ToList();
            List<CategoryGroup> categoryGroupList = new List<CategoryGroup>();
            foreach (var p in parentCategory)
            {
                CategoryGroup categoryGroup = new CategoryGroup();
                var categories = (from item in this.db.SubCategories
                                  where item.ParentCategoryId == p.ParentCategoryId
                                  select item.CategoryName).ToList();
                categoryGroup.ParentCategory = p.CategoryName;
                categoryGroup.SubCategory = categories;

                categoryGroupList.Add(categoryGroup);
            }

            ViewBag.CategoryList = categoryGroupList;

            var commodityInShoppingTrolleyList = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                                  from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                                  where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                                  && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                                  select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolleyList.Count;

            var consigneeInfoList = (from orderItem in this.db.Orders
                                    where orderItem.UserProfile.UserName == User.Identity.Name
                                    select new ConsigneeInfoViewModel
                                    {
                                        AddressDetail = orderItem.ConsigneeName + " " + orderItem.Address + " " + orderItem.Contact
                                    }).ToList();
            List<ConsigneeInfoViewModel> list = new List<ConsigneeInfoViewModel>();
            list.AddRange(consigneeInfoList);
            ViewBag.ConsigneeInfoList = list;


            return this.View();
        }

        public ActionResult SingleOrder()
        {
            return View();
        }

        // GET: Orders
        [Authorize(Roles = "Administrator")]
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.UserProfile);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,UserId,Contact,TotalCost,Address,State,Delivery,ConsigneeName,CreationDate,UpdateDate,Email")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", order.UserId);
            return View(order);
        }

        // GET: Orders/Edit/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", order.UserId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,UserId,Contact,TotalCost,Address,State,Delivery,ConsigneeName,CreationDate,UpdateDate,Email")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", order.UserId);
            return View(order);
        }

        // GET: Orders/Delete/5
        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
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
