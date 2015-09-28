using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class Customer : User
    {
        public string CardNumber { get; set; }

        public Customer(string ssn, int pin, string fName, string lName, string cardNumber) : base (ssn, pin, fName, lName)
        {
            CardNumber = cardNumber;
        }
    }
}