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
        private DiaryEntriesContainer db = new DiaryEntriesContainer();

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

        [Authorize(Roles = "Mentor")]        
        public ActionResult Landingpage(int Id)
        {
            ViewBag.id = Id;            
            return View();
        }
    }
}
