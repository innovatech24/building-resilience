using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Resilience.Models;
using System;
using System.Dynamic;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;

namespace Resilience.Controllers
{
    public class OptionsController : Controller
    {
        private ApplicationUserManager _userManager;
        private DiaryEntriesContainer db = new DiaryEntriesContainer();

        public OptionsController()
        {

        }

        public OptionsController(ApplicationUserManager userManager)
        {
            UserManager = userManager;
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        // GET: Options
        [Authorize]
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            var roles = UserManager.GetRoles(user.Id);

            if (roles.Count == 2)
            {
                return RedirectToAction("Choice");
            }
            else
            {
                if (roles.Contains("Mentor"))
                {
                    return RedirectToAction("Mentor");
                }
                else
                {
                    return RedirectToAction("Mentee");
                }
            }
        }

        [Authorize(Roles = "Mentor")]
        public ActionResult Choice()
        {
            return View();
        }

        [Authorize(Roles = "Mentee")]
        public ActionResult Mentee()
        {
            return View();
        }

        [Authorize(Roles = "Mentor")]
        public ActionResult Mentor()
        {
            return View();
        }

        [Authorize(Roles = "Mentor")]        
        public ActionResult Landingpage(int Id)
        {
            ViewBag.id = Id;            
            return View();
        }

        [Authorize(Roles = "Mentor")]
        public ActionResult Progress(int Id)
        {
            var user = db.Users.Find(Id);

            return View(user);
        }

        [Authorize]
        public JsonResult DashboardData(int Id)
        {
            
            var diaryentries = db.DiaryEntries
                .Where(d => d.UsersId == Id)
                .Select(d => new { Date = d.Date, d.MenteeFeedback,d.SentimentScore,d.MentorFeedback}).ToList();

            var goalsTasks = db.Goals
                .Where(d => d.UsersId == Id)
                .Select(d => new {d.GoalName,d.MenteeRating,d.DueDate,d.CompletionDate,
                    tasks = db.Exercises
                    .Where(e => e.GoalsId==d.Id)
                    .Select(e => new {e.TaskName,e.DueDate,e.CompletionDate,e.MenteeRating }) })
                .ToList();

            // DIARY ENTRY
            var Dscores = new List<double>();
            var Dentries = new List<object>();
            var Dfeeling = new List<double>();
            foreach (var entry in diaryentries) 
            {
                Dscores.Add(entry.SentimentScore.Value);
                Dfeeling.Add(entry.MenteeFeedback.Value);

                Dentries.Add(new { date = entry.Date.ToString("MM-dd-yyyy"), score = entry.SentimentScore.Value, feeling = entry.MenteeFeedback.Value });
            }

            // GOALS
            foreach(var goal in goalsTasks)
            {

            }

            return Json(new JavaScriptSerializer().Serialize(new {
                diaries = new { AvgSentimentScore = Dscores.Average(),
                                AvgFeeling = Dfeeling.Average(),
                                Entries = Dentries
                },
                goals = goalsTasks
            }));
        }
    }
}
