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
using MvcSiteMapProvider.Web.Mvc.Filters;
using Resilience.Models;

namespace Resilience.Controllers
{
    public class GoalsController : Controller
    {
        private DiaryEntriesContainer db = new DiaryEntriesContainer();

        // GET: Goals
        public ActionResult Index()
        {
            var goals = db.Goals.Include(g => g.User);
            return View(goals.ToList());
        }

        // GET: Goals/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goals goals = db.Goals.Find(id);
            if (goals == null)
            {
                return HttpNotFound();
            }
            return View(goals);
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

        // GET: Goals/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goals goals = db.Goals.Find(id);
            if (goals == null)
            {
                return HttpNotFound();
            }
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", goals.UsersId);
            return View(goals);
        }

        // POST: Goals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,GoalName,GoalDescription,DueDate,CompletionDate,MentorFeedback,MenteeComments,MenteeRating,UsersId,MentorId")] Goals goals)
        {
            if (ModelState.IsValid)
            {
                db.Entry(goals).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", goals.UsersId);
            return View(goals);
        }

        // GET: Goals/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Goals goals = db.Goals.Find(id);
            if (goals == null)
            {
                return HttpNotFound();
            }
            return View(goals);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Goals goals = db.Goals.Find(id);
            db.Goals.Remove(goals);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [SiteMapTitle("title")]
        public ActionResult MentorView(int Id)
        {            
            var goals = db.Goals.Where(g => g.UsersId == Id).ToList();
            return View(goals.ToList());
        }

        //GET        
        public ActionResult EditCompletion(int Id)
        {
            var goals = db.Goals.Find(Id);
            goals.CompletionDate = DateTime.Now;
            db.Entry(goals).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Goals", new { id = Id });
        }

        //GET
        public ActionResult Comments(int Id)
        {
            Goals goals = db.Goals.Find(Id);
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", goals.UsersId);
            return View(goals);
        }

        //POST
        [HttpPost]
        public ActionResult Comments(Goals goal)
        {
            var g = db.Goals.Find(goal.Id);
            g.MenteeComments = goal.MenteeComments;
            db.Entry(g).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Goals", new { id = g.Id });
        }

        //GET
        public ActionResult Feedback(int Id)
        {
            Goals goals = db.Goals.Find(Id);
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", goals.UsersId);
            return View(goals);
        }

        //POST
        [HttpPost]
        public ActionResult Feedback(Goals goal)
        {
            var g = db.Goals.Find(goal.Id);
            g.MentorFeedback = goal.MentorFeedback;
            db.Entry(g).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Goals", new { id = g.Id });
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