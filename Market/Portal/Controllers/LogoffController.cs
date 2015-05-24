using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Portal.Controllers
{
    public class LogoffController : Controller
    {
        // GET: Logoff
        public ActionResult Index()
        {
            FormsAuthentication.SignOut();
            return this.RedirectToAction("Index", "Login");
        }
    }
}