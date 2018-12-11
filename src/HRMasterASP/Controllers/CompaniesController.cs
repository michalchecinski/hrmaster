using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRMasterASP.EntityFramework;
using HRMasterASP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using HRMasterASP.Helpers;
using System.Net;

namespace HRMasterASP.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CompaniesController : Controller
    {
        private readonly DataContext _context;

        private IConfiguration _configuration;
        private AppSettings AppSettings { get; set; }

        public CompaniesController(DataContext context, IConfiguration Configuration)
        {
            _configuration = Configuration;
            AppSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();

            _context = context;
        }

        // GET: Companies
        [Authorize]
        public async Task<IActionResult> Index()
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            return View(await _context.Companies.ToListAsync());
        }

        // GET: Companies/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // GET: Companies/Create
        [Authorize]
        public async Task<IActionResult> Create()
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            return View();
        }

        // POST: Companies/Create
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Company company)
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            if (ModelState.IsValid)
            {
                _context.Add(company);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }
            return View(company);
        }

        // POST: Companies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Company company)
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            if (id != company.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(company);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CompanyExists(company.Id))
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
            return View(company);
        }

        // GET: Companies/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            if (id == null)
            {
                return NotFound();
            }

            var company = await _context.Companies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (company == null)
            {
                return NotFound();
            }

            return View(company);
        }

        // POST: Companies/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await isUserAdminAsync())
            {
                return RedirectToAction("NotAllowed", "Session");
            }
            var company = await _context.Companies.FindAsync(id);
            _context.Companies.Remove(company);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CompanyExists(int id)
        {
            return _context.Companies.Any(e => e.Id == id);
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
