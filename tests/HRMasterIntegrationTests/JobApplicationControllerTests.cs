using FluentAssertions;
using HRMasterASP.Controllers;
using HRMasterASP.EntityFramework;
using HRMasterASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HRMasterIntegrationTests
{
    public class JobApplicationControllerTests : IDisposable
    {
        private readonly DataContext _dataContext;
        private readonly string _databaseName;
        private readonly string _testDir;

        public JobApplicationControllerTests()
        {
            _databaseName = Guid.NewGuid().ToString();

            var options = new DbContextOptionsBuilder<DataContext>()
                      .UseSqlServer($"Server=(localdb)\\MSSQLLocalDB;" +
                                    $"Trusted_Connection=True;Database={_databaseName};")
                      .Options;

            _dataContext = new DataContext(options);
            _dataContext.Database.Migrate();

            _testDir = TestHelpers.GetTestPath("");
        }

        [Fact]
        public async Task create_new_jobapplication_via_form_should_create_new_entity_in_db()
        {
            // Arrange
            var company = new Company()
            {
                Name = "Microsoft"
            };

            _dataContext.Companies.Add(company);
            await _dataContext.SaveChangesAsync();

            var jobOffer = new JobOffer()
            {
                Company = company,
                Created = new DateTime(2018, 12, 12),
                CompanyId = _dataContext.Companies.FirstOrDefault().Id,
                JobTitle = "Test job offer",
                Location = "Warsaw",
                SalaryFrom = 2600,
                SalaryTo = 3500,
                ValidUntil = new DateTime(2019, 12, 12),
                Description = "Job offer description"
            };

            _dataContext.JobOffers.Add(jobOffer);
            await _dataContext.SaveChangesAsync();

            var jobApplication = new JobApplicationCreateView()
            {
                ContactAgreement = true,
                CvFile = TestHelpers.GetIFormFile(),
                DateOfBirth = new System.DateTime(1997, 01, 07),
                Description = "Description of the job application",
                EmailAddress = "test@test.com",
                FirstName = "First",
                LastName = "Last",
                JobOfferName = jobOffer.JobTitle,
                OfferId = _dataContext.JobOffers.FirstOrDefault().Id,
                PhoneNumber = "123456789"
            };

            var appSettings = TestHelpers.GetAppSettings(_testDir);
            var controller = new JobApplicationController(_dataContext, TestHelpers.GetIConfiguration(_testDir));


            // Act
            await controller.Create(jobApplication);

            //Assert
            var dbJobApplications = await _dataContext.JobApplications.ToListAsync();
            dbJobApplications.Should().HaveCount(1);
            dbJobApplications.Should().ContainSingle();
            dbJobApplications.Should().Contain(jobApplication);
        }

        public void Dispose()
        {
            _dataContext.Database.ExecuteSqlCommand((string)$"USE [master] DROP DATABASE [{_databaseName}]");
        }
    }
}
