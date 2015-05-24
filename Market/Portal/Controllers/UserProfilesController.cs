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
    public class UserProfilesController : Controller
    {
        private MarketContext db = new MarketContext();

        public ActionResult CheckAdmin()
        {
            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var roleName = (from userProfile in this.db.UserProfiles
                          from role in this.db.Roles
                          where userProfile.UserName == User.Identity.Name
                          && role.RoleId == userProfile.RoleId
                         select role.RoleName).FirstOrDefault();
            if (string.IsNullOrEmpty(roleName))
            {
                return RedirectToAction("SingleUserProfile", "SingleUserProfiles");
            }

            if (roleName == "Administrator")
            {
                return RedirectToAction("Index", "UserProfiles");
            }

            if (roleName == "TemporaryAdmin")
            {
                return RedirectToAction("TemAdminSingleUserProfile", "SingleUserProfiles");
            }

            return RedirectToAction("SingleUserProfile", "SingleUserProfiles");
        }

        [Authorize(Roles = "Administrator")]
        // GET: UserProfiles
        public ActionResult Index()
        {
            var userProfiles = from userProfile in this.db.UserProfiles
                                   select userProfile;
            return View(userProfiles.ToList());
        }

         [Authorize(Roles = "Administrator")]
        // GET: UserProfiles/Details/5
        public ActionResult Details(int? id)
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

            return View(userProfile);
        }

        [Authorize(Roles = "Administrator")]
        // GET: UserProfiles/Create
        public ActionResult Create()
        {
            ViewBag.UserId = new SelectList(db.Favorites, "UserId", "UserId");
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName");
            ViewBag.UserId = new SelectList(db.ShoppingTrolleys, "UserId", "UserId");
            return View();
        }

        // POST: UserProfiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "UserId,UserName,RoleId,NickName,Sex,Email,Password,Contact,Address")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                db.UserProfiles.Add(userProfile);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        [Authorize(Roles = "Administrator")]
        // GET: UserProfiles/Edit/5
        public ActionResult Edit(int? id)
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

            ViewBag.UserId = new SelectList(db.Favorites, "UserId", "UserId", userProfile.UserId);
            ViewBag.RoleId = new SelectList(db.Roles, "RoleId", "RoleName", userProfile.RoleId);
            ViewBag.UserId = new SelectList(db.ShoppingTrolleys, "UserId", "UserId", userProfile.UserId);
            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "UserId,UserName,RoleId,NickName,Sex,Email,Password,Contact,Address")] UserProfile userProfile)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        [Authorize(Roles = "Administrator")]
        // GET: UserProfiles/Delete/5
        public ActionResult Delete(int? id)
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

            return View(userProfile);
        }

        // POST: UserProfiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UserProfile userProfile = db.UserProfiles.Find(id);
            db.UserProfiles.Remove(userProfile);
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
