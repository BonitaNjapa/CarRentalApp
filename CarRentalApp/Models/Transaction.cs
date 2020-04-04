using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CarRentalApp.Models
{
    public class Transaction
    {
        public Guid Id { get; set; }
        [Display(Name ="Transaction Date")]
        public DateTime TransactionDate { get; set; }
        public string PayRequestId { get; set; }
        public string Reference { get; set; }
        public int Amount { get; set; }
        [Display(Name ="Email Address")]
        public string EmailAddress { get; set; }
        public Guid BookingId { get; set; }
    }
}