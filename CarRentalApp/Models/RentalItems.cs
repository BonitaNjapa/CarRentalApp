using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class RentalItems
    {
        public Guid Id { get; set; }
        [Display(Name = "Image")]
        public string ImageUrl { get; set; }
        //[Display(Name = "Vehicle Model")]
        //public CModel CModel { get; set; }
      
        [Display(Name = "Car Registration")]
        public string CarReg { get; set; }
        [Display(Name = "Vehicle Name")]
        public string VehicleName { get; set; }
        public int Seats { get; set; }
        public int Doors { get; set; }
        public bool AirCondition { get; set; }
        [Display(Name = "Tank Capacity(L)")]
        public string TankCapacity { get; set; }
        [Display(Name = "Manual Or Automatic")]
        public string ManualOrAutomatic { get; set; }
        [Display(Name = "Mileage Per Day")]
        public int MileagePerDay { get; set; }
        public bool IsUnlimited { get; set; }
        [DataType(DataType.Currency)]
        public decimal Rate { get; set; }
        [Display(Name = "Catergory")]
        public RentalCatergory RentalCatergory { get; set; }
        public bool IsAvailable { get; set; }
        [Display(Name ="Is This Car Ready To Be Booked ? ")]
        public bool IsBookable { get; set; }
        [Display(Name = "Current Mileage")]
        public int currentMileage { get; set; }
        public Locations Location { get; set; }
        public string Catergory { get; set; }
        [Display(Name = "Location Name")]
        public string LocationName { get; set; }
        public Locations LocationsC { get; set; }
        [Display(Name = "Registration Number")]
        public string RegNumber { get; set; }
        [Display(Name = "Grouping Name")]
        public GroupingClass GroupingClass { get; set; } 

        public string GroupingClassName { get; set; }
        //public Guid packageId { get; set; }
        //[ForeignKey("packageId")]
        //public CategoryPackage CategoryPackage { get; set; }
        public VehicleMake VehicleMake{ get; set; }
        [Display(Name = " Vehicle Make")]
        public string VehicleMakeName { get; set; }
        public virtual ICollection<Feature> Features { get; set; }
        public virtual Extras Extras { get; set; }
        [Display(Name = " Location ")]
        public Guid LocationId { get; set; }
        [Display(Name = " Car Model ")]
        public Guid CarModelId { get; set; }
        [Display(Name = " Car Model ")]
        [ForeignKey("CarModelId")]
        public CarModel CarModel { get; set; }
        public bool isDamaged { get; set; }
        public Guid DamageId { get; set; }
     //   public Damages Damages { get; set; }
        public ICollection<Maintanance> maintanances { get; set; }
        //public ICollection<CategoryPackage> CategoryPackage { get; set; }
    }
    public class VehicleMake
    {
        [Key]
        public Guid VehicleMakeId { get; set; }
        public string Make { get; set; }
    }
}