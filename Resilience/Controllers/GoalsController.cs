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
            goals.CompletionDate = DateTime.Now;
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

        public ActionResult MentorView(int Id)
        {            
            var goals = db.Goals.Where(g => g.UsersId == Id).ToList();
            return View(goals.ToList());
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