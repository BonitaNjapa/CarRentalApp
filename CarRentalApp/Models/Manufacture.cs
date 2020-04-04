using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Manufacture
    {
        [Key]
        [Display(Name = "Manufacture")]
        public Guid ManufactureId { get; set; }
        [Display(Name = "Manufacture Name")]

        public string Name { get; set; }
        //public string Logo { get; set; }
        public ICollection<CarModel> CarModel { get; set; }
    }
}