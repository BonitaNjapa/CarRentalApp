using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Reservation
    {
        [Key]
        public Guid reservationId { get; set; }
        public string refference { get; set; }
        public string employeeId { get; set; }
        public string FirstName { get; set; }
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
        public Guid carId { get; set; }
        [ForeignKey("carId")]
    //    public virtual Car Car { get; set; }
        public RentalItems RentalItems { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public List<BookingLocation> BookingLocations { get; set; }
    }
}