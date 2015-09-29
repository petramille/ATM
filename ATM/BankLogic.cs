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

        public string LogIn(string ssn, int pin)
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
            if (Convert.ToInt32(tmpCustomer[2]) == 3)
            {
                Session["name"] = (string)Session[tmpCustomer[0]];
                Session["ssn"] = (string)Session[tmpCustomer[1]];
                //myController.StoreHistory();
                return "Ok";
            }
            else
            {
                string errorMessage = myErrorHandler.LogInAlarms(Convert.ToInt32(tmpCustomer[2]));
                //myController.StoreHistory();
                return errorMessage; 
            }
        }



        public void GetAccountsById()
        {
            //Gets all account names associated with the specifc person id, calls GetAccounts in DbController
        }

        public void GetAccount()
        {
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