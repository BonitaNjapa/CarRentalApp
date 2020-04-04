using CarRentalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Repository
{
    public class Stats : IStats
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public int Bookings()
        {
            return context.Bookings.Count();
        }

        public int GetCarCount()
        {
            return context.RentalItems.Count();
        }

        public int GetRentals()
        {
            return context.Bookings.Count();
        }

        public int Maintainance()
        {
            return context.RentalItems.Where(x => x.IsBookable == false).Count();
        }
        public int Transactions()
        {
            return context.Transaction.Count();
        }

    }
}