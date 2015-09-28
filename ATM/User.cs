using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class User
    {
        
        public string Ssn { get; set; }
        public int Pin { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        public User(string ssn, int pin, string fName, string lName)
        {
            Ssn = ssn;
            Pin = pin;
            FName = fName;
            LName = lName;
        }
    }
}