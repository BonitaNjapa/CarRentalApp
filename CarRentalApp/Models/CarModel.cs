using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class CarModel
    {
        [Key]
        public Guid CarModelId { get; set; }
        public string Name { get; set; }
        public string Year { get; set; }
        //[Display(Name = " Car Model Count")]
        //public int CarModelCount { get; set; }
        [Display(Name = "Mileage Per Day")]
        public int MileagePerDay { get; set; }
        public bool IsUnlimited { get; set; }
        public decimal Rate { get; set; }
        //[Display(Name = "Image")]
        //public string ImageUrl { get; set; }
        [Display(Name = "Variant")]
        public string variant { get; set; }
        [Display(Name = "Manufacture")]
        public Guid ManufactureId { get; set; }
        [ForeignKey("ManufactureId")]
        public Manufacture Manufacture { get; set; }
       // [Display(Name = "Catergory")]
        //public Guid catId { get; set; }
        //[ForeignKey("catId")]
        //public Catergory catergory { get; set; }
      
        public Guid Id { get; set; }
        [ForeignKey("Id")]
        [Display(Name = "Rental Catergory")]
        public RentalCatergory RentalCatergory { get; set; }
         //public ICollection<CategoryPackage> CategoryPackage { get; set; }
    }
}