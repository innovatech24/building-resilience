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
using MvcSiteMapProvider.Web.Mvc.Filters;

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
            if (db.Users.Find(user.Id) == null)
            {
                return RedirectToAction("Create", "Users");
            }
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

        [SiteMapTitle("title")]
        [Authorize(Roles = "Mentor")]        
        public ActionResult Landingpage(int Id)
        {
            ViewBag.id = Id;            
            return View();
        }

        [SiteMapTitle("title")]
        [Authorize(Roles = "Mentor")]
        public ActionResult Progress(int Id)
        {
            var user = db.Users.Find(Id);

            return View(user);
        }

        [SiteMapTitle("title")]
        [Authorize(Roles = "Mentee")]
        public ActionResult MenteeProgress()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());

            Users userdb = db.Users.Find(user.Id);

            return View(userdb);
        }

        [Authorize]
        public JsonResult DashboardData(int Id)
        {
            
            // Get from database
            var diaryentries = db.DiaryEntries
                .Where(d => d.UsersId == Id)
                .Select(d => new { Date = d.Date, d.MenteeFeedback,d.SentimentScore,d.MentorFeedback}).ToList();

            var goalsTasks = db.Goals
                .Where(d => d.UsersId == Id)
                .Select(d => new {d.GoalName,d.MenteeRating,d.DueDate,d.CompletionDate,
                    tasks = db.Exercises
                    .Where(e => e.GoalsId==d.Id)
                    .Select(e => new {e.TaskName,e.DueDate,e.CompletionDate,e.MenteeRating }).ToList() })
                .ToList();            

            // DIARY ENTRY. Format output
            var Dscores = new List<double>();
            var Dentries = new List<object>();
            var Dfeeling = new List<double>();
            foreach (var entry in diaryentries) 
            {
                Dscores.Add(entry.SentimentScore.Value);
                Dfeeling.Add(entry.MenteeFeedback.Value);

                Dentries.Add(new
                {
                    date = entry.Date.ToString("MM-dd-yyyy"),
                    score = entry.SentimentScore.Value,
                    feeling = entry.MenteeFeedback.Value
                });
            }

            // GOALS
            var Ggoals = new List<object>();
            var Gtasks = new List<object>();
            var openGoals = 0;
            var closeGoals = 0;
            foreach (var goal in goalsTasks)
            {
                // Count 
                int completedTasks = 0;
                int delayedCompletedTasks = 0;
                int delayedTasks = 0;
                int pendingTasks = 0;
                foreach (var task in goal.tasks)
                {

                    if(DateTime.Compare(task.CompletionDate.Date,new DateTime(1990,2,10).Date)!=0)
                    {
                        if (DateTime.Compare(task.CompletionDate.Date,task.DueDate.Date) > 0)
                        {
                            delayedCompletedTasks++;
                        }
                        else
                        {
                            completedTasks++;
                        }
                    }
                    else if(DateTime.Compare(DateTime.Now.Date, task.DueDate.Date) > 0)
                    {
                        delayedTasks++;
                    }
                    else
                    {
                        pendingTasks++;
                    }

                    Gtasks.Add(new
                    {
                        goal.GoalName,
                        goalDueDate = goal.DueDate.ToString("MM-dd-yyyy"),
                        task.TaskName,
                        DueDate =task.DueDate.ToString("MM-dd-yyyy"),
                        CompletionDate =task.CompletionDate.ToString("MM-dd-yyyy"),
                        task.MenteeRating
                    });

                }

                //If goal has no completion date. It's open
                if (DateTime.Compare(goal.CompletionDate.Date, new DateTime(1990, 2, 10).Date) == 0)
                {
                    openGoals++;
                    Ggoals.Add(new
                    {
                        goal.GoalName,
                        goal.MenteeRating,
                        DueDate = goal.DueDate.ToString("MM-dd-yyyy"),
                        CompletionDate = goal.CompletionDate.ToString("MM-dd-yyyy"),
                        totalTasks = goal.tasks.Count(),
                        delayedCompletedTasks,
                        completedTasks,
                        delayedTasks,
                        pendingTasks
                    });
                }
                else
                {
                    closeGoals++;
                }

                /*Ggoals.Add(new
                {
                    goal.GoalName,
                    goal.MenteeRating,
                    DueDate = goal.DueDate.ToString("MM-dd-yyyy"),
                    CompletionDate = goal.CompletionDate.ToString("MM-dd-yyyy"),
                    totalTasks = goal.tasks.Count(),
                    delayedCompletedTasks,
                    completedTasks,
                    delayedTasks,
                    pendingTasks
                });*/

            }
            try
            {
                return Json(new JavaScriptSerializer().Serialize(new
                {
                    diaries = new
                    {
                        AvgSentimentScore = Dscores.Average(),
                        AvgFeeling = Dfeeling.Average(),
                        NumDiaries = Dentries.Count(),
                        Entries = Dentries
                    },
                    goals = new
                    {
                        Goals = Ggoals,
                        OpenGoals = openGoals,
                        CloseGoals = closeGoals,
                        Tasks = Gtasks
                    }
                }));
            }
            catch
            {
                return Json("error");
            }
            
        }
    }
}
