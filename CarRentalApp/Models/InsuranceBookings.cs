using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class InsuranceBookings
    {
        public Guid Id { get; set; }
        public Guid InsuranceId { get; set; }
        public Guid BookingId { get; set; }
    }
}