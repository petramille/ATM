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
        // GET: Bank
        public ActionResult Index()
        {
            BankLogic loginHandler = new BankLogic();

            // Change to string SSN instead of bool
            string sessionState = loginHandler.CheckSessionState();

            if (!string.IsNullOrEmpty(sessionState))
            {
                List<string> getAccounts = loginHandler.GetAccountsById(Session["ssn"].ToString());

                AccountList AccountList = new AccountList();

                foreach (var account in getAccounts)
                {
                    AccountList.account.Add(account.ToString());
                }

                return View(AccountList);
            }
            else
            {
                string loginStatus = "Session is invalid, please try again.";
                return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
            }
        }

        // POST: Bank
        [HttpPost]
        public ActionResult Index(string SSN, string pin)
        {
            BankLogic loginHandler = new BankLogic();
            Error errorContainer = new Error();
            string loginStatus = null;

            if (!string.IsNullOrEmpty(SSN) && !string.IsNullOrEmpty(pin))
            {
                loginStatus = loginHandler.LogIn(SSN, pin);

                if (loginStatus == "Ok")
                {
                    List<string> getAccounts = loginHandler.GetAccountsById(SSN);

                    AccountList AccountList = new AccountList();

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
            return View();
        }

        // GET: Withdrawal
        public ActionResult Withdrawal()
        {
            return View();
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