using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class BookingLocation
    {
        public Guid Id { get; set; }
        public bool PickUp { get; set; }
        public Guid LocationId { get; set; }
        //public Guid BookingId { get; set; }
    }
}