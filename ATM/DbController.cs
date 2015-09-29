using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ATM
{
    public class DbController
    {
        static string connectionString = @"Server=tcp:igru9irx7p.database.windows.net,1433;Database=ATM;User ID=ATM-Admin@igru9irx7p;Password=Qwerty123!;Trusted_Connection=False;Encrypt=True;Connection Timeout=30";
        SqlConnection myConnection = new SqlConnection();
        SqlCommand command = new SqlCommand();
        ErrorHandler myErrorHandler = new ErrorHandler();


        //return List<string>
        public List<string> FindUser(string ssn, string pin)
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
                //command.Parameters.Clear();

                command.Parameters.Add("@Username", System.Data.SqlDbType.VarChar, 12);
                command.Parameters.Add("@Password", System.Data.SqlDbType.VarChar, 50);
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
            catch (Exception x)
            {
                myErrorHandler.HandleErrorMessage(x.ToString());
                return null;
            }

           
        }


        public void StoreHistory(string eventType, int id, string accountNumber, string ipNumber, double transactionAmount)
        {
            //Sends event details to db for logging
            //sp?
        }

        
       

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
            catch (Exception ex)
            {
                myErrorHandler.HandleErrorMessage("No connection found");
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
                while (myReader.Read())
                {
                    for (int i = 0; i < myReader.FieldCount; i++)
                    {
                        mySQLResult.Add(myReader[i].ToString());
                    }
                }

                return mySQLResult;

            }
            catch (Exception ex)
            {
                myErrorHandler.HandleErrorMessage("No connection found");
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
    }
}