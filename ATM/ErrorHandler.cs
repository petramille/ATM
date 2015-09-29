using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class ErrorHandler
    {
        public void HandleErrorMessage(string message)
        {
            //sends error message to error page

        }

        public string LogInAlarms(string logInAttempts)
        {
            //
            switch (logInAttempts)
            {
                case "0":
                    return "YOU ARE SUCH A FAILURE!!! GO TO YOUR BANK TO ENABLE YOUR CARD AGAIN";
                case "1":
                    return "WRONG CODE AGAIN. YOU ONLY HAVE ONE ATTEMPT LEFT!!!!!!!";
                case "2":
                    return "Wrong code. You have two more attempts";
                default:
                    return "";
            }
            
        }
    }
}