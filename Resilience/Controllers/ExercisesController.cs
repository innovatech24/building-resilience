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
using Newtonsoft.Json;
using Resilience.Models;
using System.Web.Script.Serialization;

namespace Resilience.Controllers
{
    public class ExercisesController : Controller
    {
        private DiaryEntriesContainer db = new DiaryEntriesContainer();

        //GET: Exercises/5        
        public ActionResult Index(int Id)
        {   
            // Get the tasks related to the goal selected
            var exercises = db.Exercises.Where(e => e.GoalsId == Id).ToList();
            if (!exercises.Any())
            {   
                // If the goal has no tasks, is redirected to the page to add tasks
                TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "warning", Title = "Warning:", Message = "There were no tasks for this goal, so please create some." });
                return RedirectToAction("Create", "Exercises", new { id = Id });
            }
            return View(exercises.ToList());
        }

        // GET: Exercises/Create
        public ActionResult Create(int Id)
        {
            ViewBag.GoalsId = new SelectList(db.Goals, Id, "GoalName");
            var goal = db.Goals.Find(Id);
            ViewBag.goalName = goal.GoalName;
            ViewBag.goalId = goal.Id;
            ViewBag.mentorId = goal.MentorId;
            ViewBag.DueDate = goal.DueDate;

            return View();
        }

        // POST: Exercises/Create
        [HttpPost]      
        public ActionResult Create(string data)
        {
            try
            {
                var serializedData = JsonConvert.DeserializeObject<List<Exercise>>(data);
                var mentor = 0;
                var id = 0;
                foreach (var record in serializedData)
                {                   
                    record.CompletionDate = new DateTime(1990, 2, 10);
                    db.Exercises.Add(record);
                    mentor = record.MentorId.Value;
                    id = record.GoalsId;
                }
                db.SaveChanges();
                var goals = db.Goals.Find(id);
                var mentee = db.Users.Find(goals.UsersId);
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var mentorUser = UserManager.FindById(mentor);
                var ment = db.Users.Find(mentor);
                EmailController mail = new EmailController();
                mail.NewTask(mentorUser.Email, ment.FirstName, mentee.FirstName, mentee.LastName);

                // Type options : info, danger, success, warning
                var res = new JavaScriptSerializer().Serialize(new { Type = "success", Title = "Success!", Message = "Task(s) added correctly!" });

                return Json(res);
            }
            catch
            {
                return Json("error");
            }
        }

        // GET: Exercises/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Exercise exercise = db.Exercises.Find(id);
            if (exercise == null)
            {
                return HttpNotFound();
            }
            ViewBag.GoalsId = new SelectList(db.Goals, "Id", "GoalName", exercise.GoalsId);
            return View(exercise);
        }

        // POST: Exercises/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TaskName,TaskDescription,MentorId,DueDate,CompletionDate,MentorFeedback,MenteeComments,MenteeRating,GoalsId")] Exercise exercise)
        {            
            if (ModelState.IsValid)
            {
                db.Entry(exercise).State = EntityState.Modified;
                db.SaveChanges();
                var goals = db.Goals.Find(exercise.GoalsId);
                var mentee = db.Users.Find(goals.UsersId);
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var menteeUser = UserManager.FindById(mentee.Id);
                var mentorUser = UserManager.FindById(goals.MentorId);
                var ment = db.Users.Find(mentorUser.Id);
                EmailController mail = new EmailController();
                mail.EditTask(menteeUser.Email, mentee.FirstName, ment.FirstName, ment.LastName);
                
                // Type options : info, danger, success, warning
                TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "success", Title = "Success!", Message = "Task updated correctly!" });


                return View(exercise);
            }
            ViewBag.GoalsId = new SelectList(db.Goals, "Id", "GoalName", exercise.GoalsId);
            return View(exercise);
        }

        //GET EditCompletion/5 
        public ActionResult EditCompletion(int Id)
        {
            var exercise = db.Exercises.Find(Id);
            exercise.CompletionDate = DateTime.Now;
            db.Entry(exercise).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Exercises", new { id = exercise.GoalsId });
        }

        //GET Comments/5
        public ActionResult Comments(int Id)
        {
            Exercise exercise = db.Exercises.Find(Id);
            ViewBag.GoalsId = new SelectList(db.Goals, "Id", "GoalName", exercise.GoalsId);
            return View(exercise);
        }

        //POST Comments
        [HttpPost]
        public ActionResult Comments(Exercise exercise)
        {
            var e = db.Exercises.Find(exercise.Id);
            e.MenteeComments = exercise.MenteeComments;
            db.Entry(e).State = EntityState.Modified;
            db.SaveChanges();

            // Type options : info, danger, success, warning
            TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "success", Title = "Success!", Message = "Comment added correctly!" });


            return View(e);
        }

        //GET Feedback/5
        public ActionResult Feedback(int Id)
        {
            Exercise exercise = db.Exercises.Find(Id);
            ViewBag.GoalsId = new SelectList(db.Goals, "Id", "GoalName", exercise.GoalsId);
            return View(exercise);
        }

        //POST Feedback
        [HttpPost]
        public ActionResult Feedback(Exercise exercise)
        {
            var e = db.Exercises.Find(exercise.Id);
            e.MentorFeedback = exercise.MentorFeedback;
            db.Entry(e).State = EntityState.Modified;
            db.SaveChanges();

            // Type options : info, danger, success, warning
            TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "success", Title = "Success!", Message = "Feedback added correctly!" });


            return View(e);
        }

        [Authorize(Roles = "Mentee")]
        [HttpPost]
        public void setExerciseMenteeRate(int Id, int rate)
        {
            Exercise ex = db.Exercises.Find(Id);
            ex.MenteeRating = rate.ToString();
            db.Entry(ex).State = EntityState.Modified;
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