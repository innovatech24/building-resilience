using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Resilience.Models;

namespace Resilience.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            // Turn off validation. Comment this to start validating again
            Session["validated"] = "yes";
            return View();
        }

        // This function is to validate if the entry password has passed. It is called from all the views
        public JsonResult ValidateEntry()
        {

            // Turn on validation
            bool res = false;

            // Turn off validation. Comment this to start validating again
            Session["validated"] = "yes";

            if (Session["validated"] != null)
            {
                res = Session["validated"].Equals("yes");
            }

            return Json(res);
        }

        // The actual validation when the user inputs the password.
        public JsonResult Validate(string entry)
        {
            string pass = "IT@2019";
            //string pass = DateTime.Now.ToString("ddd_ddMM");

            if (pass.Equals(entry))
            {
                Session["validated"] = "yes";
            }

            return Json(pass.Equals(entry));
        }

        public string GetDatasets()
        {
            SentimentPy s = new SentimentPy();
            string res = s.getDatasets();

            return res;
        }
    }
}