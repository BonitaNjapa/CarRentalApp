using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Damages
    {
        public Guid DamageId { get; set; }
        public Guid Id { get; set; }
      
        public RentalItems RentalItems { get; set; }
        public string Description { get; set; }
        public double damageCost  { get; set; }
    }
}