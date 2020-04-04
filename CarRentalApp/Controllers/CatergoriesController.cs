using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarRentalApp.Models;

namespace CarRentalApp.Controllers
{
    public class CatergoriesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Catergories
        public ActionResult Index()
        {
            return View(db.Catergories.ToList());
        }

        // GET: Catergories/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catergory catergory = db.Catergories.Find(id);
            if (catergory == null)
            {
                return HttpNotFound();
            }
            return View(catergory);
        }

        // GET: Catergories/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Catergories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "catId,catName")] Catergory catergory)
        {
            if (ModelState.IsValid)
            {
                catergory.catId = Guid.NewGuid();
                db.Catergories.Add(catergory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catergory);
        }

        // GET: Catergories/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catergory catergory = db.Catergories.Find(id);
            if (catergory == null)
            {
                return HttpNotFound();
            }
            return View(catergory);
        }

        // POST: Catergories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "catId,catName")] Catergory catergory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(catergory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catergory);
        }

        // GET: Catergories/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Catergory catergory = db.Catergories.Find(id);
            if (catergory == null)
            {
                return HttpNotFound();
            }
            return View(catergory);
        }

        // POST: Catergories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Catergory catergory = db.Catergories.Find(id);
            db.Catergories.Remove(catergory);
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
