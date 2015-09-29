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
        public double Balance { get; }
        public string Currency { get; set; }



        public Account(string accountType, string accountAlias, string accountNumber, double balance, string currency)
        {
            AccountType = accountType;
            AccountAlias = accountAlias;
            AccountNumber = accountNumber;
            Balance = balance;
            Currency = currency;
        }

        public virtual string WithdrawMoney(double amountToWithdraw, double balance)
        {
            return "";
        }
    }
}