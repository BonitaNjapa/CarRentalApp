using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class BookingSummary
    {
        public Guid Id { get; set; }
        [Required]
        public string Reference { get; set; }
        [Required]
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
        public List<Insurance> InsuranceBookings { get; set; }
        public List<Extras> ExtrasBooking { get; set; }
        [Display(Name = "Pick-Up Name")]
        public string PickUpName { get; set; }
        [Display(Name = "Drop-Off Name")]
        public string DropOffName { get; set; }
        public double Total { get; set; }
    }
}