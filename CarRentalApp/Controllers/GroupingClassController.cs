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
    public class GroupingClassController : Controller
    {
       private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult CreateGroupingClass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateGroupingClass(GroupingClass clas)
        {
            clas.ClassId = Guid.NewGuid();
            //StandardWaiver.groupingClassId = clas.ClassId;
            //SuperWaiver.groupingClassId = clas.ClassId;
            //clas.StandardWaiver = StandardWaiver;
            //clas.SuperWaiver = SuperWaiver;
            db.GroupingClasses.Add(clas);
            db.SaveChanges();
            return RedirectToAction("ListClasses");
        }
        public ActionResult ListClasses(string Search)
        {
            return View(db.GroupingClasses.ToList());
        }
        // GET: Insurances/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            GroupingClass insurance = await db.GroupingClasses.FindAsync(id);
            if (insurance == null)
            {
                return HttpNotFound();
            }
            return View(insurance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(GroupingClass insurance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insurance).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("ListClasses");
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
                    var delete = db.GroupingClasses.Find(id);
                    db.GroupingClasses.Remove(delete);
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
