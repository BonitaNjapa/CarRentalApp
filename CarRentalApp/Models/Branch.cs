using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Branch
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string telephone { get; set; }
        public string Email { get; set; }
        public string address { get; set; }
        //public ICollection<Car> cars { get; set; }
    }

}