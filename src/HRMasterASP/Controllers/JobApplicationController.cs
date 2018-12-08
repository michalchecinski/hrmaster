using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMasterASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HRMasterASP.Controllers
{
    public class JobApplicationController : Controller
    {
        // GET: JobApplication/Details/5
        public ActionResult Details(int id)
        {
            var modelView = new JobApplication();
            return View(modelView);
        }

        // GET: JobApplication/Create
        public ActionResult Create(int id, string jobTitle)
        {
            var modelView = new JobApplicationCreateView();
            modelView.OfferId = id;
            modelView.JobOfferName = jobTitle;
            return View(modelView);
        }

        // POST: JobApplication/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(JobApplicationCreateView jobApplication)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Details", "JobOffer", new { id = jobApplication.OfferId });
            }
            catch
            {
                return View();
            }
        }

        // GET: JobApplication/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: JobApplication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Details));
            }
            catch
            {
                return View();
            }
        }

        // GET: JobApplication/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: JobApplication/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Details", "JobOffer", new { id = 0 });
            }
            catch
            {
                return View();
            }
        }
    }
}