using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class Account
    {
       
       

        public string AccountType { get; }
        public string AccountAlias { get; }
        public string AccountNumber { get; }
        public double Balance { get; set; }
        public string Currency { get; set; }



        public Account(string accountType, string accountAlias, string accountNumber, double balance, string currency)
        {
            AccountType = accountType;
            AccountAlias = accountAlias;
            AccountNumber = accountNumber;
            Balance = balance;
            Currency = currency;
        }

        /// <summary>
        /// Checks if it is possible to withdraw from an account
        /// </summary>
        /// <param name="amountToWithdraw"> the amount to withdraw</param>
        /// <returns>Ok if it is possible to withdraw. else a message string with what went wrong</returns>
        public virtual string WithdrawMoney(double amountToWithdraw)
        {
            if (amountToWithdraw > Balance)
            {
                return "There is not enough money on the account";
            }
            else
            {
                return "Ok";
            }
            
        }
    }
}