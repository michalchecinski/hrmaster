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
    public class JobOfferApiController : Controller
    {
        private readonly DataContext _context;

        public JobOfferApiController(DataContext context)
        {
            _context = context;
            foreach (var offer in _context.JobOffers)
            {
                offer.Company = _context.Companies.FirstOrDefault(x => x.Id == offer.CompanyId);
                offer.JobApplications = _context.JobApplications.Where(x => x.OfferId == offer.Id).ToList();
            }
        }


        [HttpGet]
        public async Task<PagingViewModel<JobOffer>> Index([FromQuery(Name = "search")] string searchString = "", [FromQuery(Name = "pageNo")] int pageNo = -1)
        {
            var jobOffers = _context.JobOffers;

            if (String.IsNullOrEmpty(searchString) && pageNo == -1)
            {
                var set = await jobOffers.ToListAsync();
                return new PagingViewModel<JobOffer>
                {
                    Set = set,
                    TotalPage = 1
                };
            }

            var searchResult = jobOffers.Where(o => o.JobTitle.ToLower().Contains(searchString.ToLower()));

            if (pageNo == -1)
            {
                return new PagingViewModel<JobOffer>
                {
                    Set = searchResult.ToList(),
                    TotalPage = 1
                };
            }

            var pageSize = 4;

            var totalRecord = searchResult.Count();
            var totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var record = await searchResult.OrderBy(x => x.JobTitle)
                                           .Skip((pageNo - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            var empData = new PagingViewModel<JobOffer>
            {
                Set = record,
                TotalPage = totalPage
            };

            return empData;
        }
    }
}