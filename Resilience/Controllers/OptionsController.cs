using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Resilience.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Resilience.Controllers
{
    public class OptionsController : Controller
    {
        private ApplicationUserManager _userManager;

        public OptionsController()
        {

        }

        public OptionsController(ApplicationUserManager userManager)
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

        // GET: Options
        [Authorize]
        public ActionResult Index()
        {
            ApplicationUser user = System.Web.HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>().FindById(System.Web.HttpContext.Current.User.Identity.GetUserId<int>());
            var roles = UserManager.GetRoles(user.Id);

            if (roles.Count == 2)
            {
                return RedirectToAction("Choice");
            }
            else
            {
                if (roles.Contains("Mentor"))
                {
                    return RedirectToAction("Mentor");
                }
                else
                {
                    return RedirectToAction("Mentee");
                }
            }
        }

        [Authorize(Roles = "Mentor")]
        public ActionResult Choice()
        {
            return View();
        }

        [Authorize(Roles = "Mentee")]
        public ActionResult Mentee()
        {
            return View();
        }

        [Authorize(Roles = "Mentor")]
        public ActionResult Mentor()
        {
            return View();
        }

        /*
        // GET: Options/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Options/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Options/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Options/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Options/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Options/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Options/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        */
    }
}
