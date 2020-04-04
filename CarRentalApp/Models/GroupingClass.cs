using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class GroupingClass
    {
        [Key]
        public Guid ClassId { get; set; }
        [Display(Name = "Group Name")]
        public string Name { get; set; }
    }

}