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
    public class MaintanancesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Maintanances
        public ActionResult Index()
        {
            var maintanances = db.Maintanances.Include(m => m.RentalItems);
            return View(maintanances.ToList());
        }

        // GET: Maintanances/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maintanance maintanance = db.Maintanances.Find(id);
            if (maintanance == null)
            {
                return HttpNotFound();
            }
            return View(maintanance);
        }

        // GET: Maintanances/Create
        public ActionResult Create()
        {
            ViewBag.Id = new SelectList(db.RentalItems, "Id", "ImageUrl");
            return View();
        }

        // POST: Maintanances/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaintananceId,Id,MaintainanceDate,Descritpion")] Maintanance maintanance)
        {
            if (ModelState.IsValid)
            {
                maintanance.MaintananceId = Guid.NewGuid();
                db.Maintanances.Add(maintanance);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Id = new SelectList(db.RentalItems, "Id", "ImageUrl", maintanance.Id);
            return View(maintanance);
        }

        // GET: Maintanances/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maintanance maintanance = db.Maintanances.Find(id);
            if (maintanance == null)
            {
                return HttpNotFound();
            }
            ViewBag.Id = new SelectList(db.RentalItems, "Id", "ImageUrl", maintanance.Id);
            return View(maintanance);
        }

        // POST: Maintanances/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaintananceId,Id,MaintainanceDate,Descritpion")] Maintanance maintanance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(maintanance).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Id = new SelectList(db.RentalItems, "Id", "ImageUrl", maintanance.Id);
            return View(maintanance);
        }

        // GET: Maintanances/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Maintanance maintanance = db.Maintanances.Find(id);
            if (maintanance == null)
            {
                return HttpNotFound();
            }
            return View(maintanance);
        }

        // POST: Maintanances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Maintanance maintanance = db.Maintanances.Find(id);
            db.Maintanances.Remove(maintanance);
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
