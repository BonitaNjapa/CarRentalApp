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
using System.IO;

namespace CarRentalApp.Controllers
{
    [Authorize(Roles = "Manager,Clerk,Admin")]
    public class RentalItemsController : Controller
    {  
        private ApplicationDbContext db = new ApplicationDbContext();
        [Route("rentals")]
        public async Task<ActionResult> Index()
        {
            var models = await db.RentalItems.Include(m => m.CarModel).Include(m => m.CarModel.Manufacture).ToListAsync();
            return View(models);
        }
        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalItems rentalItems = await db.RentalItems.FindAsync(id);
            if (rentalItems == null)
            {
                return HttpNotFound();
            }
            return View(rentalItems);
        }

        public ActionResult Create()
        {
            ViewBag.LocationList = new SelectList(db.Locations, "Id", "Name");
            ViewBag.Groups = new SelectList(db.GroupingClasses, "ClassId", "Name");
            ViewBag.Manufactures = new SelectList(db.Manufactures, "ManufactureId", "Name");
            ViewBag.CategoriesList = new SelectList(db.RentalCatergory, "Id", "Category");
            ViewBag.CarModelsList = new SelectList(db.CarModels, "CarModelId", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(RentalItems rentalItems, HttpPostedFileBase file, Guid? VehicleCategory, Guid? Group)
        {
            ViewBag.Manufactures = new SelectList(db.Manufactures, "ManufactureId", "Name");

            ViewBag.LocationList = new SelectList(db.Locations, "Id", "Name", rentalItems.LocationId);
            ViewBag.Groups = new SelectList(db.GroupingClasses, "ClassId", "Name", Group);
            ViewBag.CarModelsList = new SelectList(db.CarModels, "CarModelId", "Name", rentalItems.CarModelId);
            ViewBag.CategoriesList = new SelectList(db.RentalCatergory, "Id", "Category", VehicleCategory);
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
                        rentalItems.ImageUrl = path.Substring(path.LastIndexOf("\\") + 1);
                    }
                    catch (Exception e)
                    {
                        ViewBag.Error = "Something went wrong please try again.";
                        return View(rentalItems);
                    }
                }
                //int cr = db.CarModels.Find(rentalItems.CarModelId).CarModelCount;
                //if(rentalItems.CarModel.CarModelCount > 0)
               
                rentalItems.Id = Guid.NewGuid();
                rentalItems.IsAvailable = true;
                rentalItems.RentalCatergory = db.RentalCatergory.Find(VehicleCategory);
                rentalItems.GroupingClass = db.GroupingClasses.Find(Group);
                rentalItems.GroupingClassName = db.GroupingClasses.Find(Group).Name;
                db.RentalItems.Add(rentalItems);
                
                await db.SaveChangesAsync();
                rentalItems.MileagePerDay = db.CarModels.Find(rentalItems.CarModelId).MileagePerDay;
                rentalItems.Rate = db.CarModels.Find(rentalItems.CarModelId).Rate;
                db.Entry(rentalItems).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(rentalItems);
        }

        public async Task<ActionResult> Edit(Guid? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalItems rentalItems = await db.RentalItems.Where(m => m.Id == id).Include(m => m.RentalCatergory).FirstOrDefaultAsync();
            if (rentalItems == null)
            {
                return HttpNotFound();
            }
            ViewBag.LocationList = new SelectList(db.Locations, "Id", "Name", rentalItems.LocationId);
            ViewBag.CategoriesList = new SelectList(db.RentalCatergory, "Id", "Category", rentalItems.RentalCatergory.Id);
            ViewBag.CarModelsList = new SelectList(db.CarModels, "CarModelId", "Name");

            return View(rentalItems);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(RentalItems rentalItems, HttpPostedFileBase file, Guid? VehicleCategory)
        {
            ViewBag.LocationList = new SelectList(db.Locations, "Id", "Name", rentalItems.LocationId);
            ViewBag.CategoriesList = new SelectList(db.RentalCatergory, "Id", "Category", VehicleCategory);
            rentalItems.RentalCatergory = db.RentalCatergory.Find(VehicleCategory);
            if (ModelState.IsValid)
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
                            rentalItems.ImageUrl = path.Substring(path.LastIndexOf("\\") + 1);
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = "Something went wrong please try again.";
                            return View(rentalItems);
                        }
                    }
                    db.Entry(rentalItems).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return View(rentalItems);
        }



        [HttpGet]
        public JsonResult Delete(Guid? id)
        {
            try
            {
                if (id != null)
                {
                    db.RentalItems.Remove(db.RentalItems.Find(id));
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }

        #region mmhm
        [HttpGet]
        public JsonResult MakeBookable(Guid? id)
        {
            if (id != null)
            {
                var car = db.RentalItems.Find(id);
                car.IsBookable = true;
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
            }
            
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
        //[HttpGet]
        //public JsonResult StillBooked(Guid? id)
        //{
        //    if (id != null)
        //    {
        //        var car = db.RentalItems.Find(id);
        //        car.IsAvailable = false;
        //        db.Entry(car).State = EntityState.Modified;
        //        db.SaveChanges();
        //    }

        //    return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        //}
        [HttpGet]
        public JsonResult Maintain(Guid? id)
        {
            if (id != null)
            {
                var car = db.RentalItems.Find(id);
                Maintanance maintanance = new Maintanance();
                maintanance.MaintananceId = Guid.NewGuid();
                maintanance.Id = car.Id;
                maintanance.MaintainanceDate = DateTime.Now;
                db.Maintanances.Add(maintanance);
                addMaintainance(car.Id);
                car.IsBookable = false;
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);

        
        }
        public void addMaintainance(Guid id)
        {
            Maintanance maintanance = new Maintanance();
            maintanance.MaintananceId = Guid.NewGuid();
            maintanance.Id = id;
            maintanance.RentalItems = db.RentalItems.Find(id);
            maintanance.MaintainanceDate = DateTime.Now;
            db.Maintanances.Add(maintanance);
            db.SaveChanges();
        }
       
        #endregion
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public JsonResult GetSProductList(Guid CategoryID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<CarModel> ProductList = db.CarModels.Where(x => x.ManufactureId == CategoryID).ToList();
            return Json(ProductList, JsonRequestBehavior.AllowGet);

        }
    }
}
