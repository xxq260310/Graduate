using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Portal;
using Portal.Models;
using PagedList;

namespace Portal.Controllers
{
    [Authorize]
    [Portal.Filter.AuthorizationFilter]
    public class AnnouncementsController : Controller
    {
        private MarketContext db = new MarketContext();

        [Authorize(Roles="Administrator")]
        // GET: Announcements
        public ActionResult Index()
        {
            return View(db.Announcements.ToList());
        }

        [Authorize(Roles = "Administrator")]
        // GET: Announcements/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }

            return View(announcement);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Announcements/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Announcements/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.CreationDate = DateTime.Now;
                db.Announcements.Add(announcement);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(announcement);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Announcements/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }
            return View(announcement);
        }

        // POST: Announcements/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Announcement announcement)
        {
            if (ModelState.IsValid)
            {
                announcement.UpdateDate = DateTime.Now;
                db.Entry(announcement).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(announcement);
        }

        [Authorize(Roles = "Administrator")]
        // GET: Announcements/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Announcement announcement = db.Announcements.Find(id);
            if (announcement == null)
            {
                return HttpNotFound();
            }

            return View(announcement);
        }

        // POST: Announcements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Announcement announcement = db.Announcements.Find(id);
            db.Announcements.Remove(announcement);
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
