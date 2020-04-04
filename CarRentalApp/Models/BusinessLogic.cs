using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace CarRentalApp.Models
{
    public class BusinessLogic
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public double calcBasic()
        {
            double basic = 0;
            foreach (var e in db.CategoryPackages.ToList())
            {
                basic = e.rate* e.MileagePerDay;
            }
            return basic;
        }
        public double totalRental()
        {
            double tot = 0;

            foreach (var e in db.Transaction.ToList())
            {
                tot += e.Amount;
            }
            return tot; 
        }
        // public double totalExtras()
        //{
        //    double additions = 0;
        //    foreach (var e in db.Extras.ToList())

        //    {
        //        additions += e.Price;
        //    }
        //    return additions;
        //}
    }
}