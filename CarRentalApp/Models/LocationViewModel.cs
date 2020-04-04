using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class LocationViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Branch Name")]
        public string Name { get; set; }
        [Display(Name = "Contact Person")]
        public string ContactPerson { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public string Street { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Suburb { get; set; }
        public long Code { get; set; }
    }
}