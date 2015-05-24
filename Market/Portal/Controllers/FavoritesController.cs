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
    public class FavoritesController : Controller
    {
        private MarketContext db = new MarketContext();

        public ActionResult SingleDelete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userId = (from favorite in this.db.Favorites
                                        where favorite.UserProfile.UserName == User.Identity.Name
                                        select favorite.UserId).FirstOrDefault();
            var commodityInFavorite = (from item in this.db.CommodityInFavorites
                                       where item.UserId == userId 
                                       && item.CommodityId == id.Value
                                       select item).FirstOrDefault();
            this.db.CommodityInFavorites.Remove(commodityInFavorite);
            this.db.SaveChanges();
            return RedirectToAction("Home");
        }

        public ActionResult Home()
        {
            var userId = (from userProfile in this.db.UserProfiles
                          where userProfile.UserName == User.Identity.Name
                          select userProfile.UserId).FirstOrDefault();
            var commodityIdList = (from commodityInFavorite in this.db.CommodityInFavorites
                            where commodityInFavorite.UserId == userId
                            select commodityInFavorite.CommodityId).ToList();
            List<CommodityViewModel> model = new List<CommodityViewModel>();
            foreach (var p in commodityIdList)
            {
                var commodities = (from commodity in this.db.Commodities
                                  where commodity.CommodityId == p
                                  select commodity).FirstOrDefault();
                CommodityViewModel commodityViewModel = new CommodityViewModel() { 
                    CommodityId = commodities.CommodityId,
                    UnitPrice = commodities.UnitPrice,
                    Description = commodities.Description
                };
                model.Add(commodityViewModel);
            }

            ViewBag.CommoditiesInFavorite = model;

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View();
        }

        public JsonResult AddCommodityInFavorite(int? id)
        {
            var userId = (from userProfile in this.db.UserProfiles
                          where userProfile.UserName == User.Identity.Name
                          select userProfile.UserId).FirstOrDefault();
            Favorite model = this.db.Favorites.Find(userId);
            int state = 0;
            if (model == null)
            {
                Favorite favorite = new Favorite()
                {
                    UserId = userId,
                    CreationDate = System.DateTime.Now
                };
                this.db.Favorites.Add(favorite);

                CommodityInFavorite commodityInFavorite = new CommodityInFavorite()
                {
                    UserId = userId,
                    CommodityId = id.Value
                };
                this.db.CommodityInFavorites.Add(commodityInFavorite);

                state = 1;
            }
            else
            {
                var commodityInFavoriteItem = this.db.CommodityInFavorites.SingleOrDefault(p => p.CommodityId == id && p.UserId == userId);
                if (commodityInFavoriteItem == null)
                {
                    Favorite favorite = this.db.Favorites.SingleOrDefault(p => p.UserId == userId);
                    favorite.UpdateDate = DateTime.Now;
                    CommodityInFavorite commodityInFavorite = new CommodityInFavorite()
                    {
                        UserId = userId,
                        CommodityId = id.Value
                    };
                    this.db.CommodityInFavorites.Add(commodityInFavorite);
                    state = 1;
                }
                else
                {
                    state = 0;
                }
                
            }
            
            this.db.SaveChanges();
            return Json(state, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult GetJson(int id)
        //{
        //    var userId = (from userProfile in this.db.UserProfiles
        //                  where userProfile.UserName == User.Identity.Name
        //                  select userProfile.UserId).FirstOrDefault();
        //    var commodityInFavoriteItem = this.db.CommodityInFavorites.SingleOrDefault(p => p.CommodityId == id && p.UserId == userId);
        //    int count = 0;
        //    if(commodityInFavoriteItem != null)
        //    {
        //        count++;
        //    }
        //    return Json(count,JsonRequestBehavior.AllowGet);
        //}

        [Authorize(Roles = "Administrator")]
        // GET: Favorites
        public ActionResult Index()
        {
            var favorites = db.Favorites.Include(f => f.UserProfile);
            return View(favorites.ToList());
        }

        [Authorize(Roles = "Administrator")]
        // GET: Favorites/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Favorite favorite = db.Favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }
            return View(favorite);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Favorites/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName");
            return View();
        }

        // POST: Favorites/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Favorite favorite)
        {
            if (ModelState.IsValid)
            {
                favorite.CreationDate = DateTime.Now;
                db.Favorites.Add(favorite);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(favorite);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Favorites/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Favorite favorite = db.Favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }

            var commodityInFavorite = (from item in this.db.CommodityInFavorites
                                       from commodityItem in this.db.Commodities
                                       where item.UserId == id
                                       && commodityItem.CommodityId == item.CommodityId
                                       select new
                                       {
                                           CommodityId = item.CommodityId
                                       }).ToList();
            var commodity = from commodityItem in this.db.Commodities
                            select new
                            {
                                Value = commodityItem.CommodityId,
                                Text = commodityItem.CommodityName
                            };
            ViewBag.CommodityId = new SelectList(commodity, "Value", "Text");
            var favorites = new List<CommodityInFavoriteViewModel>();
            foreach (var p in commodityInFavorite)
            {
                CommodityInFavoriteViewModel model = new CommodityInFavoriteViewModel();
                model.CommodityId = new SelectList(commodity, "Value", "Text", p.CommodityId);

                favorites.Add(model);
            }
            ViewBag.SelectedCommodities = favorites;
            ViewBag.UserId = new SelectList(db.UserProfiles, "UserId", "UserName", favorite.UserId);
            return View(favorite);
        }

        // POST: Favorites/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Favorite favorite, IEnumerable<CommodityInFavorite> model)
        {
            if (ModelState.IsValid)
            {
                favorite.UpdateDate = DateTime.Now;
                this.db.Entry(favorite).State = EntityState.Modified;

                var commodities = (from commodityInFavorites in this.db.CommodityInFavorites
                                   where commodityInFavorites.UserId == favorite.UserId
                                   select commodityInFavorites).ToList();
                this.db.CommodityInFavorites.RemoveRange(commodities);

                if (model != null)
                {
                    foreach (var p in model)
                    {
                        CommodityInFavorite commodityInFavorite = new CommodityInFavorite();
                        commodityInFavorite.UserId = favorite.UserId;
                        commodityInFavorite.CommodityId = p.CommodityId;

                        this.db.CommodityInFavorites.Add(commodityInFavorite);
                    }
                }

                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(favorite);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Favorites/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Favorite favorite = db.Favorites.Find(id);
            if (favorite == null)
            {
                return HttpNotFound();
            }

            return View(favorite);
        }

        // POST: Favorites/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Favorite favorite = db.Favorites.Find(id);
            db.CommodityInFavorites.RemoveRange(favorite.CommodityInFavorites);
            db.Favorites.Remove(favorite);
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
