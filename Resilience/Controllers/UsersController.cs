using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Dynamic;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Resilience.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web.Script.Serialization;

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
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: Users/Details/5
        [Authorize]
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
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
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
        [Authorize]
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
        [Authorize]
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

        //GET: Users/ChooseMentor
        [Authorize(Roles = "Mentee")]
        public ActionResult ChooseMentor()
        {
            return View();
        }

        //POST: Users/ChooseMentor
        [Authorize(Roles = "Mentee")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChooseMentor(AddMentor mentor)
        {
            if(ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(mentor.Email))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var users = UserManager.FindByEmail(mentor.Email);
                
                if (users == null)
                {
                    // Type options : info, danger, success, warning
                    TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "danger", Title = "Error:", Message = "User not found" });

                    return View();
                }
                var userId = users.Id;
                var mentorUser = db.Users.Find(userId);
                var roles = UserManager.GetRoles(userId);
                if (roles.Contains("Mentor"))
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
                    Users currentUser = db.Users.Find(user.Id);
                    if (currentUser.Id == users.Id)
                    {
                        // Type options : info, danger, success, warning
                        TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "warning", Title = "Warning:", Message = "You cannot add yourself as a mentor!" });

                        return View();
                    }
                    currentUser.MentorId = users.Id;                    
                    db.Entry(currentUser).State = EntityState.Modified;
                    db.SaveChanges();
                    EmailController mail = new EmailController();
                    mail.MentorConfirmation(mentor.Email, currentUser.FirstName, currentUser.LastName, mentorUser.FirstName);

                    // Type options : info, danger, success, warning
                    TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type="success", Title = "Success!", Message = "Mentor added correctly!" });

                    
                    //return RedirectToAction("Mentee", "Options");
                    return View();
                }
                else
                {
                    // Type options : info, danger, success, warning
                    TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "warning", Title = "Warning:", Message = "User not registered as mentor" });

                    return View();
                }                
            }
            return View();
        }

        //GET: Users/ChooseMentee
        [Authorize(Roles = "Mentor")]
        public ActionResult ChooseMentee()
        {
            return View();
        }

        //POST: Users/ChooseMentee
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChooseMentee(AddMentee mentee)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(mentee.Email))
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                var UserManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
                var users = UserManager.FindByEmail(mentee.Email);
                
                if (users == null)
                {
                    // Type options : info, danger, success, warning
                    TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "danger", Title = "Error:", Message = "User not found" });

                    return View();
                }
                var userId = users.Id;              
                var roles = UserManager.GetRoles(userId);
                if(roles.Contains("Mentee"))
                {
                    ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
                    Users currentUser = db.Users.Find(user.Id);
                    Users menteeUser = db.Users.Find(userId);

                    if(menteeUser.MentorId != null)
                    {
                        if(menteeUser.MentorId == currentUser.Id)
                        {
                            // Type options : info, danger, success, warning
                            TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "warning", Title = "Warning:", Message = "This user is already your mentee!" });

                            return View();
                        }
                        else if (currentUser.Id == menteeUser.Id)
                        {
                            // Type options : info, danger, success, warning
                            TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "warning", Title = "Warning:", Message = "You cannot add yourself as a mentee!" });

                            return View();
                        }
                        else
                        {
                            // Type options : info, danger, success, warning
                            TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "warning", Title = "Warning:", Message = "This user is allocated to another mentor!" });

                            return View();
                        }
                    }

                    menteeUser.MentorId = currentUser.Id;
                    db.Entry(currentUser).State = EntityState.Modified;
                    db.SaveChanges();
                    EmailController mail = new EmailController();
                    mail.MenteeConfirmation(mentee.Email, currentUser.FirstName, currentUser.LastName , menteeUser.FirstName);

                    // Type options : info, danger, success, warning
                    TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "success", Title = "Success!", Message = "Mentee added correctly!" });

                    //return RedirectToAction("Mentee", "Options");
                    return View();
                }
                else
                {
                    // Type options : info, danger, success, warning
                    TempData["UserMessage"] = new JavaScriptSerializer().Serialize(new { Type = "warning", Title = "Warning:", Message = "User not registered as mentee" });

                    return View();
                }                
            }
            return View();
        }

        // GET: Users/Delete/5
        [Authorize]
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
        [Authorize]
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

        [Authorize]
        public string getUser(int id)
        {
            Users user = db.Users.Find(id);

            dynamic userObj = new ExpandoObject(); ;
            userObj.user = new { user.FirstName, user.LastName };

            if (user.MentorId != null)
            {
                Users mentor = db.Users.Find(user.MentorId);
                userObj.mentor = new {mentor.FirstName,mentor.LastName,mentor.EmailAddress };
            }
            else
            {
                userObj.mentor = null;
            }

            return (new JavaScriptSerializer().Serialize(new { User = userObj.user, Mentor = userObj.mentor }));
        }
    }
}
