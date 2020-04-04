using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Locations
    {
        public Guid Id { get; set; }
        [Display(Name = "Branch Name")]
        public string Name { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public Address Address { get; set; }
        public bool Cancelled { get; set; }
    }
}