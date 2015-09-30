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

            //string ATMID = "A1"; // Out of Service
            string ATMID = "B2"; // OK
            //string ATMID = "A2"; // Low amount of currency
            //string ATMID = "C1"; // No currency

            Session["ATMID"] = ATMID;

            List<string> checkStatus = ATMStatus.CheckATMStatus(ATMID);

            if (checkStatus[0] == "Ok")
            {
                return View();
            }
            else
            {
                return this.RedirectToAction("Error", "Bank", new { error = checkStatus[1] });
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Done()
        {
            BankLogic bankLogic = new BankLogic();

            string ssn = (string)Session["SSN"];

            bankLogic.LoggingOfEvents("Logout_success", ssn, " ", 0);

            Session.Abandon();

            return View();
        }
    }
}