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

            ViewBag.CategoryList = GetViewBag.GetCategoryViewBag();

            ViewBag.ShoppingTrolleysCount = GetViewBag.GetShoppingTrolleyViewBag(User.Identity.Name);

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
                orderList.Add(new CommodityInOrderViewModel() { OrderId = p.OrderId, CommodityId = p.CommodityId, SellerName = p.Commodity.UserProfileCommoditys.Select(x => x.UserProfile.UserName).FirstOrDefault(), UnitPrice = p.UnitPrice.HasValue ? p.UnitPrice.Value : 0, Quantity = p.Quantity.HasValue ? p.Quantity.Value : 0, Color = p.Color, Size = p.Size, Capacity = p.Capacity, Degree = p.Commodity.Degree, CommodityName = p.Commodity.CommodityName });
            }

            var cost = this.db.Orders.SingleOrDefault(x => x.UserProfile.UserName == User.Identity.Name && x.OrderId == id).TotalCost;
            ViewBag.Cost = cost;
            ViewBag.Shipment = order.Postage;
            ViewBag.CommodityCount = orderList.Count;
            ViewBag.CommodityInOrderList = orderList;
            return View(order);
        }

        [HttpPost]
        public JsonResult ParseAddress(string value)
        {
            ConsigneeDTO consigneeDto = new ConsigneeDTO();
            if (value != null)
            {
                string[] consignee = value.Split(',');
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
        public ActionResult Home(OrderInfoDTO orderInfoDto)
        {
            Order order = this.db.Orders.Find(Convert.ToInt32(orderInfoDto.OrderId));
            if (orderInfoDto != null)
            {
                string[] address = orderInfoDto.Address.Split(',');
                order.Address = address[4];
                order.Province = address[1];
                order.City = address[2];
                order.Town = address[3];
                order.ConsigneeName = address[0];
                order.Contact = address[5];
                order.Email = address[6];
                order.Payfor = orderInfoDto.Payfor;
                order.Delivery = orderInfoDto.Delivery;
                order.CreationDate = DateTime.Now;
                db.Entry(order).State = EntityState.Modified;

                var userId = GetInfo.GetUserIdByUserName(User.Identity.Name);
                List<CommodityInShoppingTrolley> commodityInShoppingTrolley = this.db.CommodityInShoppingTrolleys.Where(x => x.UserId == userId).ToList();
                this.db.CommodityInShoppingTrolleys.RemoveRange(commodityInShoppingTrolley);
                db.SaveChanges();
                return this.Redirect("/Orders/SingleOrder");
            }

            return this.View();
        }

        public ActionResult SingleOrder()
        {
            ViewBag.CategoryList = GetViewBag.GetCategoryViewBag();

            ViewBag.ShoppingTrolleysCount = GetViewBag.GetShoppingTrolleyViewBag(User.Identity.Name);

            var userId = GetInfo.GetUserIdByUserName(User.Identity.Name);
            var orderList = (from order in this.db.Orders
                             where order.UserId == userId
                             select order).ToList();
            List<OrderViewModel> orderViewModelList = new List<OrderViewModel>();
            foreach (var u in orderList)
            {
                OrderViewModel orderViewModel = new OrderViewModel();
                orderViewModel.OrderId = u.OrderId;
                orderViewModel.Cost = u.TotalCost.Value;
                orderViewModel.ConsigneeName = u.ConsigneeName;
                orderViewModel.Payfor = u.Payfor;
                orderViewModel.State = u.State;
                orderViewModel.CreationDate = u.CreationDate.Value;
                orderViewModel.CommodityInfoes = u.CommodityInOrders.Select(x => new CommodityInOrderViewModel()
                {
                    CommodityId = x.CommodityId,
                    SellerName = x.Commodity.UserProfileCommoditys.Select(p => p.UserProfile.UserName).FirstOrDefault()
                });

                orderViewModelList.Add(orderViewModel);
            }

            ViewBag.OrderList = orderViewModelList;
            return View();
        }

        [HttpPost]
        public JsonResult SingleDelete(int? id)
        {
            Order order = this.db.Orders.Where(x => x.OrderId == id).FirstOrDefault();
            List<CommodityInOrder> model = this.db.CommodityInOrders.Where(x => x.OrderId == id).ToList();

            this.db.CommodityInOrders.RemoveRange(model);
            this.db.Orders.Remove(order);
            return this.Json(this.db.SaveChanges());
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
