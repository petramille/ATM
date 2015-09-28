using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ATM
{
    public class DbController
    {
        static string connectionString = "";
        SqlConnection newConnection = new SqlConnection();
        SqlCommand newCommand;

        public string FindUser(string cardNumber, int pin)
        {
            string id = "";
            //Checks if login is valid and if user exists
            return id;

        }

        
        public string GetUserName(string id)
        {
            string fName = "";
            return fName;
            //Gets firstName from db
        }

        //Supposed to return List<string> for all accounts
        public void GetAccounts(string id)
        {
            //Gets all account numbers and alias associated with that person
        }

        //Returns List<string>
        public void GetAccountDetails(string accountNumber)
        {
        //Gets all details about the account chosen by the customer
        }
    }
}