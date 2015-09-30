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

    public class AccountList
    {
        public List<string> account { get; set; }

        public AccountList()
        {
            this.account = new List<string>();
        }
    }

    public class WithdrawalConfirmation
    {
        public string sum { get; set; }
        public string account { get; set; }
        public string value100 { get; set; }
        public string value200 { get; set; }
        public string value500 { get; set; }
        public string value1000 { get; set; }
    }
}