using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Resilience.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace Resilience.Controllers
{
    public class UsersController : Controller
    {
        private DiaryEntriesContainer db = new DiaryEntriesContainer();
        private ApplicationUserManager _userManager;

        public UsersController()
        {

        }

        public UsersController(ApplicationUserManager userManager)
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

        // GET: Users
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // GET: Users/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,EmailAddress,MentorId")] Users users)
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            users.Id = user.Id;
            users.EmailAddress = user.Email;

            ModelState.Clear();
            TryValidateModel(users);

            if (ModelState.IsValid)
            {
                db.Users.Add(users);
                db.SaveChanges();

                var roles = UserManager.GetRoles(user.Id);
                
                if (roles.Count == 2)
                {
                    return RedirectToAction("Choice", "Options");
                }
                else
                {
                    if (roles.Contains("Mentor"))
                    {
                        return RedirectToAction("Mentor", "Options");
                    }
                    else
                    {
                        return RedirectToAction("Mentee", "Options");
                    }
                }                
            }

            return View(users);
        }

        // GET: Users/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,EmailAddress,MentorId")] Users users)
        {
            if (ModelState.IsValid)
            {
                db.Entry(users).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(users);
        }

        //GET: Users/Choose
        public ActionResult Choose()
        {
            return View();
        }

        //POST: Users/Choose
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Choose(AddMentor mentor)
        {
            if(ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(mentor.Email))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var users = UserManager.FindByEmail(mentor.Email);
                var userId = users.Id;
                if (users == null)
                {
                    return HttpNotFound();
                }
                ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
                Users currentUser = db.Users.Find(user.Id);
                currentUser.MentorId = users.Id;
                db.Entry(currentUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Mentee", "Options");
            }
            return View();
        }

        // GET: Users/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Users users = db.Users.Find(id);
            if (users == null)
            {
                return HttpNotFound();
            }
            return View(users);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Users users = db.Users.Find(id);
            db.Users.Remove(users);
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
