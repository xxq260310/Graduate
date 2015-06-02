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
            List<UserProfileViewModel> model = new List<UserProfileViewModel>();
            foreach (var p in userProfiles.Include("Role"))
            {
                UserProfileViewModel user = new UserProfileViewModel()
                { 
                    UserId = p.UserId,
                    UserName = p.UserName,
                    Address = p.Address,
                    CreationDate = p.CreationDate,
                    UpdateDate = p.UpdateDate,
                    Contact = p.Contact,
                    Password = Encryption.RSADecrypt(p.Password),
                    Email = p.Email,
                    Nickname = p.Nickname,
                    RoleName = p.Role.RoleName,
                    Sex = p.Sex
                };
                model.Add(user);
            }

            return View(model.ToList());
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

            userProfile.Password = Encryption.RSADecrypt(userProfile.Password);
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
        public ActionResult Create(UserProfile userProfile, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    userProfile.ImageType = image.ContentType;
                    userProfile.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(userProfile.ImageData, 0, image.ContentLength);
                }

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
            userProfile.Password = Encryption.RSADecrypt(userProfile.Password);
            return View(userProfile);
        }

        // POST: UserProfiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UserProfile userProfile, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if (image != null)
                {
                    userProfile.ImageType = image.ContentType;
                    userProfile.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(userProfile.ImageData, 0, image.ContentLength);
                }

                db.Entry(userProfile).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userProfile);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Image(int? id)
        {
            if (id == null || GetInfo.GetRoleNameByUserName(User.Identity.Name) != "Administrator")
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserProfile userProfile = this.db.UserProfiles.Find(id);
            if (userProfile == null)
            {
                return HttpNotFound();
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

            userProfile.Password = Encryption.RSADecrypt(userProfile.Password);
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

        public FileContentResult GetImageByUserId(int id)
        {
            UserProfile model = this.db.UserProfiles.FirstOrDefault(p => p.UserId == id);
            if (model != null)
            {
                if (model.ImageData != null)
                {
                    return File(model.ImageData, model.ImageType);
                }

                return null;
            }

            else
            {
                return null;
            }
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
