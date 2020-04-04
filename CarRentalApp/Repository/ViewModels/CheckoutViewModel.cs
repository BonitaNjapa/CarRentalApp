using CarRentalApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Repository.ViewModels
{
    public class CheckoutViewModel
    {
        public Bookings Booking { get; set; }
        public ApplicationUser User { get; set; }
        public EmployeeViewModel Employee { get; set; }
        
    }
}