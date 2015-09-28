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


        //return List<string>
        public void FindUser(string ssn, int pin)
        {
            
            //Checks if login is valid and if user exists
            //Gets id, first name and number of attempts to login
            
            //save in session
            //sp
        }

        
       

        //Supposed to return List<string> for all accounts
        public void GetAccounts(string ssn)
        {
            //Gets all account numbers and alias associated with that person
        }

        public double CalculateWithdrawAmountLeftToday(string accountNumber)
        {
            double amount = 0;
            return amount;
        }

        //Returns List<string>
        public void GetAccountDetails(string accountNumber)
        {
        //Gets all details about the account chosen by the customer
        }

        public void StoreHistory(string eventType, int id, string accountNumber, string ipNumber, double transactionAmount)
        {
            //Sends event details to db for logging
        }

        public double GetAccountBalance(string accountNumber)
        {
            return 0;
            
        }

        //returns List<string> with 5 or 25 latest history events
        public void GetAccountHistory(string accountNumber, int numberOfHistoryEvents)
        {

        }
        
        //returns List<int> representing the numbers of different bills in the atm
        public void GetAmountOfBills(int atmId)
        {
            
        }

        public bool GetReceiptLeft(int atmId)
        {
            return true;
        }
    }
}