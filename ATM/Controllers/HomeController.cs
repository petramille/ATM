using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ATM.Models;

namespace ATM.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            BankLogic ATMStatus = new BankLogic();

            // Change ATM units
            string ATMID = "A1"; // Ok
            //string ATMID = "B2"; // Out of Service
            //string ATMID = "A2"; // Low amount of currency
            //string ATMID = "C1"; // No currency

            // Set current ATM in session and check status
            Session["ATMID"] = ATMID;
            List<string> checkStatus = ATMStatus.CheckATMStatus(ATMID);

            // Show log in page or error page depending on status
            if (checkStatus[0] == "Ok")
            {
                return View();
            }
            else
            {
                return this.RedirectToAction("Error", "Bank", new { error = checkStatus[1] });
            }
        }

        // GET: About
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        // GET: Contact
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        // GET: Done
        public ActionResult Done()
        {
            // Get bank logic object and current session
            BankLogic bankLogic = new BankLogic();
            string ssn = (string)Session["SSN"];

            // Send log out successfull to log
            bankLogic.LoggingOfEvents("Logout_success", ssn, " ", 0);

            // Clear session
            Session.Abandon();

            return View();
        }
    }
}