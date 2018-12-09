using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HRMasterASP.EntityFramework;
using HRMasterASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMasterASP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOfferSearchController : Controller
    {
        private readonly DataContext _context;

        public JobOfferSearchController(DataContext context)
        {
            _context = context;
            foreach (var offer in _context.JobOffers)
            {
                offer.Company = _context.Companies.FirstOrDefault(x => x.Id == offer.CompanyId);
                offer.JobApplications = _context.JobApplications.Where(x => x.OfferId == offer.Id).ToList();
            }
        }


        [HttpGet]
        public async Task<IEnumerable<JobOffer>> Index([FromQuery(Name = "search")] string searchString)
        {
            var jobOffers = await _context.JobOffers.ToListAsync();

            if (String.IsNullOrEmpty(searchString))
                return jobOffers;

            List<JobOffer> searchResult = jobOffers.FindAll(o => o.JobTitle.ToLower().Contains(searchString.ToLower()));
            return searchResult;
        }
    }
}