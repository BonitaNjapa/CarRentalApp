using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Extras
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        [Display(Name="Image")]
        public string ImageUri { get; set; }
        public bool Cancelled { get; set; }
    }
}