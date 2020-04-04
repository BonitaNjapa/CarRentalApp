using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Maintanance
    {
        [Key]
        public Guid MaintananceId { get; set; }
        public Guid Id { get; set; }
        [ForeignKey("Id")]
        public RentalItems RentalItems { get; set; }
        [Display(Name ="Maintance Date")]
        public DateTime MaintainanceDate { get; set; }
        public string Descritpion { get; set; }
    }
}