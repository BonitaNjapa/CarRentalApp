using CarRentalApp.Models;
using CarRentalApp.Repository;
using CarRentalApp.Repository.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    [Authorize(Roles = "Customer,Admin,Clerk,Manager")]
    public class CustomerController : Controller
    {
        private IStats _StatsRepso = new Stats();
        private ApplicationDbContext db = new ApplicationDbContext();
        [AllowAnonymous]
        [Route("customer-rentals")]
        public async Task<ActionResult> Index(Guid? id)
        {
            ViewBag.AllCategories = new SelectList(db.RentalCatergory, "Id", "Category", id);
            if (id != null)
            {
                return View(await db.RentalItems.Include(m => m.RentalCatergory).Include(c => c.VehicleMake).Include(x => x.GroupingClass).Where(m => m.RentalCatergory.Id == id).Include(x => x.CarModel).ToListAsync());
            }
            return View(await db.RentalItems.Include(m => m.RentalCatergory).Include(c => c.VehicleMake).Include(x => x.GroupingClass).Include(x=>x.CarModel).ToListAsync());
        }
        [Route("bookings")]
        public async Task<ActionResult> Bookings(Guid id)
        {
            ViewBag.PickUpLocation = new SelectList(db.Locations, "Id", "Name");
            ViewBag.DropOffLocation = new SelectList(db.Locations, "Id", "Name");
            var veh = db.RentalItems.Find(id);
            var cls = db.GroupingClasses.ToList().Find(x => x.Name == veh.GroupingClassName);

            ViewBag.VehicleModel = veh;
            BookingViewModel vm = new BookingViewModel();
            ApplicationUser user = db.Users.Find(User.Identity.GetUserId());
            //ApplicationUser user = await db.Users.FirstAsync(x=>x.Id == User.Identity.GetUserId());
            vm.FirstName = user.FirstName;
            vm.Surname = user.LastName;
            vm.Email = user.Email;
            vm.PhoneNumber = user.PhoneNumber;
            vm.GroupingClass = cls;
            return View(vm);
        }
        [HttpPost]
        [Route("bookings")]
        public async Task<ActionResult> Bookings(BookingViewModel booking, Guid id)
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ViewBag.PickUpLocation = new SelectList(db.Locations, "Id", "Name");
            ViewBag.DropOffLocation = new SelectList(db.Locations, "Id", "Name");
            ViewBag.VehicleModel = db.RentalItems.Find(id);
            if (ModelState.IsValid)
            {
                try
                {
                    var loc = new List<BookingLocation>();
                    var book = new Bookings();
                    var pickup = new BookingLocation();
                    var dropoff = new BookingLocation();

                    // book.Id = Guid.NewGuid();

                    pickup.Id = Guid.NewGuid();
                    pickup.LocationId = booking.PickUpLocation;
                    pickup.PickUp = true;
                    //  pickup.BookingId = book.Id;

                    dropoff.Id = Guid.NewGuid();
                    dropoff.LocationId = booking.DropOffLocation;
                    dropoff.PickUp = false;
                    //  dropoff.BookingId = book.Id;
                    book.IsActive = true;
                    db.BookingLocation.Add(pickup);
                    db.BookingLocation.Add(dropoff);
                    db.SaveChanges();

                    loc.Add(pickup);
                    loc.Add(dropoff);

                    book.Id = Guid.NewGuid();
                    book.Reference = Common.Common.RandomString(8);
                    book.FirstName = booking.FirstName;
                    book.Surname = booking.Surname;
                    book.Email = booking.Email;
                    book.PhoneNumber = booking.PhoneNumber;
                    book.BookingLocations = loc;
                    book.PickUpDate = booking.PickUpDate;
                    book.DropOffDate = booking.DropOffDate;
                    book.PickUpTime = booking.PickUpTime;
                    book.DropOffTime = booking.DropOffTime;
                    book.RentalItems = db.RentalItems.Find(id);
                    //edit.Extras.Fuel_Options = Fuel_Options;
                    //edit.Extras = booking.Extras;
                    //book.RentalItems = edit;
                    book.ApplicationUser = userManager.FindById(User.Identity.GetUserId());
                    book.TimeStamp = book.GetTime();
                    //StillBooked(book.RentalItems.Id);
                    //db.Entry(edit).State = EntityState.Modified;
                    db.Bookings.Add(book);
                    await db.SaveChangesAsync();

                    return RedirectToAction("GetInsurance", new { id = book.Id });
                }
                catch (Exception e)
                {
                    var error = e.Message;
                }
            }

            return View(booking);
        }
        public async Task<ActionResult> GetInsurance(Guid id)
        {
            var list = new List<InsuranceViewModel>();
            var existCheck = db.InsuranceBookings.Where(m => m.BookingId == id).ToList();
            List<Guid> ids = new List<Guid>();
            foreach (var item in existCheck)
            {
                ids.Add(item.InsuranceId);
            }

            foreach (var item in await db.Insurance.ToListAsync())
            {
                var l = new InsuranceViewModel();

                l.Id = item.Id;
                l.Name = item.Name;
                l.Price = item.Price;
                l.Description = item.Description;
                l.BookingId = id;
                if (existCheck.Count() > 0)
                {
                    if (existCheck.Where(m => ids.Contains(item.Id)).Any())
                    {
                        l.Selected = true;
                    }
                }
                else
                {
                    l.Selected = false;
                }
                list.Add(l);
            }
            ViewBag.VehicleModel = db.RentalItems.Find(db.Bookings.Include(m => m.RentalItems).Where(m => m.Id == id).FirstOrDefault().RentalItems.Id);
            return View(list);
        }
        public async Task<ActionResult> Damages(Guid id)
        {
            TempData["bookingId"] = id;
            ViewBag.VehicleModel =  db.RentalItems.Find(db.Bookings.Include(m => m.RentalItems).Where(m => m.Id == id).FirstOrDefault().RentalItems.Id);
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Damages(Guid id,string Description,double damageCost)
        {
            var Carid =   db.RentalItems.Find(db.Bookings.Include(m => m.RentalItems).Where(m => m.Id == id).FirstOrDefault().RentalItems.Id);
            Damages Dm = new Damages();
            Dm.DamageId = Guid.NewGuid();
            Dm.Id = Carid.Id;
            Dm.damageCost = damageCost;
            Dm.Description = Description;
            Dm.RentalItems = Carid;
            //ICollection<Damages> ld = new List<Damages>();
            db.Damages.Add(Dm);
            db.SaveChanges();
           // ld.Add(Dm);
            Carid.DamageId = Dm.DamageId;
            db.Entry(Carid).State = EntityState.Modified;
            db.SaveChanges();
            TempData["bookingId"] = id;
            ViewBag.VehicleModel = db.RentalItems.Find(db.Bookings.Include(m => m.RentalItems).Where(m => m.Id == id).FirstOrDefault().RentalItems.Id);
            return RedirectToAction("rental-bookings");
        }
        public async Task<ActionResult> GetExtras(Guid id)
        {
            var list = new List<ExtrasViewModel>();
            ViewBag.VehicleModel = db.RentalItems.Find(db.Bookings.Include(m => m.RentalItems).Where(m => m.Id == id).FirstOrDefault().RentalItems.Id);
            List<Guid> ids = new List<Guid>();
            var existCheck = db.ExtrasBooking.Where(m => m.BookingId == id).ToList();
            foreach (var item in db.ExtrasBooking.Where(m => m.BookingId == id).ToList())
            {
                ids.Add(item.ExtraId);
            }

            foreach (var item in await db.Extras.ToListAsync())
            {
                var l = new ExtrasViewModel();

                l.Id = item.Id;
                l.Name = item.Name;
                l.Price = item.Price;
                l.ImageUri = item.ImageUri;
                l.BookingId = id;
                if (existCheck.Count() > 0)
                {
                    if (existCheck.Where(m => ids.Contains(item.Id)).Any())
                    {
                        l.Selected = true;
                    }
                }
                else
                {
                    l.Selected = false;
                }
                list.Add(l);
            }
            return View(list);
        }
        public ActionResult GotToSummary(Guid id)
        {
            var booking = db.Bookings.Find(id);
            return RedirectToAction("Summary", booking);
        }
        [Route("summary")]
        public async Task<ActionResult> Summary(Bookings bookings)
        {
            Guid bookingId = bookings.Id;
            List<Insurance> insurances = new List<Insurance>();
            List<Extras> extras = new List<Extras>();
            var bookSummary = await db.Bookings.Include(m => m.BookingLocations).Include(m => m.RentalItems).FirstOrDefaultAsync(m => m.Id == bookings.Id);
           // var bookSummary = await db.Bookings.Include(m => m.BookingLocations).Include(m => m.RentalItems).FirstOrDefaultAsync(x => x.Id == bookingId);
            BookingSummary summary = new BookingSummary();
            var pickUpId = bookSummary.BookingLocations.FirstOrDefault(u => u.PickUp).LocationId;
            var DropOffId = bookSummary.BookingLocations.FirstOrDefault(u => !u.PickUp).LocationId;
            summary.Id = bookings.Id;
            summary.Reference = bookSummary.Reference;
            summary.FirstName = bookSummary.FirstName;
            summary.Surname = bookSummary.Surname;
            summary.Email = bookSummary.Email;
            summary.PhoneNumber = bookSummary.PhoneNumber;
            summary.PickUpName = db.Locations.Where(m => m.Id == pickUpId).FirstOrDefault().Name;
            summary.DropOffName = db.Locations.Where(m => m.Id == DropOffId).FirstOrDefault().Name;
            summary.PickUpDate = bookSummary.PickUpDate;
            summary.DropOffDate = bookSummary.DropOffDate;
            summary.PickUpTime = bookSummary.PickUpTime;
            summary.DropOffTime = bookSummary.DropOffTime;
            summary.ApplicationUser = bookings.ApplicationUser;
            summary.RentalItems = bookSummary.RentalItems;


            foreach (var item in db.InsuranceBookings.Where(m => m.BookingId == bookingId).ToList())
            {
                insurances.Add(db.Insurance.Find(item.InsuranceId));
            }
            foreach (var item in db.ExtrasBooking.Where(m => m.BookingId == bookingId).ToList())
            {
                extras.Add(db.Extras.Find(item.ExtraId));
            }
            summary.InsuranceBookings = insurances;
            summary.ExtrasBooking = extras;
            summary.Total = (bookSummary.DropOffDate.Subtract(bookSummary.PickUpDate).TotalDays * (double)bookSummary.RentalItems.Rate) +
            summary.InsuranceBookings.Sum(m => m.Price) + summary.ExtrasBooking.Sum(m => m.Price);
            return View(summary);
        }
        //public async Task<ActionResult> Summary(Bookings bookings)
        //{
        //    var bookSummary = await db.Bookings.Include(m => m.BookingLocations).Include(m => m.RentalItems).FirstOrDefaultAsync(m => m.Id == bookings.Id);
        //    return View(bookSummary);
        //}

        //public ActionResult MyBookings()
        //{
        //    return View();
        //}
        [Route("rental-bookings")]
        public async Task<ActionResult> MyBookings(string Firstname)
        {

            if (User.IsInRole("Admin") || User.IsInRole("Clerk"))
            {
                if (string.IsNullOrEmpty(Firstname))
                {
                    var model1 = await db.Bookings.Include(m => m.RentalItems).Include(m => m.BookingLocations).ToListAsync();

                    return View(model1);
                }
                if (Firstname != null)
                {
                    var modell = db.Bookings.Include(m => m.RentalItems).Include(m => m.BookingLocations).Where(x => x.FirstName.Contains(Firstname) || x.Surname.Contains(Firstname) || x.Reference.Contains(Firstname)).ToList();
                    return View(modell);
                }

                var model = db.Bookings.Include(m => m.RentalItems).Include(m => m.BookingLocations).ToList();
                return View(model);

            }
            else if (User.IsInRole("Customer"))
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

                var app = userManager.FindById(User.Identity.GetUserId()).Id;
                var model = db.Bookings.Where(x => x.ApplicationUser.Id == app).ToList();
                return View(model);
            }
            return View();
        }
        public async Task<ActionResult> CancelBooking(Guid id)
        {
            try
            {
                var model = db.Bookings.Find(id);
                db.Bookings.Remove(model);
                await db.SaveChangesAsync();
            }
            catch
            {

            }

            return RedirectToAction("Index");
        }
        [Route("print-confirmation")]
        public ActionResult Print(Guid id)
        {
            var mx = db.Bookings.Include(m => m.RentalItems).Include(m => m.BookingLocations).FirstOrDefault(m => m.Id == id);


            if (mx.IsCheckedIn == false)
            {
                mx.IsCheckedOut = true;
            }
            else if (mx.IsCheckedOut == true)
            {
                mx.IsCheckedIn = true;
            }
            db.SaveChanges();

            return View(mx);
        }
        [Route("results")]
        public async Task<ActionResult> Results(Guid id)
        {
            var model = db.Bookings.Include(m => m.RentalItems).Include(m => m.BookingLocations).FirstOrDefault(m => m.Id == id);
            if (model == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var plocation = db.Locations.Find(model.BookingLocations.Where(m => m.PickUp).FirstOrDefault().LocationId);
            var dlocation = db.Locations.Find(model.BookingLocations.Where(m => !m.PickUp).FirstOrDefault().LocationId);
            var amount = model.DropOffDate.Subtract(model.PickUpDate).TotalDays * (double)model.RentalItems.Rate;
            try
            {
                var apiKey = ConfigurationManager.AppSettings["apikey"];
                var fromName = ConfigurationManager.AppSettings["sendGridUser"];
                var fromEmail = ConfigurationManager.AppSettings["sendGridFrom"];

                string message = $"Greetings," + "<br/>" +
                    "<br/>" + $"Dear {model.FirstName} {model.Surname}, Welcome to Suzuka car rentals." +
                    "<br/>" + $"Reference: #{model.Reference}" +
                    "<br/>" + $"Pick up location: {plocation.Name}" +
                    "<br/>" + $"Drop off location: {dlocation.Name}" +
                    "<br/>" + $"Pick up date: {model.PickUpDate.ToLongDateString()}" +
                    "<br/>" + $"Drop off date: {model.DropOffDate.ToLongDateString()}" +
                    "<br/>" + $"Pick up time :{model.PickUpTime.ToShortTimeString()}" +
                    "<br/>" + $"Pick up time :{model.DropOffTime.ToShortTimeString()}" +
                    "<br/>" + $"Total Paid: {amount.ToString("c")}" +
                    "<br/>" + "<br/>" + "Regards Suzuka";
                string body = message;

                var client = new SendGridClient(apiKey);
                var from = new EmailAddress(fromEmail, fromName);
                var subject = "Booking Confirmation";
                var to = new EmailAddress(model.Email, "Booking Confirmation");
                var plainTextContent = "";
                var htmlContent = body;
                var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                var response = await client.SendEmailAsync(msg);
            }
            catch
            {

            }
            return View(model);
        }

        //public async void CancelBoo(Guid id)
        //{
        //    var booking =  db.Bookings.Find(id);
        //    booking.IsActive = false;
        //    db.Entry(booking).State = EntityState.Modified;
        //    await db.SaveChangesAsync();
        //    //return RedirectToAction("Index");
        //}
        [HttpGet]
        public JsonResult CancelBooking1(Guid? id)
        {
            if (id != null)
            {
                var b = db.Bookings.Find(id);
                b.IsActive = false;
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Checkout(Guid? Id)
        {
            var get = db.Bookings.Include(m => m.BookingLocations).Include(m => m.RentalItems).FirstOrDefaultAsync(m => m.Id == Id);
            var ma = db.Bookings.Find(Id);
            if (ma.IsCheckedOut == true)
            {
                ma.IsCheckedOut = false;
            }
            else
            {
                ma.IsCheckedOut = true;
            }
            db.SaveChanges();
            return RedirectToAction("Summary", get);
        }

        public ActionResult CheckoutSammary(Bookings bookings)
        {
            return View(bookings);
        }
        [HttpPost]
        public ActionResult AddKmReading(Guid? Id, int KmReading1)
        {
            if (Id != null)
            {
                var b = db.Bookings.Find(Id);
                b.KmReading1 = KmReading1;
                b.IsCheckedOut = true;
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                var bookSummary = db.Bookings.Include(m => m.BookingLocations).Include(m => m.RentalItems).FirstOrDefaultAsync(m => m.Id == Id);
                return RedirectToAction("Summary", bookSummary);
            }
            return View();
        }
        public ActionResult AddKmReadingAfter(Guid? Id, int AddKmReadingAfter)
        {
            if (Id != null)
            {
                var b = db.Bookings.Find(Id);
                b.AddKmReadingAfter = AddKmReadingAfter;
                Checkin(b.Id);
                db.Entry(b).State = EntityState.Modified;
                db.SaveChanges();
                var bookSummary = db.Bookings.Include(m => m.BookingLocations).Include(m => m.RentalItems).FirstOrDefaultAsync(m => m.Id == Id);
                return RedirectToAction("Summary", bookSummary);
            }
             return View();
        }
        public PartialViewResult _Stats()
        {
            return PartialView();
        }
        public void StillBooked(Guid? id)
        {
            if (id != null)
            {
                var car = db.RentalItems.Find(id);
                car.IsAvailable = false;
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void UnBooked(Guid? id)
        {
            if (id != null)
            {
                var car = db.RentalItems.Find(id);
                car.IsAvailable = true;
                db.Entry(car).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
        public void Checkingout(Guid? id)
        {
            var b = db.Bookings.Find(id);
            b.IsCheckedOut = true;
            db.SaveChanges();
        }
        public void Checkin(Guid? id)
        {
            var b = db.Bookings.Find(id);
            b.IsCheckedIn = true;
            db.SaveChanges();
        }
        [HttpGet]
        public JsonResult AddInsurance(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var split = id.Split(',');
                    var insure = new InsuranceBookings();
                    insure.Id = Guid.NewGuid();
                    insure.InsuranceId = Guid.Parse(split[0]);
                    insure.BookingId = Guid.Parse(split[1]);
                    db.InsuranceBookings.Add(insure);
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult RemoveInsurance(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var split = id.Split(',');
                    var insuranceId = Guid.Parse(split[0]);
                    var bookingId = Guid.Parse(split[1]);
                    var remove = db.InsuranceBookings.Where(m => m.InsuranceId == insuranceId
                    && m.BookingId == bookingId).FirstOrDefault();
                    db.InsuranceBookings.Remove(remove);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult AddExtra(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var split = id.Split(',');
                    var insure = new ExtrasBooking();
                    insure.Id = Guid.NewGuid();
                    insure.ExtraId = Guid.Parse(split[0]);
                    insure.BookingId = Guid.Parse(split[1]);
                    db.ExtrasBooking.Add(insure);
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult RemoveExtra(string id)
        {
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var split = id.Split(',');
                    var insuranceId = Guid.Parse(split[0]);
                    var bookingId = Guid.Parse(split[1]);
                    var remove = db.ExtrasBooking.Where(m => m.ExtraId == insuranceId
                    && m.BookingId == bookingId).FirstOrDefault();
                    db.ExtrasBooking.Remove(remove);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                ViewBag.Error = e.Message;
            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AddDriver(Guid id)
        {
            TempData["bookingId"] = id;
            ViewBag.VehicleModel = db.RentalItems.Find(db.Bookings.Include(m => m.RentalItems).Where(m => m.Id == id).FirstOrDefault().RentalItems.Id);
            return View(db.Driver.Where(m => m.BookingId == id).ToList());
        }

        public async Task<ActionResult> AddDriverAction(Guid id)
        {
            TempData["Show"] = "Show";
            return RedirectToAction("AddDriver", new { id });
        }
        [HttpPost]
        public async Task<ActionResult> AddDriverAction(Guid id, string FirstName, string Surname,
            string Email, string PhoneNumber, string IdNumber, string LicenceNumber
            , HttpPostedFileBase fileIdCopy
            , HttpPostedFileBase fileLicenseCopy)
        {
            string pathId = Path.Combine(Server.MapPath("~/Docs"), Guid.NewGuid().ToString() + Path.GetExtension(fileIdCopy.FileName));
            fileIdCopy.SaveAs(pathId);

            string pathLicense = Path.Combine(Server.MapPath("~/Docs"), Guid.NewGuid().ToString() + Path.GetExtension(fileLicenseCopy.FileName));
            fileLicenseCopy.SaveAs(pathLicense);

            Driver driver = new Driver();

            driver.BookingId = id;
            driver.Approved = true;
            driver.Cancelled = false;
            driver.Email = Email;
            driver.FirstName = FirstName;
            driver.Id = Guid.NewGuid();
            driver.IdNumber = IdNumber;
            driver.LicenceNumber = LicenceNumber;
            driver.PhoneNumber = PhoneNumber;
            driver.Surname = Surname;
            driver.IDcopy = pathId.Substring(pathId.LastIndexOf("\\") + 1);
            driver.LicenceCopy = pathLicense.Substring(pathLicense.LastIndexOf("\\") + 1);
            db.Driver.Add(driver);
            db.SaveChanges();

            TempData["bookingId"] = id;
            TempData["Show"] = "Show";
            return RedirectToAction("AddDriver", new { id });
        }

        [HttpGet]
        public JsonResult Delete(Guid? id)
        {
            try
            {
                if (id != null)
                {
                    db.Driver.Remove(db.Driver.Find(id));
                    db.SaveChanges();
                }
            }
            catch
            {

            }
            return Json(new object[] { new object() }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Stats()
        {
            return View();
        }
    }

}