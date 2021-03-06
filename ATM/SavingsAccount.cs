﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ATM
{
    public class SavingsAccount : Account
    {
        
        public double WithdrawAmountLeftToday { get; }

        public SavingsAccount(string accountType, string accountAlias, string accountNumber, double balance, string currency, double withdrawAmountLeftToday) : base (accountType, accountAlias, accountNumber, balance, currency)
        {
            WithdrawAmountLeftToday = withdrawAmountLeftToday;
        }

        /// <summary>
        /// Checks if it is possible to withdraw from an account
        /// </summary>
        /// <param name="amountToWithdraw"> the amount to withdraw</param>
        /// <returns>Ok if it is possible to withdraw. else a message string with what went wrong</returns>
        public override string WithdrawMoney(double amountToWithdraw)
        {
            if (amountToWithdraw > 5000)
            {
                return "The maximum amount to withdraw is 5000 SEK";
            }
            else if (amountToWithdraw > Balance)
            {
                return "There is not enough money on the account";
            }
            else if (WithdrawAmountLeftToday - amountToWithdraw < 0)
            {
                return "The maximum amount to withdraw per day is 10000 SEK";
            }
            else
            {
                //Balance -= amountToWithdraw;
                return "Ok";
            }

        }
    }
}