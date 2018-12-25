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
    public class CompaniesApiController : ControllerBase
    {
        private readonly DataContext _context;

        public CompaniesApiController(DataContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<PagingViewModel<Company>> GetCompanies([FromQuery(Name = "search")] string searchString = "", [FromQuery(Name = "pageNo")] int pageNo = -1)
        {
            var companies = _context.Companies;

            if (String.IsNullOrEmpty(searchString) && pageNo == -1)
            {
                var set = await companies.ToListAsync();
                return new PagingViewModel<Company>
                {
                    Set = set,
                    TotalPage = 1
                };
            }

            var searchResult = companies.Where(o => o.Name.ToLower().Contains(searchString.ToLower()));

            if (pageNo == -1)
            {
                return new PagingViewModel<Company>
                {
                    Set = searchResult.ToList(),
                    TotalPage = 1
                };
            }

            var pageSize = 4;

            var totalRecord = searchResult.Count();
            var totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
            var record = await searchResult.OrderBy(x => x.Name)
                                           .Skip((pageNo - 1) * pageSize)
                                           .Take(pageSize)
                                           .ToListAsync();

            var empData = new PagingViewModel<Company>
            {
                Set = record,
                TotalPage = totalPage
            };

            return empData;
        }
    }
}