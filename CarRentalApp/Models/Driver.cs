using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Driver
    {
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "First  Name")]
        public string FirstName { get; set; }
        [Required]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required]
        [Display(Name = "Identity Number")]
        public string IdNumber { get; set; }
        [Required]
        [Display(Name = "Licence Number")]
        public string LicenceNumber { get; set; }
        [Required]
        [Display(Name = "ID Copy")]
        public string IDcopy { get; set; }
        [Required]
        [Display(Name = "Licence Copy")]
        public string LicenceCopy { get; set; }
        public Guid BookingId { get; set; }
        public bool Approved { get; set; }
        public bool Cancelled { get; set; }
    }
}