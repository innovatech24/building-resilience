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
                foreach (var record in serializedData)
                {                   
                    record.CompletionDate = DateTime.Now;
                    db.Exercises.Add(record);
                }
                db.SaveChanges();
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
                return RedirectToAction("Index");
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
