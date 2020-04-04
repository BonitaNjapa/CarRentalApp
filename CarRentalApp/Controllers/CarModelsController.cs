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
    public class CarModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CarModels
        public ActionResult Index()
        {
            var carModels = db.CarModels/*.Include(c => c.catergory)*/.Include(c => c.Manufacture);
            return View(carModels.ToList());
        }

        // GET: CarModels/Details/5
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carModel = db.CarModels.Find(id);
            if (carModel == null)
            {
                return HttpNotFound();
            }
            return View(carModel);
        }

        // GET: CarModels/Create
        public ActionResult Create()
        {
            ViewBag.catId = new SelectList(db.Catergories, "catId", "catName");
            //ViewBag.Cdis = new SelectList(db.RentalCatergory, "Id", "Catergory");
            ViewBag.CategoriesList = new SelectList(db.RentalCatergory, "Id", "Category");

            ViewBag.ManufactureId = new SelectList(db.Manufactures, "ManufactureId", "Name");
            return View();
        }

        // POST: CarModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CarModelId,Name,variant,Year,IsUnlimited,MileagePerDay,Rate,CarModelCount,ManufactureId,catId,Id")] CarModel carModel)
        {
            if (ModelState.IsValid)
            {
                carModel.CarModelId = Guid.NewGuid();
                db.CarModels.Add(carModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            //ViewBag.Cdis = new SelectList(db.RentalCatergory, "Id", "Catergory", carModel.Id);
            ViewBag.CategoriesList = new SelectList(db.RentalCatergory, "Id", "Category",carModel.Id);


        //    ViewBag.catId = new SelectList(db.Catergories, "catId", "catName", carModel.catId);
            ViewBag.ManufactureId = new SelectList(db.Manufactures, "ManufactureId", "Name", carModel.ManufactureId);
            return View(carModel);
        }

        // GET: CarModels/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carModel = db.CarModels.Find(id);
            if (carModel == null)
            {
                return HttpNotFound();
            }
          //  ViewBag.catId = new SelectList(db.Catergories, "catId", "catName", carModel.catId);
            ViewBag.ManufactureId = new SelectList(db.Manufactures, "ManufactureId", "Name", carModel.ManufactureId);
            //ViewBag.Cdis = new SelectList(db.RentalCatergory, "Id", "Catergory", carModel.Id);
            ViewBag.CategoriesList = new SelectList(db.RentalCatergory, "Id", "Category", carModel.Id);

            return View(carModel);
        }

        // POST: CarModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CarModelId,Name,variant,Year,MileagePerDay,IsUnlimited,Rate,CarModelCount,ManufactureId,catId,Id")] CarModel carModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(carModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           // ViewBag.catId = new SelectList(db.Catergories, "catId", "catName", carModel.catId);
            ViewBag.ManufactureId = new SelectList(db.Manufactures, "ManufactureId", "Name", carModel.ManufactureId);
            //ViewBag.Cdis = new SelectList(db.RentalCatergory, "Id", "Catergory", carModel.Id);
            ViewBag.CategoriesList = new SelectList(db.RentalCatergory, "Id", "Category", carModel.Id);

            return View(carModel);
        }

        // GET: CarModels/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CarModel carModel = db.CarModels.Find(id);
            if (carModel == null)
            {
                return HttpNotFound();
            }
            return View(carModel);
        }

        // POST: CarModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            CarModel carModel = db.CarModels.Find(id);
            db.CarModels.Remove(carModel);
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
