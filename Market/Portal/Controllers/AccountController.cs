using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class AccountController : Controller
    {
        private MarketContext db = new MarketContext();

        //
        // GET: /Account/ResetPassword
        public ActionResult ResetPassword()
        {
            return View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = this.db.UserProfiles.Single(x => x.UserName == model.UserName);
                if (user != null)
                {
                    user.Password = model.Password;
                    user.UpdateDate = System.DateTime.Now;
                    db.SaveChanges();
                    return this.RedirectToAction("Index", "Login");
                }
            }

            return this.View(model);
        }

        public FileContentResult GetImageByUserName()
        {
            UserProfile userProfile = this.db.UserProfiles.FirstOrDefault(x => x.UserName == User.Identity.Name);
            if (userProfile != null)
            {
                if (userProfile.ImageData != null)
                {
                    return File(userProfile.ImageData, userProfile.ImageType);
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