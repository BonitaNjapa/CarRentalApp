using CarRentalApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    public class InsurancesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("insurance")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Insurance.Where(m => !m.Cancelled).ToListAsync());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                insurance.Id = Guid.NewGuid();
                db.Insurance.Add(insurance);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(insurance);
        }

        // GET: Insurances/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Insurance insurance = await db.Insurance.FindAsync(id);
            if (insurance == null)
            {
                return HttpNotFound();
            }
            return View(insurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Insurance insurance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insurance).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(insurance);
        }

        [HttpGet]
        public JsonResult Delete(Guid id)
        {
            try
            {
                if (id != null)
                {
                    var delete = db.Insurance.Find(id);
                    delete.Cancelled = true;
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