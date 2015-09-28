using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class Admin : User
    {

        public Admin(string ssn, int pin, string fName, string lName) : base(ssn, pin, fName, lName)
        {
           
        }

        private void InsertMoney()
        {

        }
    }

    
}