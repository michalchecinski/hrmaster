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

namespace HRMasterASP.Controllers
{
    [Route("[controller]/[action]")]
    public class JobOfferController : Controller
    {
        private readonly DataContext _context;

        public JobOfferController(DataContext context)
        {
            _context = context;
            foreach (var offer in _context.JobOffers)
            {
                offer.Company = _context.Companies.FirstOrDefault(x => x.Id == offer.CompanyId);
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

        [HttpPost]
        public async Task<ActionResult> Edit(int? id, JobOffer jobOffer)
        {
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid) return View();
            var offer = await _context.JobOffers.FirstOrDefaultAsync(j => j.Id == model.Id);
            offer.JobTitle = model.JobTitle;
            return RedirectToAction("Details", new { id = model.Id });
        }
        [HttpPost]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            _context.JobOffers.Remove(await _context.JobOffers.FirstOrDefaultAsync(x => x.Id == id));
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<ActionResult> Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = await _context.Companies.ToListAsync()
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(JobOfferCreateView model)
        {
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
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var offer = await _context.JobOffers.FirstOrDefaultAsync(o => o.Id == id);
            return View(offer);
        }

    }
}