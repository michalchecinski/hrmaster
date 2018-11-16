using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HRMasterASP.Models;
using HRMasterASP.Models;
using Microsoft.AspNetCore.Mvc;

namespace HRMasterASP.Controllers
{
    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {
        private static List<Company> _companies = new List<Company>
        {
            new Company() { Id = 1, Name = "Predica"},
            new Company() { Id = 2, Name = "Microsoft"},
            new Company() { Id = 3, Name = "GitHub"}
        };

        private static List<JobOffer> _jobOffers = new List<JobOffer>
        {
            new JobOffer{
                Id =1,
                JobTitle = "Backend Developer",
                Company = _companies.FirstOrDefault(c => c.Name =="Predica"),
                Created = DateTime.Now.AddDays(-2),
                Description = "Backend C# developer with intrests about IoT solutions. The main task would be building API which expose data from phisical devices. Description need to have at least 100 characters so I am adding some. In test case I reccomend you to use Lorem Impsum.",
                Location = "Poland",
                SalaryFrom = 2000,
                SalaryTo = 10000,
                ValidUntil = DateTime.Now.AddDays(20)
            },
            new JobOffer{
                Id =2,
                JobTitle = "Frontend Developer",
                Company = _companies.FirstOrDefault(c => c.Name =="Microsoft"),
                Created = DateTime.Now.AddDays(-2),
                Description = "Developing Office 365 front end interface. Working with SharePoint and graph API. Connecting with AAD and building ML for Mailbox smart assistant. Description need to have at least 100 characters so I am adding some. In test case I reccomend you to use Lorem Impsum.",
                Location = "Poland",
                SalaryFrom = 2000,
                SalaryTo = 10000,
                ValidUntil = DateTime.Now.AddDays(20)
            }
        };

        [HttpGet]
        public IActionResult Index([FromQuery(Name = "search")] string searchString)
        {
            if (String.IsNullOrEmpty(searchString))
                return View(_jobOffers);

            List<JobOffer> searchResult = _jobOffers.FindAll(o => o.JobTitle.Contains(searchString));
            return View(searchResult);
        }
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var offer = _jobOffers.Find(j => j.Id == id);
            if (offer == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            return View(offer);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid) return View();
            var offer = _jobOffers.Find(j => j.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            return RedirectToAction("Details", new { id = model.Id });
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            _jobOffers.RemoveAll(j => j.Id == id);
            return RedirectToAction("Index");
        }
        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = _companies
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = _companies;
                return View(model);
            }
            var id = _jobOffers.Max(j => j.Id) + 1;
            _jobOffers.Add( new JobOffer
                {
                    Id = id,
                    CompanyId = model.CompanyId,
                    Company = _companies.FirstOrDefault(c => c.Id == model.CompanyId),
                    Description = model.Description,
                    JobTitle = model.JobTitle,
                    Location = model.Location,
                    SalaryFrom = model.SalaryFrom,
                    SalaryTo = model.SalaryTo,
                    ValidUntil = model.ValidUntil,
                    Created = DateTime.Now
                }
            );
            return RedirectToAction("Index");
        }
        public IActionResult Details(int id)
        {
            var offer = _jobOffers.FirstOrDefault(o => o.Id == id);
            return View(offer);
        }

    }
}