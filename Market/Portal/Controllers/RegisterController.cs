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
                if (Image != null)
                {
                    model.ImageType = Image.ContentType;
                    model.ImageData = new byte[Image.ContentLength];
                    Image.InputStream.Read(model.ImageData, 0, Image.ContentLength);

                    string Gender = string.Empty;
                    if (model.Sex == 1)
                    {
                        Gender = "Male";
                    }

                    else if (model.Sex == 2)
                    {
                        Gender = "Female";
                    }
                    UserProfile userProfile = new UserProfile()
                    {
                        UserName = model.UserName,
                        Nickname = model.NickName,
                        Sex = Gender,
                        Email = model.Email,
                        Address = model.Address,
                        Contact = model.Contact,
                        Password = model.Password,
                        ImageData = model.ImageData,
                        ImageType = model.ImageType,
                        CreationDate = System.DateTime.Now
                    };
                    this.db.UserProfiles.Add(userProfile);
                    this.db.SaveChanges();

                    return RedirectToAction("AlertLogin", "Login");
                }
                else
                {
                    string Gender = string.Empty;
                    if (model.Sex == 1)
                    {
                        Gender = "Male";
                    }

                    else if (model.Sex == 2)
                    {
                        Gender = "Female";
                    }
                    UserProfile userProfile = new UserProfile()
                    {
                        UserName = model.UserName,
                        Nickname = model.NickName,
                        Sex = Gender,
                        Email = model.Email,
                        Address = model.Address,
                        Contact = model.Contact,
                        Password = model.Password,
                        CreationDate = System.DateTime.Now
                    };
                    this.db.UserProfiles.Add(userProfile);
                    this.db.SaveChanges();

                    return RedirectToAction("AlertLogin", "Login");
                }
            }

            return this.View(model);
            
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