using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class ExtrasViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        [Display(Name="Image")]
        public string ImageUri { get; set; }
        public bool Selected { get; set; }
        public Guid BookingId { get; set; }
    }
}