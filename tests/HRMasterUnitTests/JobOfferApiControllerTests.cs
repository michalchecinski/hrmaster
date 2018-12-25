using HRMasterASP.Controllers;
using HRMasterASP.EntityFramework;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using HRMasterASP.Models;
using Xunit;
using FluentAssertions;

namespace HRMasterUnitTests
{
    public class JobOfferApiControllerTests
    {
        [Fact]
        public async Task job_offer_api_get_without_search_string_should_return_all_job_offers()
        {
            var company = new Company() { Id = 1, Name = "Microsoft" };

            var jobOfferList = new List<JobOffer>()
            {
                new JobOffer()
                {
                    Id = 1,
                    Company = company,
                    CompanyId = 1,
                    Created = new DateTime(2018, 12, 12),
                    Description = TestHelpers.GetLoremIpsum(),
                    JobApplications = new List<JobApplication>(),
                    JobTitle = "title",
                    Location = "Warsaw",
                    SalaryFrom = 2600,
                    SalaryTo = 3200,
                    ValidUntil = new DateTime(2019,3,22)
                }
            };

            var jobApplicationList = new List<JobApplication>();

            var companiesList = new List<Company>()
            {
                company
            };

            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase()
                .Options;

            using (var context = new DataContext(options))
            {
                context.JobOffers.AddRange(jobOfferList);
                context.JobApplications.AddRange(jobApplicationList);
                context.Companies.AddRange(companiesList);
                context.SaveChanges();
            }

            // Use a clean instance of the context to run the test
            using (var context = new DataContext(options))
            {
                var controller = new JobOfferApiController(context);
                var result = await controller.Index();
                var resultSet = result.Set;

                resultSet.Count().Should().Be(jobOfferList.Count);
                resultSet.FirstOrDefault().Should().BeOfType<JobOffer>();
                resultSet.Should().BeEquivalentTo(jobOfferList);
            }
        }
    }
}
