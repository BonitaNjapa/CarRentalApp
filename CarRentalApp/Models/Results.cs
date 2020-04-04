using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Results
    {
        public bool IsCompletedSuccessFully { get; set; }
        public List<string> Errors { get; set; }
    }
}