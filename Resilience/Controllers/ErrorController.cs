using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Resilience.Controllers
{
    public class ErrorController : Controller
    {

        //GET: NotFound
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult NothingAdded()
        {
            return View();
        }

        //GET: NoMentorMapped
        public ActionResult NoMentorMapped()
        {
            return View();
        }
    }
}