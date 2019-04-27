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
    public class DiaryEntriesController : Controller
    {
        private DiaryEntriesContainer db = new DiaryEntriesContainer();

        // GET: DiaryEntries
        public ActionResult Index()
        {
            //var diaryEntries = db.DiaryEntries.Include(d => d.User);
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            Users currentUser = db.Users.Find(user.Id);
            var diaryEntries = db.DiaryEntries.Where(d => d.UsersId == currentUser.Id).ToList();
            return View(diaryEntries.ToList());
        }

        //GET: Dashboard
        public ActionResult Dashboard()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            Users currentUser = db.Users.Find(user.Id);
            var diaryEntries = db.DiaryEntries.Where(d => d.MentorId == currentUser.Id).ToList();
            return View(diaryEntries.ToList());
        }

        //GET: View/5
        public ActionResult View(int Id)
        {
            var diaryEntries = db.DiaryEntries.Where(d => d.Id == Id).ToList();
            return View(diaryEntries.ToList());
        }

        // GET: DiaryEntries/Details/5
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
        public ActionResult Create()
        {
            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName");
            return View();
        }

        // POST: DiaryEntries/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
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
                return RedirectToAction("Index");
            }

            ViewBag.UsersId = new SelectList(db.Users, "Id", "FirstName", diaryEntries.UsersId);
            return View(diaryEntries);
        }

        // GET: DiaryEntries/Edit/5
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DiaryEntries diaryEntries = db.DiaryEntries.Find(id);
            db.DiaryEntries.Remove(diaryEntries);
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
