using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class Customer : User
    {
        public string CardNumber { get; set; }

        public Customer(string id, int pin, string fName, string lName, string cardNumber) : base (id, pin, fName, lName)
        {
            CardNumber = cardNumber;
        }
    }
}