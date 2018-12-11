using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HRMasterASP.EntityFramework;
using HRMasterASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace HRMasterASP.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class JobApplicationController : Controller
    {
        private readonly DataContext _context;
        private IConfiguration _configuration;

        public JobApplicationController(DataContext context, IConfiguration Configuration)
        {
            _configuration = Configuration;
            _context = context;
        }

        // GET: JobApplication/Details/5
        [Authorize]
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
        [Authorize]
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
        [Authorize]
        public async Task<ActionResult> Create(JobApplicationCreateView model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var jobApplication = (JobApplication)model;
            jobApplication.CvUrl = await UploadFileToBlob(model.CvFile);
            jobApplication.Id = 0;

            _context.JobApplications.Add(jobApplication);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "JobOffer", new { id = jobApplication.OfferId });
        }

        // GET: JobApplication/Edit/5
        [HttpGet]
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            return View(await jobApplicationByIdAsync(id));
        }

        // POST: JobApplication/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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
        [Authorize]
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

        private async Task<string> UploadFileToBlob(IFormFile file)
        {
            string connectionString = _configuration.GetConnectionString("BlobStorageCV");

            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);

                CloudStorageAccount storageAccount = null;
                if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
                {
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    CloudBlobContainer container = cloudBlobClient.GetContainerReference("applications");
                    await container.CreateIfNotExistsAsync();

                    CloudBlockBlob blockBlob = container.GetBlockBlobReference(file.FileName);

                    // Upload the file
                    await blockBlob.UploadFromStreamAsync(memoryStream);
                    return blockBlob.Uri.ToString();
                }
            }
            return null;
        }
    }
}