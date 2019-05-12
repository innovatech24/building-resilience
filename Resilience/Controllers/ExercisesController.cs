using System;
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

namespace Resilience.Controllers
{
    public class ExercisesController : Controller
    {
        private DiaryEntriesContainer db = new DiaryEntriesContainer();

        // GET: Exercises
        /*public ActionResult Index()
        {
            var exercises = db.Exercises.Include(e => e.Goal);
            return View(exercises.ToList());
        }*/

        //GET: Exercises/5
        public ActionResult Index(int Id)
        {
            var exercises = db.Exercises.Where(e => e.GoalsId == Id);
            return View(exercises.ToList());
        }

        // GET: Exercises/Details/5
        public ActionResult Details(int? id)
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
            return View(exercise);
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
                return Json("success");
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
                return RedirectToAction("Index", new { id = exercise.GoalsId });
            }
            ViewBag.GoalsId = new SelectList(db.Goals, "Id", "GoalName", exercise.GoalsId);
            return View(exercise);
        }

        // GET: Exercises/Delete/5
        public ActionResult Delete(int? id)
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
            return View(exercise);
        }

        // POST: Exercises/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Exercise exercise = db.Exercises.Find(id);
            db.Exercises.Remove(exercise);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //GET        
        public ActionResult EditCompletion(int Id)
        {
            var exercise = db.Exercises.Find(Id);
            exercise.CompletionDate = DateTime.Now;
            db.Entry(exercise).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Exercises", new { id = exercise.GoalsId });
        }

        //GET
        public ActionResult Comments(int Id)
        {
            Exercise exercise = db.Exercises.Find(Id);
            ViewBag.GoalsId = new SelectList(db.Goals, "Id", "GoalName", exercise.GoalsId);
            return View(exercise);
        }

        //POST
        [HttpPost]
        public ActionResult Comments(Exercise exercise)
        {
            var e = db.Exercises.Find(exercise.Id);
            e.MenteeComments = exercise.MenteeComments;
            db.Entry(e).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Exercises", new { id = e.Id });
        }

        //GET
        public ActionResult Feedback(int Id)
        {
            Exercise exercise = db.Exercises.Find(Id);
            ViewBag.GoalsId = new SelectList(db.Goals, "Id", "GoalName", exercise.GoalsId);
            return View(exercise);
        }

        //POST
        [HttpPost]
        public ActionResult Feedback(Exercise exercise)
        {
            var e = db.Exercises.Find(exercise.Id);
            e.MentorFeedback = exercise.MentorFeedback;
            db.Entry(e).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index", "Exercises", new { id = e.Id });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
    }
}
