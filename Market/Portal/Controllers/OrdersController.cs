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

        [Authorize(Roles = "Administrator")]
        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders;
            return View(orders.ToList());
        }

        public ActionResult SingleOrder(int? id)
        {
            return RedirectToAction("Home", new { orderId = id });
        }

        public ActionResult Home(int? orderId)
        {
            if (orderId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Order order = this.db.Orders.Find(orderId);
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

            var consigneeInfoList = from orderItem in this.db.Orders
                              where orderItem.UserProfile.UserName == User.Identity.Name
                              select new { 
                                  ConsigneeInfo = orderItem.ConsigneeName + " " + orderItem.Address + " " + orderItem.Contact,
                                  Email = orderItem.Email
                              };
            return this.View(order);
        }

        [Authorize(Roles="Administrator")]
        // GET: Orders/Details/5
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

        [Authorize(Roles="Administrator")]
        // GET: Orders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OrderId,BuyerId,Contact,TotalCost,Address,State,Delivery,ConsigneeName,SellerId,CreationDate,UpdateDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.CreationDate = DateTime.Now;
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Orders/Edit/5
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

            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OrderId,BuyerId,Contact,TotalCost,Address,State,Delivery,ConsigneeName,SellerId,CreationDate,UpdateDate")] Order order)
        {
            if (ModelState.IsValid)
            {
                order.UpdateDate = DateTime.Now;
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(order);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Orders/Delete/5
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
