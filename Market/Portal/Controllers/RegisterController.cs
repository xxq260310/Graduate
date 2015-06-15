using Portal.Models;
using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class RegisterController : Controller
    {
        private MarketContext db = new MarketContext();

        // GET: Register
        public ActionResult Index()
        {
            return View("Register");
        }

        //
        // POST: Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model, HttpPostedFileBase Image)
        {
            var userProfileCount = (from user in this.db.UserProfiles
                                    where user.UserName == model.UserName
                                    select user).Count();
            if (userProfileCount >= 1)
            {
                ModelState.AddModelError("UserName", "The UserName has been existed!");
            }

            if (ModelState.IsValid)
            {
                UserProfile userProfile = new UserProfile();
                if (Image != null)
                {
                    model.ImageType = Image.ContentType;
                    model.ImageData = new byte[Image.ContentLength];
                    Image.InputStream.Read(model.ImageData, 0, Image.ContentLength);
                    userProfile.ImageData = model.ImageData;
                    userProfile.ImageType = model.ImageType;
                }

                string Gender = model.Sex == 1 ? "Male" : "Female";
                userProfile.UserName = model.UserName;
                userProfile.Nickname = model.NickName;
                userProfile.Sex = Gender;
                userProfile.Email = model.Email;
                userProfile.Address = model.Address;
                userProfile.Contact = model.Contact;
                userProfile.Password = SetPassword(model.Password);
                userProfile.CreationDate = System.DateTime.Now;
                userProfile.RoleId = 2;
                this.db.UserProfiles.Add(userProfile);
                this.db.SaveChanges();

                return RedirectToAction("AlertLogin", "Login");
            }

            return View();
        }

        public static string SetPassword(string password)
        {
            string value = Encryption.RSAEncrypt(password);
            return value;
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