using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Catergory
    {
        [Key]
        public Guid catId { get; set; }
        [Display(Name ="Category Name")]
        public string catName { get; set; }
    }
}