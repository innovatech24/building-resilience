using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Resilience.Controllers
{
    public class OptionsController : Controller
    {
        // GET: Options
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Choice()
        {
            return View();
        }

        public ActionResult Mentee()
        {
            return View();
        }

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
