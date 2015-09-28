using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class SavingsAccount : Account
    {
        
        public double WithdrawAmountLeftToday { get; }

        public SavingsAccount(string accountType, string accountAlias, string accountNumber, double balance, double withdrawAmountLeftToday) : base (accountType, accountAlias, accountNumber, balance)
        {
            WithdrawAmountLeftToday = withdrawAmountLeftToday;
        }

        public override string WithdrawMoney(double amountToWithdraw, double balance)
        {
            if (amountToWithdraw > 5000)
            {

            }
            else if (amountToWithdraw > balance)
            {

            }
            else if (WithdrawAmountLeftToday - amountToWithdraw < 0)
            {

            }
            else
            {

            }

            return "";

        }
    }
}