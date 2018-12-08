using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HRMasterASP.EntityFramework;
using HRMasterASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMasterASP.Controllers
{
    public class JobApplicationController : Controller
    {
        private readonly DataContext _context;

        public JobApplicationController(DataContext context)
        {
            _context = context;
        }

        // GET: JobApplication/Details/5
        public async Task<ActionResult> Details(int id)
        {
            var modelView = await jobApplicationByIdAsync(id);
            if(modelView == null)
            {
                return NotFound();
            }
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
        public async Task<ActionResult> Create(JobApplicationCreateView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobApplication = new JobApplication()
            {
                OfferId = model.OfferId,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber,
                EmailAddress = model.EmailAddress,
                ContactAgreement = model.ContactAgreement,
                CvUrl = model.CvUrl,
                DateOfBirth = model.DateOfBirth,
                Description = model.Description
            };

            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "JobOffer", new { id = jobApplication.OfferId });
        }

        // GET: JobApplication/Edit/5
        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            return View(await jobApplicationByIdAsync(id));
        }

        // POST: JobApplication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, JobApplication model)
        {
            if(id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return RedirectToAction(nameof(Details), new { model.Id });
        }

        // POST: JobApplication/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var jobApplication = await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == id);
            _context.JobApplications.Remove(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "JobOffer", new { id = jobApplication.OfferId });
        }

        private async Task<JobApplication> jobApplicationByIdAsync(int id)
        {
            return await _context.JobApplications.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}