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
            string checkStatus = "Ok";

            // checkStatus = CheckDatabase();

            if (checkStatus == "Ok")
            {
                return View();
            }
            else
            {
                return this.RedirectToAction("Error", "Bank", new { error = checkStatus });
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
            Session.Abandon();

            return View();
        }
    }
}