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



            return View();
        }

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
                    return View(loginHandler);
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