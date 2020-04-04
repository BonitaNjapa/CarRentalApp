using CarRentalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CarRentalApp.Controllers
{
    public class DamagesController : Controller
    {
        // GET: Damages\
        private ApplicationDbContext db = new ApplicationDbContext(); 
        public ActionResult Index()
        {
            var models =  db.Damages.ToList();
            foreach (var item in models)
            {
                item.RentalItems = db.RentalItems.Find(item.Id);
            }
            return View(models);
        }
    }
}