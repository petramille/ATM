using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ATM.Models
{
    public class BankViewModels
    {
        
    }

    public class Login
    {
        [Required]
        public string ssn { get; set; }
        [Required]
        public string pin { get; set; }
        public string message { get; set; }
    }

    public class Error
    {
        [Required]
        public string errorMessage { get; set; }
    }
}