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
        // handle if user gets here in other ways
        public ActionResult Index(string error)
        {
            // Create BankLogic and Account List obects
            BankLogic bankLogic = new BankLogic();
            string loginStatus = null;
            string sessionState = bankLogic.CheckSessionState();
            AccountList AccountList = new AccountList();

            // Check if user was redirected
            if (!string.IsNullOrEmpty(error))
            {
                List<string> getAccounts = bankLogic.GetAccountsById(error);

                // Check if user has bankaccounts, else redirect to error page
                if (getAccounts[0] == "false")
                {
                    return this.RedirectToAction("Error", "Bank", new { error = getAccounts[1] });
                }
                else
                {
                    // Get list of accounts
                    foreach (var account in getAccounts)
                    {
                        AccountList.account.Add(account.ToString());
                    }
                }

                return View(AccountList);
            }

            // Check if session is active
            else if (!string.IsNullOrEmpty(sessionState))
            {
                List<string> getAccounts = bankLogic.GetAccountsById(sessionState);

                // Check if user has bankaccounts, else redirect to error page
                if (getAccounts[0] == "false")
                {
                    return this.RedirectToAction("Error", "Bank", new { error = getAccounts[1] });
                }
                else
                {
                    // Get list of accounts
                    foreach (var account in getAccounts)
                    {
                        AccountList.account.Add(account.ToString());
                    }
                }
                return View(AccountList);
            }

            // Handle unexpected errors
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
        // Redirect from log in page
        [HttpPost]
        public ActionResult Index(string SSN, string pin)
        {

            // Create BankLogic and Account List obects
            BankLogic bankLogic = new BankLogic();
            AccountList AccountList = new AccountList();
            List<string> loginStatus = new List<string>();

            // Check log in credentials
            if (!string.IsNullOrEmpty(SSN) && !string.IsNullOrEmpty(pin))
            {
                loginStatus = bankLogic.LogIn(SSN, pin);

                // Check log in status
                if (loginStatus[0] == "Ok")
                {

                    // Get list of accounts
                    List<string> getAccounts = bankLogic.GetAccountsById(SSN);

                    foreach (var account in getAccounts)
                    {
                        AccountList.account.Add(account.ToString());
                    }

                    return View(AccountList);
                }
                else
                {
                    return this.RedirectToAction("Error", "Bank", new { error = loginStatus[1] });
                }
            }
            else
            {
                return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
            }
        }

        // GET: History
        // handle if user gets here in other ways
        public ActionResult History()
        {
            // Create BankLogic objects and check session
            BankLogic bankLogic = new BankLogic();
            string SessionSSN = bankLogic.CheckSessionState();

            // Check if session is active, redirect to index
            if (!string.IsNullOrEmpty(SessionSSN))
            {
                List<string> getAccounts = bankLogic.GetAccountsById(SessionSSN);
                AccountList AccountList = new AccountList();

                // Get accounts to display in index view
                foreach (var account in getAccounts)
                {
                    AccountList.account.Add(account.ToString());
                }

                return this.RedirectToAction("Index", "Bank", new { error = SessionSSN });
            }
            else
            {
                // Redirect to error page if session is invalid
                string loginStatus = "Session is invalid, please try again.";
                return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
            }
        }

        // POST: History
        // Get account history and display account balance
        [HttpPost]
        public ActionResult History(string accountNumber)
        {
            // Check if account number is valid
            if (!string.IsNullOrEmpty(accountNumber))
            {
                // Create BankLogic object and get 5 latest events
                BankLogic bankLogic = new BankLogic();
                List<string> accountHistory = bankLogic.GetAccountInformation(accountNumber, 5);

                // If error is returned go to error page and display error
                if (accountHistory[0] == "false")
                {
                    return this.RedirectToAction("Error", "Bank", new { error = accountHistory[1] });
                }


                else
                {
                    AccountInformation accountInfo = new AccountInformation();

                    List<string> receipt = (List<string>)Session["numberOfBills"];

                    // Check if unit has receipt paper
                    if (int.Parse(receipt[4]) < 2)
                    {
                        //Disable receipt button
                        accountInfo.receipt = "disabled";
                    }

                    // List all account events
                    for (int i = 2; i < accountHistory.Count; i++)
                    {
                        accountInfo.entry.Add(accountHistory[i]);
                    }

                    // Add account info to be displayed in view
                    accountInfo.accountRaw = accountNumber;
                    accountInfo.account = accountHistory[0];

                    //Get session info and add to event log
                    string ssn = (string)Session["SSN"];
                    bankLogic.LoggingOfEvents("View_amount", ssn, accountNumber, 0);

                    return View(accountInfo);
                }
            }
            else
            {
                return this.RedirectToAction("Index", "Bank");
            }
        }

        // GET: Withdrawal
        // handle if user gets here in other ways
        public ActionResult Withdrawal()
        {
            // Create BankLogic objects and check session
            BankLogic bankLogic = new BankLogic();
            string sessionState = bankLogic.CheckSessionState();

            // Check if session is active then redirect to index
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
                // Redirect to error page if session is invalid
                string loginStatus = "Session is invalid, please try again.";
                return this.RedirectToAction("Error", "Bank", new { error = loginStatus });
            }
        }

        // POST: Withdrawal
        // Show page to be able to withdraw money
        [HttpPost]
        public ActionResult Withdrawal(string accountNumber)
        {
            // Check if a acc number was sent
            if (!string.IsNullOrEmpty(accountNumber))
            {
                // Add it to button value for processing
                ViewBag.account = accountNumber;
                return View();
            }
            else
            {
                return this.RedirectToAction("Index", "Bank");
            }
        }

        // GET: Landing
        // Confirmation page when taking out money
        public ActionResult Landing(string quantity, string account)
        {
            // Create BankLogic object and prepare variables
            BankLogic bankLogic = new BankLogic();
            int sum = int.Parse(quantity);

            // Get number of bills that was taken out and if transaction was successfull
            List<string> receipt = (List<string>)Session["numberOfBills"];
            List<string> message = bankLogic.WithdrawFromAccount(sum, account);

            if (message[0] == "Ok")
            {
                WithdrawalConfirmation confirm = new WithdrawalConfirmation();

                // Check if unit has receipt paper
                if (int.Parse(receipt[4]) < 1)
                {
                    confirm.receipt = "disabled";
                }
                
                // Prepare variables to be passsed to view
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
                return this.RedirectToAction("Error", "Bank", new { error = message[1] });
            }
        }

        // POST: Receipt
        // Handle incomming request for receipt
        [HttpPost]
        public ActionResult Receipt(string acc, string quantity, string accRaw)
        {
            // Create BankLogic Object
            BankLogic bankLogic = new BankLogic();

            // Check if the receipt is for withdrawal
            if (string.IsNullOrEmpty(accRaw))
            {
                // Remove one length 
                bankLogic.subtractFromReceipt(1, (string)Session["ATMID"]);

                // Prepare receipt info to be passed in to view
                Receipt receipt = new Receipt();
                receipt.acc = acc;
                receipt.sum = quantity;
                receipt.receiptType = 1;

                // Log receipt event
                string ssn = (string)Session["SSN"];
                bankLogic.LoggingOfEvents("Print_Success", ssn, " ", 0);

                return View(receipt);
            }

            //Check if the receipt is for history and balance
            else if (!string.IsNullOrEmpty(accRaw))
            {
                // Prepare variables and objects
                Receipt receipt = new Receipt();
                List<string> accountHistory = bankLogic.GetAccountInformation(accRaw, 25);

                // Remove two lengths 
                bankLogic.subtractFromReceipt(2, (string)Session["ATMID"]);

                // Prepare receipt info to be passed in to view
                for (int i = 2; i < accountHistory.Count; i++)
                {
                    receipt.entry.Add(accountHistory[i]);
                }
                receipt.acc = acc;
                receipt.receiptType = 2;

                // Log receipt event
                string ssn = (string)Session["SSN"];
                bankLogic.LoggingOfEvents("Print_Success", ssn, " ", 0);

                return View(receipt);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        // GET: Error
        // Error page to display errors
        public ActionResult Error(string error)
        {
            // Open error Model to be passed in to the veiw with the error
            Error errorContainer = new Error();
            errorContainer.errorMessage = error;

            return View(errorContainer);
        }

    }
}