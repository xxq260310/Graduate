using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class SingleUserProfilesController : Controller
    {
        private MarketContext db = new MarketContext();

        // GET: SingleUserProfile
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SingleUserProfile()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var userProfileItem = (from userProfile in this.db.UserProfiles
                                   where userProfile.UserName == User.Identity.Name
                                   select userProfile).FirstOrDefault();
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel()
            {
                UserId = userProfileItem.UserId,
                UserName = userProfileItem.UserName,
                Nickname = userProfileItem.Nickname,
                Password = userProfileItem.Password,
                Contact = userProfileItem.Contact,
                Email = userProfileItem.Email,
                Address = userProfileItem.Address
            };

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View(userProfileViewModel);
        }

        public ActionResult SingleEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }

            EditUserProfileViewModel model = new EditUserProfileViewModel()
            {
                UserId = userProfile.UserId,
                UserName = userProfile.UserName,
                Nickname = userProfile.Nickname,
                ConfirmPassword = string.Empty,
                Password = string.Empty,
                Contact = userProfile.Contact,
                Address = userProfile.Address,
                Email = userProfile.Email
            };

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SingleEdit(EditUserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.db.UserProfiles.Single(x => x.UserId == model.UserId);
                user.UserName = model.UserName;
                user.Nickname = model.Nickname;
                user.Password = model.Password;
                user.Address = model.Address;
                user.Contact = model.Contact;
                user.Email = model.Email;

                this.db.SaveChanges();
                return this.RedirectToAction("SingleUserProfile");
            }

            return View(model);
        }

        public ActionResult TemAdminSingleUserProfile()
        {
            if (User.Identity.Name == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var userProfileItem = (from userProfile in this.db.UserProfiles
                                   where userProfile.UserName == User.Identity.Name
                                   select userProfile).FirstOrDefault();
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel()
            {
                UserId = userProfileItem.UserId,
                UserName = userProfileItem.UserName,
                Nickname = userProfileItem.Nickname,
                Password = userProfileItem.Password,
                Contact = userProfileItem.Contact,
                Email = userProfileItem.Email,
                Address = userProfileItem.Address
            };

            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;
            return View(userProfileViewModel);
        }

        public ActionResult TemAdminSingleEdit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserProfile userProfile = db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
            }

            EditUserProfileViewModel model = new EditUserProfileViewModel()
            {
                UserId = userProfile.UserId,
                UserName = userProfile.UserName,
                Nickname = userProfile.Nickname,
                ConfirmPassword = string.Empty,
                Password = string.Empty,
                Contact = userProfile.Contact,
                Address = userProfile.Address,
                Email = userProfile.Email
            };
            var commodityInShoppingTrolley = (from shoppingTrolleyItem in this.db.ShoppingTrolleys
                                              from commodityInShoppingTrolleyItem in this.db.CommodityInShoppingTrolleys
                                              where shoppingTrolleyItem.UserId == commodityInShoppingTrolleyItem.UserId
                                              && shoppingTrolleyItem.UserProfile.UserName == User.Identity.Name
                                              select commodityInShoppingTrolleyItem).ToList();
            ViewBag.ShoppingTrolleysCount = commodityInShoppingTrolley.Count;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TemAdminSingleEdit(EditUserProfileViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.db.UserProfiles.Single(x => x.UserId == model.UserId);
                user.UserName = model.UserName;
                user.Nickname = model.Nickname;
                user.Password = model.Password;
                user.Address = model.Address;
                user.Contact = model.Contact;
                user.Email = model.Email;

                this.db.SaveChanges();
                return this.RedirectToAction("SingleUserProfile");
            }

            return View(model);
        }
    }
}