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

        public void LogIn(string ssn, int pin)
        {
            //Checks if valid login. Gets user details and account numbers from database. 
            //Checks number of login attempt
            List<string> tmpCustomer = new List<string>();
            tmpCustomer = myController.FindUser(ssn, pin);
            if (Convert.ToInt32(tmpCustomer[2]) == 3)
            {
                Session["name"] = (string)Session[tmpCustomer[0]];
            }
            else if (Convert.ToInt32(tmpCustomer[2]) == 2)
            {

            }
            else if (Convert.ToInt32(tmpCustomer[2]) == 1)
            {

            }
            else if (Convert.ToInt32(tmpCustomer[2]) == 0)
            {

            }
            // StoreHistory()
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