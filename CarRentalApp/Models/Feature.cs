using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Feature
    {
        [Key]
        public Guid fId { get; set; }
        [Display(Name ="Feature Name")]
        public string fName { get; set; }
        public string description { get; set; }
        public string faIcon { get; set; }
    }
}
