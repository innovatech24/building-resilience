﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Resilience.Models;
using System.Web.Script.Serialization;

namespace Resilience.Controllers
{
    public class GoalsController : Controller
    {
        private DiaryEntriesContainer db = new DiaryEntriesContainer();

        // GET: Goals
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());            
            var goals = db.Goals.Where(g => g.UsersId == user.Id);
            return View(goals.OrderByDescending(g => g.Id).ToList());
        }

        // GET: Goals/Create
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

        // POST: Goals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,GoalName,GoalDescription,DueDate,CompletionDate,MentorFeedback,MenteeComments,MenteeRating,UsersId,MentorId")] Goals goals)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            Users currentUser = db.Users.Find(user.Id);            
            goals.UsersId = currentUser.Id;
            goals.MentorId = currentUser.MentorId.Value;
            goals.CompletionDate = new DateTime(1990, 2, 10);
            if (ModelState.IsValid)
            {
                db.Goals.Add(goals);
                db.SaveChanges();
                return RedirectToAction("Create", "Exercises", new { id = goals.Id });
            }

            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", goals.UsersId);
            return View(goals);
        }
        
        public ActionResult MentorView(int Id)
        {            
            var goals = db.Goals.Where(g => g.UsersId == Id).ToList();
            return View(goals.OrderByDescending(g => g.Id).ToList());
        }

        //GET        
        public ActionResult EditCompletion(int Id)
        {
            var goals = db.Goals.Find(Id);
            var tasks = db.Exercises.Where(t => t.GoalsId == Id);
            var counter = 0;
            foreach (var task in tasks)
            {
                if (DateTime.Compare(task.CompletionDate, new DateTime(1990, 02, 10)) == 0)
                {
                    counter++;
                }
            }
            if (counter == 0)
            {
                goals.CompletionDate = DateTime.Now;
                db.Entry(goals).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Goals", new { id = Id });
            }
            TempData["GoalMessage"] = new JavaScriptSerializer().Serialize(new { Type = "danger", Title = "Error:", Message = "There are tasks in the goal that are not completed. Please complete all tasks before completing a goal." });
            return RedirectToAction("Index", "Goals", new { id = Id });
        }
        
        //GET Comments
        public ActionResult Comments(int Id)
        {
            Goals goals = db.Goals.Find(Id);
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", goals.UsersId);
            return View(goals);
        }

        //POST Comments
        [HttpPost]
        public ActionResult Comments(Goals goal)
        {
            var g = db.Goals.Find(goal.Id);
            g.MenteeComments = goal.MenteeComments;
            db.Entry(g).State = EntityState.Modified;
            db.SaveChanges();

            // Type options : info, danger, success, warning
            TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "success", Title = "Success!", Message = "Comment added correctly!" });

            return View();
        }

        //GET Feedback
        public ActionResult Feedback(int Id)
        {
            Goals goals = db.Goals.Find(Id);
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", goals.UsersId);
            return View(goals);
        }

        //POST Feedback
        [HttpPost]
        public ActionResult Feedback(Goals goal)
        {
            var g = db.Goals.Find(goal.Id);
            g.MentorFeedback = goal.MentorFeedback;
            db.Entry(g).State = EntityState.Modified;
            db.SaveChanges();

            // Type options : info, danger, success, warning
            TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "success", Title = "Success!", Message = "Feedback added correctly!" });

            return View(g);
        }

        [Authorize(Roles = "Mentee")]
        [HttpPost]
        public void SetGoalMenteeRate(int Id, int rate)
        {
            Goals goal = db.Goals.Find(Id);
            goal.MenteeRating = rate;
            db.Entry(goal).State = EntityState.Modified;
            db.SaveChanges();

            //return View(diaryEntries);
        }

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
 