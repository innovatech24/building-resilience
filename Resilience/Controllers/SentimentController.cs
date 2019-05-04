using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Resilience.Models;

namespace Resilience.Controllers
{
    public class SentimentController : Controller
    {
        // GET: Sentiment
        public ActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Mentor")]
        public JsonResult SentimentMinMax(string input)
        {
            SentimentPy s = new SentimentPy();
            string res = s.getSentimentMinMax(input);

            return Json(res);
        }

        [Authorize(Roles = "Mentor")]
        public JsonResult SentimentScore(string input)
        {
            SentimentPy s = new SentimentPy();

            float res = s.getSentimentScore(input);

            return Json(res);
        }

    }
}