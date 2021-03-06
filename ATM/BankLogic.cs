﻿using System;
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


        /// <summary>
        /// First method to be called before login to the person account. Sends messages
        ///  to the web page describing the status of the system and the ATM or if the ATM is out of money.
        /// To add: Write to page if the ATM is out of receipts
        /// </summary>
        /// <param name="atmId">The unique ID of the ATM</param>
        public List<string> CheckATMStatus(string atmId)
        {
            string commandLine = $"SELECT UnitStatus From Unit where Id = '{atmId}'";

            List<string> status = myController.readSingleColumnFromSQL(commandLine);
            if (status == null)
            {
                status.Add("false");
                status.Add("No access to the ATM at the moment");
                return status;
            }

            List<string> message = new List<string>();

            if (status[0] == "0")
            {
                message.Add("Ok");
            }
            else if (status[0] == "1")
            {
                message.Add("false");
                message.Add("The ATM is out of service");
            }
            else if (status[0] == "2")
            {
                message.Add("false");
                message.Add("The system is out of service. Maintenance on-going");

            }

            commandLine = $"SELECT bills100, bills200, bills500, bills1000, Receipt From Unit where Id = '{atmId}'";

            List<string> bills = myController.readSingleColumnFromSQL(commandLine);

            if (bills[0] == "0" && bills[1] == "0" && bills[2] == "0" && bills[3] == "0")
            {
                message.Add("false");
                message.Add("The ATM is out of money.");
            }
            //bills[0] is number of 100 kr bills, bill[1] 200 kr etc, bills[4] is number of receipts
            Session["numberOfBills"] = bills;
            //Session["receipts"] = bills[4];          
            return message;
        }


        /// <summary>
        /// Checks if valid login. Checks number of login attempt.
        /// Saves ssn and the name of the person in a session.
        /// </summary>
        /// <param name="ssn">The SSN of the person who logs in</param>
        /// <param name="pin">Pin code</param>
        /// <returns>Error message describing the number of login tries left or "ok" if login was successful.</returns>
        public List<string> LogIn(string ssn, string pin)
        {

            List<string> tmpCustomer = new List<string>();
            tmpCustomer = myController.FindUser(ssn, pin);

            List<string> tmpList = new List<string>();
            if (tmpCustomer == null)
            {
                tmpList.Add("false");
                tmpList.Add("No access to the ATM at the moment");
            }

            if ((tmpCustomer[2]) == "3")
            {
                Session["ssn"] = tmpCustomer[0];
                Session["name"] = tmpCustomer[1];

                //myController.StoreHistory();
                tmpList.Add("Ok");
            }
            else
            {
                string errorMessage = myErrorHandler.LogInAlarms(tmpCustomer[2]);
                tmpList.Add("false");
                tmpList.Add(errorMessage);
                //myController.StoreHistory();

            }
            return tmpList;
        }


        /// <summary>
        /// Gets all account alias and account numbers associated with the specific person SSN using a SQL query.
        /// </summary>
        /// <param name="ssn">Person SSN</param>
        /// <returns>A list with alias and account numbers for the person logged in</returns>
        public List<string> GetAccountsById(string ssn)
        {
            string commandLine = $"SELECT Alias, Account.AccountNR From Account, Controller  where  Controller.SSN = '{ssn}' And Controller.AccountNR=Account.AccountNR";
            List<string> tmpAccounts = myController.readFromSQL(commandLine);
            if (tmpAccounts == null)
            {
                tmpAccounts.Add("false");
                tmpAccounts.Add("No access to the ATM at the moment");
            }

            return tmpAccounts;

        }

        /// <summary>
        /// The row containing the alias and account number is splitted when ' '.
        /// SQL query gets all account details associated with the specific account.
        /// Creates an instance of an Account object.
        /// </summary>
        /// <param name="alias_accountNr">A string containing both the alias and the account number of the selected account</param>
        /// <returns>The Account containing all account details</returns>
        private Account GetAccount(string alias_accountNr)
        {
            string[] splittedLine = alias_accountNr.Split(' ');
            string accountNumber = splittedLine[1];

            string commandLine = $"SELECT Alias, Currency, Amount, AccountType From Account where AccountNR= '{accountNumber}'";

            List<string> accountDetails = myController.readSingleColumnFromSQL(commandLine);

            if (accountDetails == null)
            {
                //connection failed
                return null;
            }

            string alias = accountDetails[0];
            string currency = accountDetails[1];
            double balance = Convert.ToDouble(accountDetails[2]);
            string accountType = accountDetails[3];
            Account account;

            switch (accountType)
            {
                case "1":
                    double withDrawmoneyLeftToday = CalculateAmountLeftToday(accountNumber);
                    if (withDrawmoneyLeftToday == (-1))
                    {
                        //cant withdraw more than 500 a day
                        return null;
                    }

                    account = new SavingsAccount(accountType, alias, accountNumber, balance, currency, withDrawmoneyLeftToday);
                    return account;


                default:
                    account = new Account(accountType, alias, accountNumber, balance, currency);
                    return account;

            }
        }

        /// <summary>
        /// Checks the amount of money withdrawn during that day using a SQL query.
        /// </summary>
        /// <param name="accountNumber">The number of account to be checked</param>
        /// <returns>The amount that still can be withdrawn during that day</returns>
        private int CalculateAmountLeftToday(string accountNumber)
        {
            string commandLine = $"SELECT HandledAmount FROM ActivityLog where EventTime > '{DateTime.Today}' and EventType = 'Withdraw_success' And AccountNR='{accountNumber}'";

            List<string> amountValues = myController.readSingleColumnFromSQL(commandLine);

            if (amountValues == null)
            {
                // connection not possible
                return -1;
            }

            int totalWithdrawnAmount = 0;
            int withdrawnAmount = 0;

            if (amountValues != null)
            {
                foreach (var value in amountValues)
                {
                    withdrawnAmount = Convert.ToInt32(value);
                    totalWithdrawnAmount += withdrawnAmount;

                }
            }
            return (5000 - totalWithdrawnAmount);
        }


        /// <summary>
        /// Checks if sessions containing the name and SSN of the person are valid
        /// </summary>
        /// <returns>The SSN for the person who is logged in</returns>
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


        /// <summary>
        /// Calls the GetAccount method to create an instance of the present Account. 
        /// Calls the WithdrawMoney method in Account class to check if the withdrawal is possible. Gets ok or false back together with a message.
        /// If the amount is withdrawn, StoreHistory is called to log the event.
        /// </summary>
        /// <param name="amount">The amount the person wants to withdraw</param>
        /// <param name="alias_accountNr">The account alias and the account number combined into one string</param>
        /// <returns>A list where the first position is "ok" if it is possible to withdraw money and "false" if it is ot.
       ///  In the "false" case, the second position contains the error message to be displayed.</returns>
        public List<string> WithdrawFromAccount(int amount, string alias_accountNr)
        {
            List<string> result = new List<string>();

            Account myAccount = GetAccount(alias_accountNr);

            if (myAccount == null)
            {
                result.Add("false");
                result.Add("Technical Error");
                return result;
            }

            string ssn = (string)Session["ssn"];



            bool insertedAmountCorrect = CheckInsertedAmount(amount);

            if (!insertedAmountCorrect)
            {
                myAccount.Equals(null);
                result.Add("false");
                result.Add("The requested amount of money must be an even 100 SEK");
                return result;
            }

            if (myAccount.WithdrawMoney(amount) == "Ok")
            {
                result = TransferBills(amount);
                if (result[0] == "false")
                {
                    myAccount.Equals(null);

                    result.Add("That combination of bills does not exist, try another amount");
                    return result;
                }

                string transferCompleted = myController.WithdrawFromAccount(myAccount.AccountNumber, ssn, amount);
                if (transferCompleted == null)
                {
                    result.Add("false");
                    result.Add("Withdrawal was not possible");
                    return result;
                }

                else if (transferCompleted.Equals("1"))
                {
                    DateTime presentTime = DateTime.Now;
                    //myController.StoreHistory(myAccount.AccountType, ssn, myAccount.AccountNumber, amount);
                    myAccount.Equals(null);
                    return result;
                    //Result shows number of the different bills
                }
                else
                {
                    myAccount.Equals(null);
                    result.Add("false");
                    result.Add("Withdrawal of that amount was not possible");
                    return result;
                }
            }
            else
            {
                string transferOk = myAccount.WithdrawMoney(amount);
                myAccount.Equals(null);
                result.Add("false");
                result.Add(transferOk);
                return result;
            }
            //string resultMessage = account.WithDrawMoney();
            //StoreHistory();
        }


        /// <summary>
        /// Checks if the requested amount can be delivered in even 100 kr bills.
        /// </summary>
        /// <param name="amount">The amount to be withdrawn</param>
        /// <returns>True if the amount is ok, else false</returns>
        private bool CheckInsertedAmount(int amount)
        {
            if (amount % 100 == 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }


        /// <summary>
        /// New instance of Account is created by calling GetAccount to get the account details needed. 
        /// A SQL query gets the events in the activity log.
        /// </summary>
        /// <param name="alias_accountNr">Both the account alias and the account number in one string</param>
        /// <param name="amountOfLines">The number of lines that will be displayed on the page/the receipt</param>
        /// <returns>List where the first position tells if the information was successfully delivered ("ok"/"false".
        /// The second position is an error message if false and information about the activities if "ok"</returns>
        public List<string> GetAccountInformation(string alias_accountNr, int amountOfLines)
        {
            List<string> accountInformation = new List<string>();
            Account myAccount = GetAccount(alias_accountNr);

            if (myAccount == null)
            {
                accountInformation.Add("false");
                accountInformation.Add("Technical Error");
                return accountInformation;
            }

            accountInformation.Add(myAccount.AccountAlias + " - " + myAccount.AccountNumber + " - Balance: " + myAccount.Balance + " " + myAccount.Currency);

            string commandLine = $"SELECT Top {amountOfLines} EventTime, EventType, HandledAmount FROM ActivityLog where EventType = 'Withdraw_success' And AccountNR='{myAccount.AccountNumber}' Order by EventTime DESC";
            List<string> informationRows = myController.readFromSQL(commandLine);

            if (informationRows == null)
            {
                accountInformation[0] = "false";
                accountInformation.Add("No access to the ATM at the moment");

            }

            string tmpString;
            accountInformation.Add("Ok");
            foreach (var infoRow in informationRows)
            {
                tmpString = infoRow.Replace("Withdraw_success", "Withdraw");
                tmpString += " SEK";
                accountInformation.Add(tmpString);

            }
            myAccount.Equals(null);
            return accountInformation;
        }

        /// <summary>
        ///  Calls the GetBillsWithdrawn method to get the amount of bills to b withdrawn.
        /// Calls the UpdateNumberOfBills method in Db controller to communicate the number of bills withdrawn.
        /// </summary>
        /// <param name="amount">The amount to be withdrawn from account</param>
        /// <returns>List containing "ok" or "false" at the first position and the number of teh different bill types that have been withdrawn</returns>
        public List<string> TransferBills(int amount)
        {
            List<string> bills = (List<string>)Session["numberOfBills"];
            int numberOf100 = Convert.ToInt32(bills[0]);
            int numberOf200 = Convert.ToInt32(bills[1]);
            int numberOf500 = Convert.ToInt32(bills[2]);
            int numberOf1000 = Convert.ToInt32(bills[3]);

            int withdrawed1000;
            int withdrawed500;
            int withdrawed200;
            int withdrawed100;

            List<string> result = new List<string>();
            int[] tmpResult = GetBillsWithdrawn(numberOf1000, amount, 1000);
            withdrawed1000 = tmpResult[0];
            amount = tmpResult[1];

            tmpResult = GetBillsWithdrawn(numberOf500, amount, 500);
            withdrawed500 = tmpResult[0];
            amount = tmpResult[1];

            tmpResult = GetBillsWithdrawn(numberOf200, amount, 200);
            withdrawed200 = tmpResult[0];
            amount = tmpResult[1];

            tmpResult = GetBillsWithdrawn(numberOf100, amount, 100);
            withdrawed100 = tmpResult[0];
            amount = tmpResult[1];

            if (amount > 0)
            {
                result.Add("false");
                return result;
            }
            else
            {
                string atmId = (string)Session["ATMID"];
                myController.UpdateNumberOfBills(atmId, withdrawed100, withdrawed200, withdrawed500, withdrawed1000);

                result.Add("Ok");
                result.Add("" + withdrawed100);
                result.Add("" + withdrawed200);
                result.Add("" + withdrawed500);
                result.Add("" + withdrawed1000);
                return result;
                //return $"You have withdrawn {withdrawed1000} 1000 SEK bills, {withdrawed500} 500 SEK bills, {withdrawed200} 200 SEK bills, {withdrawed100} 100 SEK bills";
            }
        }

        /// <summary>
        /// Calculates the number of different bills to be withdrawn.
        /// </summary>
        /// <param name="numberOfBillsInATM">Number of bills of the specific type in the specific ATM</param>
        /// <param name="amount">Amount to be withdrawn</param>
        /// <param name="typeOfBill">Type of bill; 100, 200 etc.</param>
        /// <returns>Array with the number of withdrawn bills of the specific type and the amount wihdrawn of that type</returns>
        private int[] GetBillsWithdrawn(int numberOfBillsInATM, int amount, int typeOfBill)
        {
            int[] result = new int[2];

            int withdrawnBills;
            int amountOfBillsNeeded = amount / typeOfBill;
            if (numberOfBillsInATM >= amountOfBillsNeeded)
            {
                withdrawnBills = amountOfBillsNeeded;
                amount -= withdrawnBills * typeOfBill;
            }
            else
            {
                withdrawnBills = numberOfBillsInATM;
                amount -= withdrawnBills * typeOfBill;
            }
            result[0] = withdrawnBills;
            result[1] = amount;
            return result;
        }

        /// <summary>
        /// Calls Sql query that updates number of receipts in db
        /// </summary>
        /// <param name="lengthOfReceipt">The length of the receipt (1 or 2) to be printed </param>
        /// <param name="atmId">ID of the ATM affected</param>
        public void subtractFromReceipt(int lengthOfReceipt, string atmId)
        {
            string commandLine = $"Update Unit SET Receipt = Receipt - {lengthOfReceipt} where ID = '{atmId}'";
            myController.EditSQL(commandLine);
        }

        /// <summary>
        /// Logs all events that are supposed to be stored in the db
        /// </summary>
        /// <param name="eventType">Type of event being logged</param>
        /// <param name="ssn">SSN of person doing the activity</param>
        /// <param name="accountNumber">Alias + Account number affected</param>
        /// <param name="transactionAmount">Amount to be withdrawn if event type is withdraw</param>
        public void LoggingOfEvents(string eventType, string ssn, string alias_accountNr, double transactionAmount)
        {
            //DateTime dateTimeNow = DateTime.Now;
            string accountNumber;
            //if (alias_accountNr!=null)
            //{
                string[] splittedLine = alias_accountNr.Split(' ');
                accountNumber = splittedLine[1];
            //}
            //else
            //{
            //    accountNumber = null;
            //}
            
            myController.StoreHistory(eventType, ssn, accountNumber, transactionAmount);
        }

    }


}