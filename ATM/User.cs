using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class User
    {
        
        public string Id { get; set; }
        public int Pin { get; set; }
        public string FName { get; set; }
        public string LName { get; set; }

        public User(string id, int pin, string fName, string lName)
        {
            Id = id;
            Pin = pin;
            FName = fName;
            LName = lName;
        }
    }
}