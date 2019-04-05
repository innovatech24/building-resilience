using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Resilience.Controllers
{
    public class ChallengesController : Controller
    {
        // GET: Challenges
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Bullying()
        {
            return View();
        }

        public ActionResult Abuse()
        {
            return View();
        }

        public ActionResult Violence()
        {
            return View();
        }
    }
}