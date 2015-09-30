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
            //bills[0] is number of 100 kr bills, bill[1] 200 kr etc, bills[4] is number of receipts
            Session["numberOfBills"] = bills; 
            //Session["receipts"] = bills[4];          

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
        public Account GetAccount(string alias_accountNr)
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
                    return account;
                    
               
                default:
                    account = new Account(accountType, alias, accountNumber, balance, currency);
                    return account;
                    
            }
        }

        private double CalculateAmountLeftToday(string accountNumber)
        {

            string commandLine = $"SELECT HandledAmount FROM ActivityLog where EventTime > '{DateTime.Today}' and EventType = 'Withdraw' And Account='{accountNumber}'";

            List<string> amountValues = myController.readSingleColumnFromSQL(commandLine);

            double totalWithdrawnAmount = 0;
            double withdrawnAmount = 0;

            if (amountValues != null)
            {
                foreach (var value in amountValues)
                {
                    withdrawnAmount = Convert.ToDouble(value);
                    totalWithdrawnAmount += withdrawnAmount;

                }
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


        //Discuss the string that is returned. Maybe now it is supposed to be "ok" at some point when evaluated in the controller!!!
        public string WithdrawFromAccount(int amount, string alias_accountNr)
        {

            Account myAccount = GetAccount(alias_accountNr);
            string ssn = (string)Session["ssn"];

            bool insertedAmountCorrect = CheckInsertedAmount(amount);

            if (!insertedAmountCorrect)
            {
                myAccount.Equals(null);
                return "The requested amount of money must be even 100 SEK";
            }

            if (myAccount.WithdrawMoney(amount) == "Ok")
            {
                string transferCompleted = myController.WithdrawFromAccount(myAccount.AccountNumber, ssn, amount);
                
                if (transferCompleted.Equals("1"))
                {
                    string result = TransferBills(amount);
                    if (result == "false")
                    {
                        myAccount.Equals(null);
                        return "That combination of bills does not exist, try another amount";
                    }


                    DateTime presentTime = DateTime.Now;
                    myController.StoreHistory(presentTime, myAccount.AccountType, ssn, myAccount.AccountNumber, amount);
                    myAccount.Equals(null);
                    return result;
                    //Result shows number of the different bills
                }
                else
                {
                    myAccount.Equals(null);
                    return "Withdrawal of that amount was not possible";
                }                
            }
            else
            {
                string transferOk = myAccount.WithdrawMoney(amount);
                myAccount.Equals(null);
                return transferOk;
            }
            //string resultMessage = account.WithDrawMoney();
            //StoreHistory();
        }


        public bool CheckInsertedAmount(int amount)
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



        public List<string> GetAccountInformation(string alias_accountNr, int amountOfLines)
        {
            //GetAccountBalance(accountNumber);
            //GetAccountHistory(accountNumber);
            Account myAccount = GetAccount(alias_accountNr);
            List<string> accountInformation = new List<string>();
            accountInformation.Add(myAccount.AccountAlias + ", AccountNumber: " + myAccount.AccountNumber + ", Balance: " + myAccount.Balance + ", Currency: " + myAccount.Currency);
            
            string commandLine = $"SELECT Top '{amountOfLines}' EventTime, EventType, HandledAmount FROM ActivityLog where EventType = 'Withdraw' And Account='{myAccount.AccountNumber}' Order by EventTime DESC";

            //addRange kanske strular??
            accountInformation.AddRange(myController.readFromSQL(commandLine));
            myAccount.Equals(null);
            return accountInformation;
        }

        public string TransferBills(int amount)
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

            int[] result = GetBillsWithdrawn(numberOf1000, amount, 1000);
            withdrawed1000 = result[0];
            amount = result[1];

            result = GetBillsWithdrawn(numberOf500, amount, 500);
            withdrawed500 = result[0];
            amount = result[1];

            result = GetBillsWithdrawn(numberOf200, amount, 200);
            withdrawed200 = result[0];
            amount = result[1];

            result = GetBillsWithdrawn(numberOf100, amount, 100);
            withdrawed100 = result[0];
            amount = result[1];

            if (amount>0)
            {
                return "false";
                }
                else
                {
                //Do: Calls method that contains SP in database to update number of bills

                return $"You have withdrawn {withdrawed1000} 1000 SEK bills, {withdrawed500} 500 SEK bills, {withdrawed200} 200 SEK bills, {withdrawed100} 100 SEK bills";
                }
            }

        private int[] GetBillsWithdrawn(int numberOfBillsInATM, int amount, int typeOfBill)
                {
            int[] result = new int[2];

            int withdrawn;
            int amountOfBillsNeeded = amount / typeOfBill;
            if (numberOfBillsInATM >= amountOfBillsNeeded)
            {
                withdrawn = amountOfBillsNeeded;
                amount -= withdrawn;
                }
            else
            {
                withdrawn = numberOfBillsInATM;
                amount -= withdrawn;
            }
            result[0] = withdrawn;
            result[1] = amount;
            return result;
        }

    }


}