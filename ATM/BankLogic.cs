using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class BankLogic
    {
        
        

        public void LogIn(string cardNumber, int pin)
        {
            //Checks if valid login. Gets user details and account numbers from database. Creates user and saves in session.
            //Checks number of login attempt
            //FindUser()
            
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