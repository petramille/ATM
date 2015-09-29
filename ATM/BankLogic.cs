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

        public string LogIn(string ssn, string pin)
        {
            //This code shuld be run before the person logs into the system
            //Check if ATM is out of service or if maintenance is on-going
            myErrorHandler.HandleErrorMessage("The ATM is out of service");
            int atmId = 0;
            bool billsLeft = myController.GetAmountOfBills(atmId);

            if (!billsLeft)
            {
                myErrorHandler.HandleErrorMessage("The ATM is out of banknotes.");
            }

            bool receiptLeft = myController.GetReceiptLeft(atmId);

            if (!receiptLeft)
            {
                myErrorHandler.HandleErrorMessage("No receipts can be given");
            }
            //End

            //Checks if valid login. Gets user details and account numbers from database. 
            //Checks number of login attempt
            List<string> tmpCustomer = new List<string>();
            tmpCustomer = myController.FindUser(ssn, pin);
            if ((tmpCustomer[2]) == "3")
            {
                Session["ssn"] = (string)Session[tmpCustomer[0]];
                Session["name"] = (string)Session[tmpCustomer[1]];
                
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
            string commandLine= $"SELECT Alias, AccountNR From Account, Controller  where  Controller.SSN = {ssn} And Controller.AccountNR=Account.AccountNR";
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

            string commandLine = $"SELECT Alias, Currency, Amount, AccountType From Account where AccountNR= {accountNumber}";
            //return myController.readFromSQL(commandLine);
            
            
            //Gets details about specific account, calls GetAccount in DbController
            //switch (accountType)
            //{
            //case "saving":
            //double withDrawmoneyLeftToday = CalculateAmountLeftToday(accountNumber);
            // SavingAccount account = new SavingAccount();

            //    default:
            //Account account = new Account();
            //        break;
            //}


        }
        //test 

        public void WithdrawFromAccount()
        {
            //string resultMessage = account.WithDrawMoney();
            //StoreHistory();

        }

        public void GetAccountInformation()
        {
            //GetAccountBalance(accountNumber);
            //GetAccountHistory(accountNumber);
        }

        

    }


}