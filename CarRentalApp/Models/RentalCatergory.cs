using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class RentalCatergory
    {
        public Guid Id { get; set; }
        [Required]

        public string Category { get; set; }
        public bool Cancelled { get; set; }
        public ICollection<CarModel> CarModels { get; set; }

    }
}