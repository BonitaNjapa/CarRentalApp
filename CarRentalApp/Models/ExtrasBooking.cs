using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class ExtrasBooking
    {
        public Guid Id { get; set; }
        public Guid ExtraId { get; set; }
        public Guid BookingId { get; set; }
    }
}