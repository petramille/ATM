using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ATM
{
    public class DbController
    {
        static string connectionString = @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Contacts;Integrated Security=SSPI";
        SqlConnection myConnection = new SqlConnection();
        SqlCommand command;
        ErrorHandler myErrorHandler;


        //return List<string>
        public List<string> FindUser(string ssn, int pin)
        {

            //Checks if login is valid and if user exists
            //Gets id, first name and number of attempts to login

            //save in session
            //sp
            try
            {
                command.Connection = myConnection;
                myConnection.ConnectionString = connectionString; // @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Contacts;Integrated Security=SSPI";
                myConnection.Open();

                command.CommandText = "SP_Login";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Clear();

                command.Parameters.Add("@Username", System.Data.SqlDbType.VarChar);
                command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar);
                command.Parameters.Add("@ID", System.Data.SqlDbType.VarChar);
                command.Parameters.Add("@fName", System.Data.SqlDbType.VarChar);
                command.Parameters.Add("@numberOfTries", System.Data.SqlDbType.VarChar);


                command.Parameters["@Username"].Value = ssn;
                command.Parameters["@Password"].Value = pin;
                command.Parameters["@ID"].Direction = System.Data.ParameterDirection.Output;
                command.Parameters["@fName"].Direction = System.Data.ParameterDirection.Output;
                command.Parameters["@numberOfTries"].Direction = System.Data.ParameterDirection.Output;

                command.ExecuteNonQuery();

                List<string> tmpCustomer = new List<string>();
                tmpCustomer.Add(System.Convert.ToString(command.Parameters["@ID"].Value));
                tmpCustomer.Add(System.Convert.ToString(command.Parameters["@fName"].Value));
                tmpCustomer.Add(System.Convert.ToString(command.Parameters["@numberOfTries"].Value));

                myConnection.Close();
                return tmpCustomer;

            }
            catch (Exception)
            {
                myErrorHandler.HandleErrorMessage("No connection found");
                return null;
            }

           
        }

        
       

        //Supposed to return List<string> for all accounts
        public void GetAccounts(string ssn)
        {
            //Gets all account numbers and alias associated with that person from db
            //sp
        }

        public double CalculateWithdrawAmountLeftToday(string accountNumber)
        {
            //select directly from db
            double amount = 0;
            return amount;
        }

        //Returns List<string>
        public void GetAccountDetails(string accountNumber)
        {
        //Gets all details about the account chosen by the customer
        //sp
        }

        public void StoreHistory(string eventType, int id, string accountNumber, string ipNumber, double transactionAmount)
        {
            //Sends event details to db for logging
            //sp?
        }

        public double GetAccountBalance(string accountNumber)
        {
            return 0;
            //select directly from db
            
        }

        //returns List<string> with 5 or 25 latest history events
        public void GetAccountHistory(string accountNumber, int numberOfHistoryEvents)
        {
            //select directly from db
        }
        
        //returns List<int> representing the numbers of different bills in the atm
        public bool GetAmountOfBills(int atmId)
        {
            //select directly from db
            return true;
        }

        public bool GetReceiptLeft(int atmId)
        {
            //select directly from db
            return true;
        }
    }
}