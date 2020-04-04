using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CarRentalApp.Models;

namespace CarRentalApp.Controllers
{
    public class RentalCatergoriesController : AdminController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        [Route("rentals-categories")]
        public async Task<ActionResult> Index()
        {
            return View(await db.RentalCatergory.ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Category,Cancelled")] RentalCatergory rentalCatergory)
        {
            if (ModelState.IsValid)
            {
                rentalCatergory.Id = Guid.NewGuid();
                db.RentalCatergory.Add(rentalCatergory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(rentalCatergory);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalCatergory rentalCatergory = await db.RentalCatergory.FindAsync(id);
            if (rentalCatergory == null)
            {
                return HttpNotFound();
            }
            return View(rentalCatergory);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Category,Cancelled")] RentalCatergory rentalCatergory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rentalCatergory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(rentalCatergory);
        }
        [HttpGet]
        public JsonResult Delete(Guid? id)
        {
            try
            {
                if (id != null)
                {
                    db.RentalCatergory.Remove(db.RentalCatergory.Find(id));
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
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
