using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
namespace CarRentalApp.Models
{
    public class CategoryPackage
    {
        [Key]
        public Guid packageId { get; set; }
        public Guid catergoryId { get; set; }
        [Display(Name = "Mileage Per Day")]
        public int MileagePerDay { get; set; }
        public bool IsUnlimited { get; set; }
        [Display(Name = "Rate")]
        public double rate { get; set; }

        //[Display(Name = "Car Model")]
        //public Guid CarModelId { get; set; }
        //[ForeignKey("CarModelId")]
       
        //public CarModel CarModel  { get; set; }

        //public Guid Id { get; set; }
        //[ForeignKey("Id")]
        //public RentalCatergory RentalCatergory { get; set; }
    }
}