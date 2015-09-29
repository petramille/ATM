using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ATM.Models;

namespace ATM.Controllers
{
    public class BankController : Controller
    {
        //// GET: Bank
        //public ActionResult Index()
        //{
        //    BankLogic loginHandler = new BankLogic();

        //    string sessionState = loginHandler.CheckSessionState();

        //    if (!string.IsNullOrEmpty(sessionState))
        //    {
        //        List<string> getAccounts = loginHandler.GetAccountsById(sessionState);

        //        AccountList AccountList = new AccountList();

        //        foreach (var account in getAccounts)
        //        {
        //            AccountList.account.Add(account.ToString());
        //        }

        //        return View(AccountList);
        //    }
        //    else
        //    {
        //        string loginStatus = "Session is invalid, please try again.";
        //        return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
        //    }
        //}

        // POST: Bank
        public ActionResult Index(string error)
        {
            BankLogic loginHandler = new BankLogic();
            Error errorContainer = new Error();
            string loginStatus = null;
            string sessionState = loginHandler.CheckSessionState();
            AccountList AccountList = new AccountList();

            if (!string.IsNullOrEmpty(error))
            {
                List<string> getAccounts = loginHandler.GetAccountsById(error);

                foreach (var account in getAccounts)
                {
                    AccountList.account.Add(account.ToString());
                }

                return View(AccountList);
            }
            else if (!string.IsNullOrEmpty(sessionState))
            {
                List<string> getAccounts = loginHandler.GetAccountsById(sessionState);

                foreach (var account in getAccounts)
                {
                    AccountList.account.Add(account.ToString());
                }

                return View(AccountList);
            }
            else
            {
                if (string.IsNullOrEmpty(loginStatus))
                {
                    loginStatus = "Unhandled error";
                }
                return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
            }
        }

        // POST: Bank
        [HttpPost]
        public ActionResult Index(string SSN, string pin)
        {
            BankLogic loginHandler = new BankLogic();
            Error errorContainer = new Error();
            AccountList AccountList = new AccountList();
            string loginStatus = null;

            if (!string.IsNullOrEmpty(SSN) && !string.IsNullOrEmpty(pin))
            {
                loginStatus = loginHandler.LogIn(SSN, pin);

                if (loginStatus == "Ok")
                {
                    List<string> getAccounts = loginHandler.GetAccountsById(SSN);

                    foreach (var account in getAccounts)
                    {
                        AccountList.account.Add(account.ToString());
                    }

                    return View(AccountList);
                }
                else
                {
                    if (string.IsNullOrEmpty(loginStatus))
                    {
                        loginStatus = "Unhandled error";
                    }
                    return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
                }
            }
            else
            {
                return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
            }
        }

        // GET: History
        public ActionResult History()
        {
            BankLogic loginHandler = new BankLogic();

            string sessionState = loginHandler.CheckSessionState();

            if (!string.IsNullOrEmpty(sessionState))
            {
                List<string> getAccounts = loginHandler.GetAccountsById(sessionState);

                AccountList AccountList = new AccountList();

                foreach (var account in getAccounts)
                {
                    AccountList.account.Add(account.ToString());
                }

                return this.RedirectToAction("Index", "Bank", new { error = sessionState });
            }
            else
            {
                string loginStatus = "Session is invalid, please try again.";
                return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
            }
        }

        // POST: History
        [HttpPost]
        public ActionResult History(string accountNumber)
        {
            if (!string.IsNullOrEmpty(accountNumber))
            {
                return View();
            }
            else
            {
                return this.RedirectToAction("Index", "Bank");
            }
        }

        // GET: Withdrawal
        public ActionResult Withdrawal()
        {
            BankLogic loginHandler = new BankLogic();

            string sessionState = loginHandler.CheckSessionState();

            if (!string.IsNullOrEmpty(sessionState))
            {
                List<string> getAccounts = loginHandler.GetAccountsById(sessionState);

                AccountList AccountList = new AccountList();

                foreach (var account in getAccounts)
                {
                    AccountList.account.Add(account.ToString());
                }

                return this.RedirectToAction("Index", "Bank", new { error = sessionState });
            }
            else
            {
                string loginStatus = "Session is invalid, please try again.";
                return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
            }
        }

        // GET: Withdrawal
        [HttpPost]
        public ActionResult Withdrawal(string accountNumber)
        {
            if (!string.IsNullOrEmpty(accountNumber))
            {
                return View();
            }
            else
            {
                return this.RedirectToAction("Index", "Bank");
            }
        }

        // GET: Error
        public ActionResult Error(string error)
        {
            Error errorContainer = new Error();

            errorContainer.errorMessage = error;

            return View(errorContainer);
        }

        // GET: Landing
        public ActionResult Landing(string quantity)
        {
            BankLogic withdrawal = new BankLogic();

            int sum = int.Parse(quantity);

            withdrawal.WithdrawFromAccount(sum);

            return View();
        }
    }
}