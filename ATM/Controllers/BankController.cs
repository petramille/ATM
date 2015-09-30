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
            BankLogic bankLogic = new BankLogic();
            string loginStatus = null;
            string sessionState = bankLogic.CheckSessionState();
            AccountList AccountList = new AccountList();

            if (!string.IsNullOrEmpty(error))
            {
                List<string> getAccounts = bankLogic.GetAccountsById(error);

                if (getAccounts[0] == "false")
                {
                    return this.RedirectToAction("Error", "Bank", new { error = getAccounts[1] });
                }
                else
                {
                    foreach (var account in getAccounts)
                    {
                        AccountList.account.Add(account.ToString());
                    }
                }

                return View(AccountList);
            }
            else if (!string.IsNullOrEmpty(sessionState))
            {
                List<string> getAccounts = bankLogic.GetAccountsById(sessionState);

                if (getAccounts[0] == "false")
                {
                    return this.RedirectToAction("Error", "Bank", new { error = getAccounts[1] });
                }
                else
                {
                    foreach (var account in getAccounts)
                    {
                        AccountList.account.Add(account.ToString());
                    }
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
            BankLogic bankLogic = new BankLogic();
            AccountList AccountList = new AccountList();
            string loginStatus = null;

            if (!string.IsNullOrEmpty(SSN) && !string.IsNullOrEmpty(pin))
            {
                loginStatus = bankLogic.LogIn(SSN, pin);

                if (loginStatus == "Ok")
                {
                    List<string> getAccounts = bankLogic.GetAccountsById(SSN);

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
            BankLogic bankLogic = new BankLogic();

            string sessionState = bankLogic.CheckSessionState();

            if (!string.IsNullOrEmpty(sessionState))
            {
                List<string> getAccounts = bankLogic.GetAccountsById(sessionState);

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
                BankLogic bankLogic = new BankLogic();

                List<string> accountHistory = bankLogic.GetAccountInformation(accountNumber, 5);

                if (accountHistory[0] == "false")
                {
                    return this.RedirectToAction("Error", "Bank", new { error = accountHistory[1] });
                }
                else
                {
                    AccountInformation accountInfo = new AccountInformation();

                    for (int i = 1; i < accountHistory.Count; i++)
                    {
                        accountInfo.entry.Add(accountHistory[i]);
                    }

                    accountInfo.account = accountHistory[0];

                    return View(accountInfo);
                }
            }
            else
            {
                return this.RedirectToAction("Index", "Bank");
            }
        }

        // GET: Withdrawal
        public ActionResult Withdrawal()
        {
            BankLogic bankLogic = new BankLogic();

            string sessionState = bankLogic.CheckSessionState();

            if (!string.IsNullOrEmpty(sessionState))
            {
                List<string> getAccounts = bankLogic.GetAccountsById(sessionState);

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
                ViewBag.account = accountNumber;
                return View();
            }
            else
            {
                return this.RedirectToAction("Index", "Bank");
            }
        }

        // GET: Landing
        public ActionResult Landing(string quantity, string account)
        {
            BankLogic bankLogic = new BankLogic();

            int sum = int.Parse(quantity);

            List<string> message = bankLogic.WithdrawFromAccount(sum, account);

            if (message[0] == "Ok")
            {
                WithdrawalConfirmation confirm = new WithdrawalConfirmation();

                confirm.sum = quantity;
                confirm.account = account;
                confirm.value100 = message[1];
                confirm.value200 = message[2];
                confirm.value500 = message[3];
                confirm.value1000 = message[4];

                return View(confirm);
            }
            else
            {
                return this.RedirectToAction("Error", "Bank", new { error = message[0] });
            }
        }

        // GET: Error
        public ActionResult Error(string error)
        {
            Error errorContainer = new Error();

            errorContainer.errorMessage = error;

            return View(errorContainer);
        }

    }
}