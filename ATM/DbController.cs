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