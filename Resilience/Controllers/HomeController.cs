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
            return View();
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

        public JsonResult ValidateEntry()
        {

            // Turn on validation
            bool res = false;


            if (Session["validated"] != null)
            {
                res = Session["validated"].Equals("yes");
            }

            return Json(res);
        }

        public JsonResult Validate(string entry)
        {
            string pass = "123";
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