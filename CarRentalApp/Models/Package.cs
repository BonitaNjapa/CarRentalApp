using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Package
    {
        [Key]
        public Guid Id { get; set; }
        public Guid carId { get; set; }
        public Guid groupId { get; set; }
        public Guid extraId{ get; set; }
        [Display(Name = "Seats ")]
        public string vSeates { get; set; }
        [Display(Name = "Doors ")]
        public string vDoors { get; set; }
        [Display(Name = "Tank Capacity ")]
        public string  tankCapacity { get; set; }
        public int Mileage { get; set; }
        public double rate { get; set; }
    }
}