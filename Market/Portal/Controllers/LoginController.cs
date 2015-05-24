using Portal.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login");
        }

        //
        // POST: /Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                var IsAuthenticated = User.Identity.IsAuthenticated;
                Console.WriteLine(System.Web.HttpContext.Current.User.Identity.IsAuthenticated);
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.RedirectToAction("Index", "Login");
            }
        }

        public ActionResult AlertLogin()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AlertLogin(LoginViewModel model)
        {
            if (Membership.ValidateUser(model.UserName, model.Password))
            {
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                var IsAuthenticated = User.Identity.IsAuthenticated;
                Console.WriteLine(System.Web.HttpContext.Current.User.Identity.IsAuthenticated);
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.RedirectToAction("Index", "Login");
            }
        }
    }
}