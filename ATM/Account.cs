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
       

        public Account(string accountType, string accountAlias, string accountNumber, double balance)
        {
            AccountType = accountType;
            AccountAlias = accountAlias;
            AccountNumber = accountNumber;
            Balance = balance;
        }

        public virtual void WithdrawMoney()
        {

        }
    }
}