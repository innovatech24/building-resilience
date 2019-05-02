using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Resilience.Controllers
{
    public class HomeController : Controller
    {
        //[HandleError]
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

        public JsonResult Validate(string entry)
        {
            string pass = "789456123";

            if (pass.Equals(entry))
            {
                Session["validated"] = "yes";
            }

            return Json(pass.Equals(entry));
        }
    }
}