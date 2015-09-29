using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ATM
{
    public class BankLogic : Page
    {

        DbController myController = new DbController();
        ErrorHandler myErrorHandler = new ErrorHandler();

        public void CheckATMStatus(int atmId)
        {
            string commandLine = $"SELECT UnitStatus From Unit where Id = '{atmId}'";

            List<string> status = myController.readSingleColumnFromSQL(commandLine);

            if (status[0] == "0")
            {
                myErrorHandler.HandleErrorMessage("Ok");
            }
            else if (status[0] == "1")
            {
                myErrorHandler.HandleErrorMessage("The ATM is out of service");
            }
            else if (status[0] == "2")
            {
                myErrorHandler.HandleErrorMessage("The system is out of service. Maintenance on-going");
            }

            commandLine = $"SELECT bills100, bills200, bills500, bills1000, Receipt From Unit where Id = '{atmId}'";

            List<string> bills = myController.readSingleColumnFromSQL(commandLine);

            if (bills[0] == "0" && bills[1] == "0" && bills[2] == "0" && bills[3] == "0")
            {
               myErrorHandler.HandleErrorMessage("The ATM is out of money.");

            }
            Session["receipts"] = (string)Session[bills[4]];          

        }


        public string LogIn(string ssn, string pin)
        {


            //Checks if valid login. Gets user details and account numbers from database. 
            //Checks number of login attempt
            List<string> tmpCustomer = new List<string>();
            tmpCustomer = myController.FindUser(ssn, pin);

            if ((tmpCustomer[2]) == "3")
            {
                Session["ssn"] = tmpCustomer[0];
                Session["name"] = tmpCustomer[1];

                //myController.StoreHistory();
                return "Ok";
            }
            else
            {
                string errorMessage = myErrorHandler.LogInAlarms(tmpCustomer[2]);
                //myController.StoreHistory();
                return errorMessage;
            }
        }



        public List<string> GetAccountsById(string ssn)
        {
            //Gets all accountalias and accountnumber associated with the specifc person id, calls GetAccounts in 
            string commandLine = $"SELECT Alias, Account.AccountNR From Account, Controller  where  Controller.SSN = '{ssn}' And Controller.AccountNR=Account.AccountNR";
            return myController.readFromSQL(commandLine);

        }

        /// <summary>
        /// Splittar först raden som står i listan för att få ut kontonumret. Hämtar sedan
        /// information från kontot och skapar kontot för att kunna få specifik logik.
        /// </summary>
        /// <param name="alias_accountNr">Tar raden som står i listan av konton</param>
        public void GetAccount(string alias_accountNr)
        {
            string[] splittedLine = alias_accountNr.Split(' ');
            string accountNumber = splittedLine[1];

            string commandLine = $"SELECT Alias, Currency, Amount, AccountType From Account where AccountNR= '{accountNumber}'";

            List<string> accountDetails = myController.readSingleColumnFromSQL(commandLine);

            string alias = accountDetails[0];
            string currency = accountDetails[1];
            double balance = Convert.ToDouble(accountDetails[2]);
            string accountType = accountDetails[3];
            Account account;

            switch (accountType)
            {
                case "1":
                    double withDrawmoneyLeftToday = CalculateAmountLeftToday(accountNumber);
                    account = new SavingsAccount(accountType, alias, accountNumber, balance, currency, withDrawmoneyLeftToday);
                    Session.Add("account", account);
                    break;
               
                default:
                    account = new Account(accountType, alias, accountNumber, balance, currency);
                    Session.Add("account", account);
                    break;
            }


        }

        private double CalculateAmountLeftToday(string accountNumber)
        {
            string commandLine = $"SELECT HandledAmount From ActivityLog where EventType = 'Withdraw' And Account='{accountNumber}'";

            List<string> amountValues = myController.readSingleColumnFromSQL(commandLine);

            double totalWithdrawnAmount = 0;
            double withdrawnAmount = 0;

            foreach (var value in amountValues)
            {
                withdrawnAmount = Convert.ToDouble(value);
                totalWithdrawnAmount += withdrawnAmount;

            }
            return (5000 - totalWithdrawnAmount);
        }

        

        public string CheckSessionState()
        {
            if (Session["name"] != null && Session["ssn"] != null)
            {
                return (string)Session["ssn"];
            }
            else
            {
                return null;
            }
                
        }

        public string WithdrawFromAccount(int amount)
        {
            Account myAccount = (Account)Session["account"];

            if (myAccount.WithdrawMoney(amount)=="Ok")
            {
                //string commandLine = $"SELECT HandledAmount From ActivityLog where EventType = 'Withdraw' And Account='{accountNumber}'";
                return "Ok";
            }

            //string resultMessage = account.WithDrawMoney();
            //StoreHistory();
            return "";
        }

        public void GetAccountInformation()
        {
            //GetAccountBalance(accountNumber);
            //GetAccountHistory(accountNumber);
        }



    }


}