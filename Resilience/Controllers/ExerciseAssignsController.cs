using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Resilience.Models;

namespace Resilience.Controllers
{
    public class ExerciseAssignsController : Controller
    {
        private DiaryEntriesContainer db = new DiaryEntriesContainer();

        // GET: ExerciseAssigns
        public ActionResult Index()
        {
            var exerciseAssigns = db.ExerciseAssigns.Include(e => e.Task).Include(e => e.User);
            return View(exerciseAssigns.ToList());
        }

        // GET: ExerciseAssigns/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseAssign exerciseAssign = db.ExerciseAssigns.Find(id);
            if (exerciseAssign == null)
            {
                return HttpNotFound();
            }
            return View(exerciseAssign);
        }

        // GET: ExerciseAssigns/Create
        public ActionResult Create()
        {
            ViewBag.TaskId = new SelectList(db.Exercises, "Id", "TaskName");
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: ExerciseAssigns/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,MentorId,DueDate,CompletionDate,MenteeRating,MentorFeedback,MenteeComments,TaskId,UsersId")] ExerciseAssign exerciseAssign)
        {
            if (ModelState.IsValid)
            {
                db.ExerciseAssigns.Add(exerciseAssign);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TaskId = new SelectList(db.Exercises, "Id", "TaskName", exerciseAssign.TaskId);
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", exerciseAssign.UsersId);
            return View(exerciseAssign);
        }

        // GET: ExerciseAssigns/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseAssign exerciseAssign = db.ExerciseAssigns.Find(id);
            if (exerciseAssign == null)
            {
                return HttpNotFound();
            }
            ViewBag.TaskId = new SelectList(db.Exercises, "Id", "TaskName", exerciseAssign.TaskId);
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", exerciseAssign.UsersId);
            return View(exerciseAssign);
        }

        // POST: ExerciseAssigns/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,MentorId,DueDate,CompletionDate,MenteeRating,MentorFeedback,MenteeComments,TaskId,UsersId")] ExerciseAssign exerciseAssign)
        {
            if (ModelState.IsValid)
            {
                db.Entry(exerciseAssign).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TaskId = new SelectList(db.Exercises, "Id", "TaskName", exerciseAssign.TaskId);
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", exerciseAssign.UsersId);
            return View(exerciseAssign);
        }

        // GET: ExerciseAssigns/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ExerciseAssign exerciseAssign = db.ExerciseAssigns.Find(id);
            if (exerciseAssign == null)
            {
                return HttpNotFound();
            }
            return View(exerciseAssign);
        }

        // POST: ExerciseAssigns/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ExerciseAssign exerciseAssign = db.ExerciseAssigns.Find(id);
            db.ExerciseAssigns.Remove(exerciseAssign);
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
