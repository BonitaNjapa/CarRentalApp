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
    public class CategoryPackagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CategoryPackages
        public ActionResult Index()
        {
            var categoryPackages = db.CategoryPackages/*.Include(c => c.CarModel*//*)*//*.Include(c => c.RentalCatergory)*/;
            return View(categoryPackages.ToList());
        }

        // GET: CategoryPackages/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryPackage categoryPackage = db.CategoryPackages.Find(id);
            if (categoryPackage == null)
            {
                return HttpNotFound();
            }
            return View(categoryPackage);
        }

        // GET: CategoryPackages/Create
        public ActionResult Create()
        {
            //ViewBag.CarModelId = new SelectList(db.CarModels, "CarModelId", "Name");
            //ViewBag.Id = new SelectList(db.RentalCatergory, "Id", "Category");
            return View();
        }

        // POST: CategoryPackages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "packageId,catergoryId,MileagePerDay,IsUnlimited,rate,CarModelId,Id")] CategoryPackage categoryPackage)
        {
            if (ModelState.IsValid)
            {
                categoryPackage.packageId = Guid.NewGuid();
                db.CategoryPackages.Add(categoryPackage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CarModelId = new SelectList(db.CarModels, "CarModelId", "Name", categoryPackage.CarModelId);
            //ViewBag.Id = new SelectList(db.RentalCatergory, "Id", "Category", categoryPackage/*Id*/);
            return View(categoryPackage);
        }

        // GET: CategoryPackages/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryPackage categoryPackage = db.CategoryPackages.Find(id);
            if (categoryPackage == null)
            {
                return HttpNotFound();
            }
            //ViewBag.CarModelId = new SelectList(db.CarModels, "CarModelId", "Name", categoryPackage.CarModelId);
            //ViewBag.Id = new SelectList(db.RentalCatergory, "Id", "Category", categoryPackage.Id);
            return View(categoryPackage);
        }

        // POST: CategoryPackages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "packageId,catergoryId,MileagePerDay,IsUnlimited,rate,CarModelId,Id")] CategoryPackage categoryPackage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(categoryPackage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.CarModelId = new SelectList(db.CarModels, "CarModelId", "Name", categoryPackage.CarModelId);
            //ViewBag.Id = new SelectList(db.RentalCatergory, "Id", "Category", categoryPackage.Id);
            return View(categoryPackage);
        }

        // GET: CategoryPackages/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CategoryPackage categoryPackage = db.CategoryPackages.Find(id);
            if (categoryPackage == null)
            {
                return HttpNotFound();
            }
            return View(categoryPackage);
        }

        // POST: CategoryPackages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CategoryPackage categoryPackage = db.CategoryPackages.Find(id);
            db.CategoryPackages.Remove(categoryPackage);
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
