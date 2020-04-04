using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class BookingViewModel
    {
        public string Reference { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string Surname { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Pick up location")]
        public Guid PickUpLocation { get; set; }
        [Required]
        [Display(Name = "Drop off location")]
        public Guid DropOffLocation { get; set; }
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
        public GroupingClass GroupingClass { get; set; }
    }
}