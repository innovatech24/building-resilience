using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Dynamic;
using System.Web.Script.Serialization;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using MvcSiteMapProvider;
using MvcSiteMapProvider.Web.Mvc.Filters;
using Resilience.Models;

namespace Resilience.Controllers
{
    public class DiaryEntriesController : Controller
    {
        private DiaryEntriesContainer db = new DiaryEntriesContainer();

        // GET: DiaryEntries
        [Authorize(Roles = "Mentee")]
        public ActionResult Index()
        {
            //var diaryEntries = db.DiaryEntries.Include(d => d.User);
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            Users currentUser = db.Users.Find(user.Id);
            var diaryEntries = db.DiaryEntries.Where(d => d.UsersId == currentUser.Id).ToList();
            return View(diaryEntries.ToList());
        }

        //GET: Dashboard
        [Authorize(Roles = "Mentor")]
        public ActionResult Dashboard()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            Users currentUser = db.Users.Find(user.Id);
            //var diaryEntries = db.DiaryEntries.Where(d => d.MentorId == currentUser.Id).ToList();
            var users = db.Users.Where(d => d.MentorId == currentUser.Id).ToList();
            return View(users.ToList());
        }

        //GET: View/5
        [SiteMapTitle("title")]
        [Authorize(Roles = "Mentor")]
        public ActionResult View(int Id)
        {
            var diaryEntries = db.DiaryEntries.Where(d => d.Id == Id).ToList();
            return View(diaryEntries.ToList());
        }

        //GET: ViewUser/5
        [SiteMapTitle("title")]
        [Authorize(Roles = "Mentor")]
        public ActionResult ViewUser(int Id)
        {
            var diaryEntries = db.DiaryEntries.Where(d => d.UsersId == Id);
            return View(diaryEntries);
        }


        // GET: DiaryEntries/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaryEntries diaryEntries = db.DiaryEntries.Find(id);
            if (diaryEntries == null)
            {
                return HttpNotFound();
            }
            return View(diaryEntries);
        }

        // GET: DiaryEntries/Create
        [Authorize]
        public ActionResult Create()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            Users currentUser = db.Users.Find(user.Id);
            if (currentUser.MentorId == null)
            {
                return RedirectToAction("NoMentorMapped", "Error");
            }
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: DiaryEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Mentee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Entry,UsersId,MentorId,SentimentScore,MentorFeedback,Date,MenteeFeedback")] DiaryEntries diaryEntries)
        {
            SentimentPy sent = new SentimentPy();
            var score = sent.getSentimentScore(diaryEntries.Entry);
            diaryEntries.SentimentScore = score;
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            Users currentUser = db.Users.Find(user.Id);
            diaryEntries.UsersId = currentUser.Id;
            diaryEntries.MentorId = currentUser.MentorId.Value;
            diaryEntries.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.DiaryEntries.Add(diaryEntries);
                db.SaveChanges();
                EmailController mail = new EmailController();
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var users = UserManager.FindById(diaryEntries.MentorId);
                Users mentor = db.Users.Find(users.Id);
                mail.NewDiary(users.Email, mentor.FirstName, currentUser.FirstName, currentUser.LastName);

                // Type options : info, danger, success, warning
                TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "success", Title = "Success!", Message = "Diary entry added correctly!" });

                return View();
            }

            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", diaryEntries.UsersId);
            return View(diaryEntries);
        }

        // GET: DiaryEntries/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaryEntries diaryEntries = db.DiaryEntries.Find(id);
            if (diaryEntries == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", diaryEntries.UsersId);
            return View(diaryEntries);
        }

        // POST: DiaryEntries/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Entry,UsersId,MentorId,SentimentScore,MentorFeedback,Date,MenteeFeedback")] DiaryEntries diaryEntries)
        {
            if (ModelState.IsValid)
            {
                db.Entry(diaryEntries).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", diaryEntries.UsersId);
            return View(diaryEntries);
        }

        // GET: DiaryEntries/Delete/5
        [Authorize(Roles = "Mentee")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DiaryEntries diaryEntries = db.DiaryEntries.Find(id);
            if (diaryEntries == null)
            {
                return HttpNotFound();
            }
            return View(diaryEntries);
        }

        // POST: DiaryEntries/Delete/5
        [Authorize(Roles = "Mentee")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiaryEntries diaryEntries = db.DiaryEntries.Find(id);
            db.DiaryEntries.Remove(diaryEntries);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET: Feedback/5
        [SiteMapTitle("title")]
        [Authorize(Roles = "Mentor")]
        public ActionResult Feedback(int Id)
        {
            DiaryEntries diaryEntries = db.DiaryEntries.Find(Id);
            return View(diaryEntries);
        }

        //POST: Feedback
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Feedback(DiaryEntries mentfeedback)
        {            
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(mentfeedback.MentorFeedback))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }              
                DiaryEntries diaryEntries = db.DiaryEntries.Find(mentfeedback.Id);
                if (diaryEntries == null)
                {
                    return HttpNotFound();
                }
                diaryEntries.MentorFeedback = mentfeedback.MentorFeedback;
                db.Entry(diaryEntries).State = EntityState.Modified;
                db.SaveChanges();
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var menteeUser = UserManager.FindById(mentfeedback.UsersId);
                var mentee = db.Users.Find(mentfeedback.UsersId);
                var mentor = db.Users.Find(mentee.MentorId);
                EmailController mail = new EmailController();
                mail.FeedbackProvided(menteeUser.Email, mentee.FirstName, mentor.FirstName, mentor.LastName);
                
                // Type options : info, danger, success, warning
                TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "success", Title = "Success!", Message = "Diary entry added correctly!" });

                //return RedirectToAction("Dashboard", "DiaryEntries");
                return View(diaryEntries);
            }
            return View();
        }

        //GET: ViewFeedback
        [Authorize (Roles = "Mentee")]
        public ActionResult ViewFeedback()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            Users currentUser = db.Users.Find(user.Id);
            var diaryEntries = db.DiaryEntries.Where(d => d.UsersId == currentUser.Id);
            
            // The name and last name of the mentor is going in unused variables
            foreach(DiaryEntries d in diaryEntries)
            {
                var mentor = db.Users.Find(d.MentorId);
                d.User.FirstName = mentor.FirstName;
                d.User.LastName = mentor.LastName;
               
            }

            return View(diaryEntries);
        }

        /*[Authorize(Roles = "Mentee")]
        [HttpPost]
        public void ViewFeedback(int Id, int rate)
        {
            DiaryEntries diaryEntries = db.DiaryEntries.Find(Id);
            diaryEntries.MenteeFeedback = rate;
            db.Entry(diaryEntries).State = EntityState.Modified;
            db.SaveChanges();

            //return View(diaryEntries);
        }
        */

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
