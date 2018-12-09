using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRMasterASP.EntityFramework;
using HRMasterASP.Models;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using HRMasterASP.Helpers;
using Microsoft.Extensions.Configuration;

namespace HRMasterASP.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {
        private readonly DataContext _context;

        private IConfiguration _configuration;
        private AppSettings AppSettings { get; set; }

        public JobOfferController(DataContext context, IConfiguration Configuration)
        {
            _configuration = Configuration;
            AppSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();

            _context = context;
            foreach (var offer in _context.JobOffers)
            {
                offer.Company = _context.Companies.FirstOrDefault(x => x.Id == offer.CompanyId);
                offer.JobApplications = _context.JobApplications.Where(x => x.OfferId == offer.Id).ToList();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            var jobOffers = await _context.JobOffers.ToListAsync();

            if (String.IsNullOrEmpty(searchString))
                return View(jobOffers);

            List<JobOffer> searchResult = jobOffers.FindAll(o => o.JobTitle.Contains(searchString));
            return View(searchResult);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Edit(int? id, JobOffer jobOffer)
        {
            if(!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            if (id != jobOffer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobOffer);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.JobOffers.Any(x => x.Id == jobOffer.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(jobOffer);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            if (!ModelState.IsValid)
            {
                return View();
            }
            var offer = await _context.JobOffers.FirstOrDefaultAsync(j => j.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            return RedirectToAction("Details", new { id = model.Id });
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id)
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _context.JobOffers.Remove(await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == id));
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            var model = new JobOfferCreateView
            {
                Companies = await _context.Companies.ToListAsync()
            };
            return View(model);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            if (!ModelState.IsValid)
            {
                model.Companies = await _context.Companies.ToListAsync();
                return View(model);
            }
            _context.JobOffers.Add(new JobOffer
            {
                CompanyId = model.CompanyId,
                Company = _context.Companies.FirstOrDefault(c => c.Id == model.CompanyId),
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            });
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            ViewData["IsAdmin"] = await isUserAdminAsync();
            var offer = await _context.JobOffers.FirstOrDefaultAsync(o => o.Id == id);
            return View(offer);
        }

        private async Task<bool> isUserAdminAsync()
        {
            AADGraph graph = new AADGraph(AppSettings);
            string groupName = "Admins";
            string groupId = AppSettings.AADGroups.FirstOrDefault(g => String.Compare(g.Name, groupName) == 0).Id;
            return await graph.IsUserInGroup(User.Claims, groupId);
        }

    }
}