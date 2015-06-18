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
    public class ShoppingTrolleysController : Controller
    {
        private MarketContext db = new MarketContext();

        public ActionResult Home()
        {
            ViewBag.CategoryList = GetViewBag.GetCategoryViewBag();

            var userId = GetInfo.GetUserIdByUserName(User.Identity.Name);
            var commodityInShoppingTrolley = (from commodity in this.db.CommodityInShoppingTrolleys
                                              where commodity.UserId == userId
                                              select commodity).ToList();
            List<CommodityInShoppingTrolleyViewModel> model = new List<CommodityInShoppingTrolleyViewModel>();
            foreach (var p in commodityInShoppingTrolley)
            {
                CommodityInShoppingTrolleyViewModel commodityInShoppingTrolleyViewModel = new CommodityInShoppingTrolleyViewModel();
                commodityInShoppingTrolleyViewModel.UserId = GetInfo.GetUserIdByUserName(User.Identity.Name);
                commodityInShoppingTrolleyViewModel.CommodityId = p.CommodityId;
                commodityInShoppingTrolleyViewModel.CommodityName = p.Commodity.CommodityName;
                commodityInShoppingTrolleyViewModel.Color = p.Color;
                commodityInShoppingTrolleyViewModel.Size = p.Size;
                commodityInShoppingTrolleyViewModel.Quantity = p.Quantity;
                commodityInShoppingTrolleyViewModel.Description = p.Commodity.Description;
                commodityInShoppingTrolleyViewModel.Degree = p.Commodity.Degree;
                commodityInShoppingTrolleyViewModel.Price = (p.Quantity * p.UnitPrice);
                commodityInShoppingTrolleyViewModel.UnitPrice = p.UnitPrice;
                commodityInShoppingTrolleyViewModel.Capacity = p.Capacity;
                model.Add(commodityInShoppingTrolleyViewModel);
            }

            ViewBag.CommodityInShoppingTrolleys = model;
            ViewBag.Count = model.Count;

            ViewBag.ShoppingTrolleysCount = GetViewBag.GetShoppingTrolleyViewBag(User.Identity.Name);
            return View();
        }

        [HttpPost]
        public JsonResult Home(OrderDTO orderDto)
        {
            var userId = GetInfo.GetUserIdByUserName(User.Identity.Name);
            double cost = Convert.ToDouble(orderDto.Cost.ToString());
            string [] OrderCommodityIdList = orderDto.CommodityIdList.ToString().Split(',');
            string [] notCheckedCommodityIdList;
            double postage = Convert.ToDouble(orderDto.Postage.ToString());
            if (orderDto.NotCheckedCommodityIdList != null)
            {
                notCheckedCommodityIdList = orderDto.NotCheckedCommodityIdList.ToString().Split(',');
                foreach (var notCheckedId in notCheckedCommodityIdList)
                {
                    if (string.IsNullOrWhiteSpace(notCheckedId))
                    {
                        continue;
                    }

                    var id = int.Parse(notCheckedId);
                    var commodityInShoppingTrolley = this.db.CommodityInShoppingTrolleys.SingleOrDefault(x => x.UserId == userId && x.CommodityId == id);
                    this.db.CommodityInShoppingTrolleys.Remove(commodityInShoppingTrolley);
                }
            }

            Order order = new Order() { 
                UserId = userId,
                TotalCost = cost,
                Postage = postage
            };
            this.db.Orders.Add(order);

            foreach (var id in OrderCommodityIdList)
            {
                if (string.IsNullOrWhiteSpace(id))
                {
                    continue;
                }

                var commodityId = int.Parse(id);
                var commodityInShoppingTrolley = this.db.CommodityInShoppingTrolleys.SingleOrDefault(x => x.UserId == userId && x.CommodityId == commodityId);
                ShoppingTrolleyViewModel model = new ShoppingTrolleyViewModel();
                if (commodityInShoppingTrolley != null)
                {
                    model.UnitPrice = commodityInShoppingTrolley.UnitPrice.Value;
                    model.Quantity = commodityInShoppingTrolley.Quantity.Value;
                    model.Color = commodityInShoppingTrolley.Color;
                    model.Size = commodityInShoppingTrolley.Size;
                    model.Capacity = commodityInShoppingTrolley.Capacity;
                }

                var userProfileCommodity = this.db.UserProfileCommodities.SingleOrDefault(x => x.UserProfile.UserName == User.Identity.Name && x.CommodityId == commodityId);
                if (userProfileCommodity != null)
                {
                    CommodityInOrder commodityInOrder = new CommodityInOrder()
                    {
                        OrderId = order.OrderId,
                        CommodityId = int.Parse(id),
                        UnitPrice = model.UnitPrice,
                        SellerId = userProfileCommodity.UserId,
                        Quantity = model.Quantity,
                        Color = model.Color,
                        Size = model.Size,
                        Capacity = model.Capacity
                    };
                    this.db.CommodityInOrders.Add(commodityInOrder);
                }
                else
                {
                    CommodityInOrder commodityInOrder = new CommodityInOrder()
                    {
                        OrderId = order.OrderId,
                        CommodityId = int.Parse(id),
                        UnitPrice = model.UnitPrice,
                        Quantity = model.Quantity,
                        Color = model.Color,
                        Size = model.Size,
                        Capacity = model.Capacity
                    };
                    this.db.CommodityInOrders.Add(commodityInOrder);
                }
            }

            this.db.SaveChanges();
            return this.Json(order.OrderId);
        }

        
        public JsonResult GetPostageByTotalCost(string cost)
        {
            double totalCost = Convert.ToDouble(cost);
            double postage = totalCost >= 50.00 ? 0.00 : 15.00;
            return this.Json(postage,JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetNotCheckedTotalCost(CostDTO costDto)
        {
            double a = 0;
            double b = 0;
            if (!string.IsNullOrEmpty(costDto.CurrentPrice))
            {
                a = Convert.ToDouble(costDto.CurrentPrice);
            }

            if (!string.IsNullOrEmpty(costDto.LastPrice))
            {
                b = Convert.ToDouble(costDto.LastPrice);
            }

            double Cost = b - a;
            return this.Json(Cost);
        }

        [HttpPost]
        public JsonResult GetCheckedTotalCost(CostDTO costDto)
        {
            double a = 0;
            double b = 0;
            if (!string.IsNullOrEmpty(costDto.CurrentPrice))
            {
                a = Convert.ToDouble(costDto.CurrentPrice);
            }

            if (!string.IsNullOrEmpty(costDto.LastPrice))
            {
                b = Convert.ToDouble(costDto.LastPrice);
            }

            double Cost = a + b;
            return this.Json(Cost);
        }

        public JsonResult GetAllShoppingTrolleysCost(int? id) 
        {
            double cost = 0;
            int userId = GetInfo.GetUserIdByUserName(User.Identity.Name);
            if(id == 1)
            {
                var commodityInShoppingTrolley = (from commodity in this.db.CommodityInShoppingTrolleys
                                                  from shoppingTrolley in this.db.ShoppingTrolleys
                                               where commodity.UserId == shoppingTrolley.UserId
                                               && commodity.UserId == userId
                                               select commodity).ToList();
            foreach (var p in commodityInShoppingTrolley)
            {
                cost = cost + (p.Quantity.Value * p.UnitPrice.Value);
            }
            }

            return this.Json(cost, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SingleDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = GetInfo.GetUserIdByUserName(User.Identity.Name);
            var commodityInShoppingTrolley = this.db.CommodityInShoppingTrolleys.SingleOrDefault(e => e.CommodityId == id && e.UserId == userId);
            this.db.CommodityInShoppingTrolleys.Remove(commodityInShoppingTrolley);
            var restcommodityInShoppingTrolley = this.db.CommodityInShoppingTrolleys.SingleOrDefault(e => e.CommodityId != id && e.UserId == userId);
            if (restcommodityInShoppingTrolley == null)
            {
                var shoppingTrolley = this.db.ShoppingTrolleys.SingleOrDefault(p => p.UserId == userId);
                this.db.ShoppingTrolleys.Remove(shoppingTrolley);
            }

            var json = this.db.SaveChanges();
            return this.Json(json);
        }

        // GET: ShoppingTrolleys
        public ActionResult Index()
        {
            var shoppingTrolleys = db.ShoppingTrolleys.Include(s => s.UserProfile);
            return View(shoppingTrolleys.ToList());
        }

        // GET: ShoppingTrolleys/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            ShoppingTrolley shoppingTrolley = db.ShoppingTrolleys.Find(id);
            if (shoppingTrolley == null)
            {
                return HttpNotFound();
            }

            return View(shoppingTrolley);
        }

        // GET: ShoppingTrolleys/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            return View();
        }

        // POST: ShoppingTrolleys/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,CreationDate,UpdateDate")] ShoppingTrolley shoppingTrolley)
        {
            if (ModelState.IsValid)
            {
                shoppingTrolley.CreationDate = DateTime.Now;
                db.ShoppingTrolleys.Add(shoppingTrolley);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", shoppingTrolley.UserId);
            return View(shoppingTrolley);
        }

        // GET: ShoppingTrolleys/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingTrolley shoppingTrolley = db.ShoppingTrolleys.Find(id);
            if (shoppingTrolley == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", shoppingTrolley.UserId);
            return View(shoppingTrolley);
        }

        // POST: ShoppingTrolleys/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,CreationDate,UpdateDate")] ShoppingTrolley shoppingTrolley)
        {
            if (ModelState.IsValid)
            {
                shoppingTrolley.UpdateDate = DateTime.Now;
                db.Entry(shoppingTrolley).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", shoppingTrolley.UserId);
            return View(shoppingTrolley);
        }

        // GET: ShoppingTrolleys/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ShoppingTrolley shoppingTrolley = db.ShoppingTrolleys.Find(id);
            if (shoppingTrolley == null)
            {
                return HttpNotFound();
            }
            return View(shoppingTrolley);
        }

        // POST: ShoppingTrolleys/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ShoppingTrolley shoppingTrolley = db.ShoppingTrolleys.Find(id);
            db.ShoppingTrolleys.Remove(shoppingTrolley);
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
