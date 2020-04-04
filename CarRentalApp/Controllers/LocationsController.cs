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
    [Authorize]
    public class LocationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Locations
        [Route("branches")]
        public async Task<ActionResult> Index()
        {
            return View(await db.Locations.Include(m => m.Address).Where(m => !m.Cancelled).ToListAsync());
        }

        public async Task<ActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Locations locations = await db.Locations.FindAsync(id);
            if (locations == null)
            {
                return HttpNotFound();
            }
            return View(locations);
        }
        // GET: Locations/Create
        public ActionResult Create()
        {
            ViewBag.ProvincesDropDownList = new SelectList(GetProvinces(), "Value", "Value");
            return View();
        }

        // POST: Locations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(LocationViewModel locations)
        {
            ViewBag.ProvincesDropDownList = new SelectList(GetProvinces(), "Value", "Value");
            if (ModelState.IsValid)
            {
                var loc = new Locations();
                var address = new Address();
                loc.Id = Guid.NewGuid();
                loc.Name = locations.Name;
                loc.ContactPerson = locations.ContactPerson;
                loc.Contact = locations.Contact;
                loc.Email = locations.Email;
                loc.Cancelled = false;
                loc.Address = address;

                address.Id = Guid.NewGuid();
                address.Street = locations.Street;
                address.Province = locations.Province;
                address.City = locations.City;
                address.Suburb = locations.Suburb;
                address.Code = locations.Code;


                db.Locations.Add(loc);
                db.Address.Add(address);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(locations);
        }
        private static List<Provinces> GetProvinces()
        {
            string[] pro = { "Earsten Cape",
             "Free State",
            "Gauteng",
            "KwaZulu-Natal",
            "Limpopo",
            "Mpumalanga",
            "Northern Cape",
            "North West",
            "Western Cape"};

            var list = new List<Provinces>();
            for (int i = 0; i < pro.Length; i++)
            {
                list.Add(new Provinces()
                {
                    Key = i,
                    Value = pro[i]
                });
            }

            return list;
        }
        // GET: Locations/Edit/5
        public async Task<ActionResult> Edit(Guid? id)
        {
            ViewBag.ProvincesDropDownList = new SelectList(GetProvinces(), "Value", "Value");
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Locations locations = await db.Locations.Include(m => m.Address).FirstOrDefaultAsync(m => m.Id == id);
            if (locations == null)
            {
                return HttpNotFound();
            }
            LocationViewModel locationViewModel = new LocationViewModel();
            locationViewModel.Id = locations.Id;
            locationViewModel.Name = locations.Name;
            locationViewModel.ContactPerson = locations.ContactPerson;
            locationViewModel.Contact = locations.Contact;
            locationViewModel.Email = locations.Email;
            locationViewModel.Street = locations.Address.Street;
            locationViewModel.Province = locations.Address.Province;
            locationViewModel.City = locations.Address.City;
            locationViewModel.Suburb = locations.Address.Suburb;
            locationViewModel.Code = locations.Address.Code;

            return View(locationViewModel);
        }


        // POST: Locations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(LocationViewModel locations)
        {
            ViewBag.ProvincesDropDownList = new SelectList(GetProvinces(), "Value", "Value");
            if (ModelState.IsValid)
            {
                Locations update = await db.Locations.Include(m => m.Address).FirstOrDefaultAsync(m => m.Id == locations.Id);
                Address address = await db.Address.FindAsync(update.Address.Id);

                update.Name = locations.Name;
                update.ContactPerson = locations.ContactPerson;
                update.Contact = locations.Contact;
                update.Email = locations.Email;
                update.Cancelled = false;
                update.Address = address;

                address.Street = locations.Street;
                address.Province = locations.Province;
                address.City = locations.City;
                address.Suburb = locations.Suburb;
                address.Code = locations.Code;


                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(locations);
        }

        // GET: Locations/Delete/5
        [HttpGet]
        public JsonResult Delete(Guid id)
        {
            try
            {
                if (id != null)
                {
                    var delete = db.Locations.Find(id);
                    delete.Cancelled = false;
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }

        public class Provinces
        {
            public int Key { get; set; }
            public string Value { get; set; }
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
