using CarRentalApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    public class ExtrasController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Route("extras")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Extras.Where(m => !m.Cancelled).OrderBy(m => m.Name).ToListAsync());
        }

        // GET: Extras/Details/5
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Extras extras = await db.Extras.FindAsync(id);
            if (extras == null)
            {
                return HttpNotFound();
            }
            return View(extras);
        }

        // GET: Extras/Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Extras extras, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (ext != ".png" && ext != ".PNG" && ext != ".jpg" && ext != ".JPG" && ext != ".jpeg" && ext != ".JPEG")
                    {
                        ViewBag.Error = $"Error, Accepted picture format is .png, .jpg and .jpeg";
                        return View();
                    }
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/Images"), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        extras.ImageUri = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = "Something went wrong please try again.";
                        return View(extras);
                    }
                }
                extras.Id = Guid.NewGuid();
                db.Extras.Add(extras);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(extras);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Extras extras = await db.Extras.FindAsync(id);
            if (extras == null)
            {
                return HttpNotFound();
            }
            return View(extras);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Extras extras, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string ext = Path.GetExtension(file.FileName);
                    if (ext != ".png" && ext != ".PNG" && ext != ".jpg" && ext != ".JPG" && ext != ".jpeg" && ext != ".JPEG")
                    {
                        ViewBag.Error = $"Error, Accepted picture format is .png, .jpg and .jpeg";
                        return View();
                    }
                    try
                    {
                        string path = Path.Combine(Server.MapPath("~/Images"), Guid.NewGuid().ToString() + Path.GetExtension(file.FileName));
                        file.SaveAs(path);
                        extras.ImageUri = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = "Something went wrong please try again.";
                        return View(extras);
                    }
                }
                db.Entry(extras).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(extras);
        }

        [HttpGet]
        public JsonResult Delete(Guid? id)
        {
            try
            {
                if (id != null)
                {
                    var update = db.Extras.Find(id);
                    update.Cancelled = true;
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