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
using Portal.DTO;

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

            var index = 0;
            var consigneeInfo = from orderItem in this.db.Orders
                                where orderItem.UserProfile.UserName == User.Identity.Name
                                select new SelectListItem
                                {
                                    Value = orderItem.ConsigneeName + "," + orderItem.Province + "," + orderItem.City + "," + orderItem.Town + "," + orderItem.Address + "," + orderItem.Contact + "," + orderItem.Email,
                                    Text = orderItem.ConsigneeName + " " + orderItem.Province + orderItem.City + orderItem.Town + orderItem.Address + " " + orderItem.Contact
                                };

            ViewBag.Consignee = consigneeInfo;
            var commodityInOrder = (from c in this.db.CommodityInOrders
                                    where c.OrderId == id
                                    select c).ToList();
            List<CommodityInOrderViewModel> orderList = new List<CommodityInOrderViewModel>();
            foreach (var p in commodityInOrder)
            {
                orderList.Add(new CommodityInOrderViewModel() { OrderId = p.OrderId, CommodityId = p.CommodityId, SellerId = p.SellerId.HasValue ? p.SellerId.Value : 0, UnitPrice = p.UnitPrice.HasValue ? p.UnitPrice.Value : 0, Quantity = p.Quantity.HasValue ? p.Quantity.Value : 0, Color = p.Color, Size = p.Size, Capacity = p.Capacity, Degree = p.Commodity.Degree, CommodityName = p.Commodity.CommodityName });
            }

            var cost = this.db.Orders.SingleOrDefault(x => x.UserProfile.UserName == User.Identity.Name && x.OrderId == id).TotalCost;
            ViewBag.Cost = cost;
            ViewBag.Shipment = cost >= 50 ? 0.00 : 15;
            ViewBag.CommodityCount = orderList.Count;
            ViewBag.CommodityInOrderList = orderList;
            return this.View(order);
        }

        [HttpPost]
        public JsonResult ParseAddress(string value, string index)
        {
            ConsigneeDTO consigneeDto = new ConsigneeDTO();
            if (value != null)
            {
                string[] consignee = value.Split(',');
                consigneeDto.Index = Convert.ToInt32(index);
                consigneeDto.ConsigneeName = consignee[0];
                consigneeDto.Province = consignee[1];
                consigneeDto.City = consignee[2];
                consigneeDto.Town = consignee[3];
                consigneeDto.Address = consignee[4];
                consigneeDto.Contact = consignee[5];
                consigneeDto.Email = consignee[6];
            }

            return this.Json(consigneeDto);
        }

        [HttpPost]
        public ActionResult Home(FormCollection formCollection, Order order)
        {
            string address = formCollection["addressDetail"].ToString();
            string delivery = formCollection["delivery"].ToString();
            string selfDelivery = formCollection["selfDelivery"].ToString();
            string payFor = formCollection["pay"].ToString();
            string[] consigneeInfo = formCollection["consigneeName"].ToString().Split(',');
            if (address != null && (delivery != null || selfDelivery != null) && payFor != null && consigneeInfo != null)
            {
                order.Address = address;
                order.Payfor = payFor;
                order.CreationDate = DateTime.Now;
                if (delivery != null)
                {
                    order.Delivery = delivery;
                }

                if (selfDelivery != null)
                {
                    order.Delivery = selfDelivery;
                }

                if (consigneeInfo != null)
                {
                    order.ConsigneeName = consigneeInfo[0];
                    order.Contact = consigneeInfo[1];
                    order.Email = consigneeInfo[2];
                }

                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
            }


            return View();
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
