using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
            if (!string.IsNullOrEmpty(SSN))
            {
                Models.Login loginCredentials = new Models.Login();
                BankLogic loginHandler = new BankLogic();

                try
                {
                    //loginHandler.LogIn(SSN, int.Parse(pin));

                    loginCredentials.ssn = Convert.ToInt64(SSN);
                    loginCredentials.pin = int.Parse(pin);
                }
                catch (Exception)
                {
                    throw;
                }

                return View(loginCredentials);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }

        // GET: Bank
        public ActionResult History()
        {
            return View();
        }

        // GET: Bank
        public ActionResult Withdrawal()
        {
            return View();
        }
    }
}