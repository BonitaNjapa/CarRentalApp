using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Bookings
    {
        public Guid Id { get; set; }
        [Required]
        public string Reference { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Pick up date")]
        public DateTime PickUpDate { get; set; }
        [Required]
        [Display(Name = "Drop off date")]
        public DateTime DropOffDate { get; set; }
        [Required]
        [Display(Name = "Pick up time")]
        public DateTime PickUpTime { get; set; }
        [Required]
        [Display(Name = "Drop off time")]
        public DateTime DropOffTime { get; set; }
        public RentalItems RentalItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<BookingLocation> BookingLocations { get; set; }
        public bool IsActive { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool IsCheckedOut { get; set; } = false;
        public bool IsCheckedIn { get; set; } = false;
        public int KmReading1 { get; set; }
        public int AddKmReadingAfter { get; set; }
        public double ExtrasCost { get; set; }
        public DateTime GetTime()
        {
            return DateTime.Now;
        }
    }


    
}