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
        public long ssn { get; set; }
        [Required]
        public int pin { get; set; }
    }
}