using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ATM
{
    public class DbController
    {
        /// <summary>
        /// Class that manage the conections to the database. Calls stored procedures and put in queries.
        /// </summary>

        static string connectionString = @"Server=tcp:igru9irx7p.database.windows.net,1433;Database=ATM;User ID=ATM-Admin@igru9irx7p;Password=Qwerty123!;Trusted_Connection=False;Encrypt=True;Connection Timeout=30";
        SqlConnection myConnection = new SqlConnection();
        SqlCommand command = new SqlCommand();



        /// <summary>
        /// Checks if login is valid and user exists. 
        /// </summary>
        /// <param name="ssn">social security number and cardnumber</param>
        /// <param name="pin">pin-code</param>
        /// <returns>List<string> with UserId, first name and number of login tries. null if connection fails</returns>
        public List<string> FindUser(string ssn, string pin)
        {
            try
            {
                command.Connection = myConnection;
                myConnection.ConnectionString = connectionString; // @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Contacts;Integrated Security=SSPI";
                myConnection.Open();

                command.CommandText = "SP_Login";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 12);
                command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 4);
                command.Parameters.Add("@ID", System.Data.SqlDbType.VarChar, 12);
                command.Parameters.Add("@fName", System.Data.SqlDbType.VarChar, 20);
                command.Parameters.Add("@numberOfTries", System.Data.SqlDbType.Int);


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
                //myErrorHandler.HandleErrorMessage("No access to the ATM at the moment");
                return null;
            }
        }

        /// <summary>
        /// Calls stored procedure to Withdraw money from a account
        /// </summary>
        /// <param name="accountNumber">Account number where money should be withdrawn</param>
        /// <param name="ssn">social security number/cardnumber </param>
        /// <param name="amount">amount to withdraw</param>
        /// <returns> string with number 1 if success and 0 if unsuccess</returns>
        public string WithdrawFromAccount(string accountNumber, string ssn, double amount)
        {

            try
            {
                command.Connection = myConnection;
                myConnection.ConnectionString = connectionString; // @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Contacts;Integrated Security=SSPI";
                myConnection.Open();

                command.CommandText = "SP_WithdrawMoney";
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add("@AccountNR", System.Data.SqlDbType.VarChar, 8);
                command.Parameters.Add("@Ssn", System.Data.SqlDbType.VarChar, 12);
                command.Parameters.Add("@WithdrawAmount", System.Data.SqlDbType.Float);
                command.Parameters.Add("@Message", System.Data.SqlDbType.VarChar, 1);


                command.Parameters["@AccountNR"].Value = accountNumber;
                command.Parameters["@Ssn"].Value = ssn;
                command.Parameters["@WithdrawAmount"].Value = amount;
                command.Parameters["@Message"].Direction = System.Data.ParameterDirection.Output;

                command.ExecuteNonQuery();

                string message;
                message = System.Convert.ToString(command.Parameters["@Message"].Value);

                myConnection.Close();
                return message;

            }
            catch (Exception)
            {

                return null;
            }
        }

        /// <summary>
        /// Stores history in db by a stored procedure. 
        /// </summary>
        /// <param name="eventTime">time when logging happens</param>
        /// <param name="eventType">type of event to log. exampel is withdraw. can not be null!!</param>
        /// <param name="ssn">social security number/cardnumber. Can not be null </param>
        /// <param name="accountNumber">Account number where the action happened. Can be null</param>
        /// <param name="transactionAmount">The amount of the transaction. Can be null</param>
        public void StoreHistory(DateTime eventTime, string eventType, string ssn, string accountNumber, double transactionAmount)
        {
            try
            {
                command.Connection = myConnection;
                myConnection.ConnectionString = connectionString; // @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Contacts;Integrated Security=SSPI";
                myConnection.Open();

                command.CommandText = "SP_Logging";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Clear();

                command.Parameters.Add("@EventTime", System.Data.SqlDbType.DateTime);
                command.Parameters.Add("@Eventtype", System.Data.SqlDbType.VarChar, 20);
                command.Parameters.Add("@Ssn", System.Data.SqlDbType.VarChar, 12);
                // command.Parameters.Add("@IP", System.Data.SqlDbType.VarChar, 15);
                command.Parameters.Add("@AccountNr", System.Data.SqlDbType.VarChar, 8);
                command.Parameters.Add("@HandledAmount", System.Data.SqlDbType.VarChar, 8);

                command.Parameters["@EventTime"].Value = eventTime;
                command.Parameters["@EventType"].Value = eventType;
                command.Parameters["@Ssn"].Value = ssn;
                //command.Parameters["@IP"].Value = ipNumber;
                command.Parameters["@AccountNr"].Value = accountNumber;
                command.Parameters["@HandledAmount"].Value = transactionAmount;


                command.ExecuteNonQuery();
                myConnection.Close();

            }
            catch (Exception)
            {


            }
        }

        /// <summary>
        /// Updates the amount of bills in the atm after a withdrawal
        /// </summary>
        /// <param name="atmId">each atm has an Id</param>
        /// <param name="withdrawed100">amount of 100 SEK bills that should be subtracted</param>
        /// <param name="withdrawed200">amount of 200 SEK bills that should be subtracted</param>
        /// <param name="withdrawed500">amount of 500 SEK bills that should be subtracted</param>
        /// <param name="withdrawed1000">amount of 1000 SEK bills that should be subtracted</param>
        public void UpdateNumberOfBills(string atmId, int withdrawed100, int withdrawed200, int withdrawed500, int withdrawed1000)
        {
            try
            {
                command.Connection = myConnection;
                myConnection.ConnectionString = connectionString; // @"Data Source=localhost\SQLEXPRESS;Initial Catalog=Contacts;Integrated Security=SSPI";
                myConnection.Open();

                command.CommandText = "SP_UpdateATMBills";
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.Clear();

                command.Parameters.Add("@ID", System.Data.SqlDbType.VarChar, 2);
                command.Parameters.Add("@Bills100", System.Data.SqlDbType.Int);
                command.Parameters.Add("@Bills200", System.Data.SqlDbType.Int);
                command.Parameters.Add("@Bills500", System.Data.SqlDbType.Int);
                command.Parameters.Add("@Bills1000", System.Data.SqlDbType.Int);

                command.Parameters["@ID"].Value = atmId;
                command.Parameters["@Bills100"].Value = withdrawed100;
                command.Parameters["@Bills200"].Value = withdrawed200;
                command.Parameters["@Bills500"].Value = withdrawed500;
                command.Parameters["@Bills1000"].Value = withdrawed1000;

                command.ExecuteNonQuery();
                myConnection.Close();

            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Reads from db. Ads everything per found item in one line
        /// </summary>
        /// <param name="commandLine">QueryLine</param>
        /// <returns> List<string> with the result of the query</returns>
        public List<string> readFromSQL(string commandLine)
        {
            myConnection.ConnectionString = connectionString;
            SqlDataReader myReader = null;

            try
            {
                myConnection.Open();
                command = new SqlCommand();
                command.Connection = myConnection;

                command.CommandText = commandLine;
                myReader = command.ExecuteReader();

                string mySQLResultLine = "";
                List<string> mySQLResult = new List<string>();
                if (myReader != null)
                {
                    while (myReader.Read())
                    {
                        mySQLResultLine = "";
                        for (int i = 0; i < myReader.FieldCount; i++)
                        {
                            mySQLResultLine += myReader[i].ToString() + " ";
                        }
                        mySQLResult.Add(mySQLResultLine);
                    }
                    return mySQLResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }
            }
        }

        /// <summary>
        /// reads from db ads everything thats found in a List<string>
        /// </summary>
        /// <param name="commandLine">QueryLine</param>
        /// <returns>List<string> of items found</returns>
        public List<string> readSingleColumnFromSQL(string commandLine)
        {
            myConnection.ConnectionString = connectionString;
            SqlDataReader myReader = null;

            try
            {
                myConnection.Open();
                command = new SqlCommand();
                command.Connection = myConnection;

                command.CommandText = commandLine;
                myReader = command.ExecuteReader();

                List<string> mySQLResult = new List<string>();
                if (myReader != null)
                {
                    while (myReader.Read())
                    {
                        for (int i = 0; i < myReader.FieldCount; i++)
                        {
                            mySQLResult.Add(myReader[i].ToString());
                        }
                    }
                    return mySQLResult;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }
            }
        }

        /// <summary>
        /// Edits in db by a query
        /// </summary>
        /// <param name="commandLine">Query line</param>
        public void EditSQL(string commandLine)
        {
            myConnection.ConnectionString = connectionString;
            SqlDataReader myReader = null;

            try
            {
                myConnection.Open();
                command.Connection = myConnection;

                command.CommandText = commandLine;
                myReader = command.ExecuteReader();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (myReader != null)
                {
                    myReader.Close();
                }
                if (myConnection != null)
                {
                    myConnection.Close();
                }
            }
        }
    }
}