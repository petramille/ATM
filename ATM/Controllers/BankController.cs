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
                Models.Login test = new Models.Login();
                
                try
                {
                    test.ssn = int.Parse(SSN);
                    test.pin = int.Parse(pin);
                }
                catch (Exception x)
                {
                    throw x;
                }

                return View(test);
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


    }
}