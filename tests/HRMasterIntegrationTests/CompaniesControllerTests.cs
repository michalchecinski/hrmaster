using FluentAssertions;
using HRMasterASP.Controllers;
using HRMasterASP.EntityFramework;
using HRMasterASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HRMasterIntegrationTests
{
    public class CompaniesControllerTests : IDisposable
    {
        private readonly DataContext _dataContext;
        private readonly string _databaseName;
        private readonly string _testDir;

        public CompaniesControllerTests()
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
        public async Task create_new_company_via_form_should_create_new_entity_in_db()
        {
            // Arrange    
            var appSettings = TestHelpers.GetAppSettings(_testDir);

            var controller = new CompaniesController(_dataContext, TestHelpers.GetIConfiguration(_testDir));

            TestHelpers.SetAdminUser(controller, appSettings);

            var company = new Company()
            {
                Name = "Microsoft"
            };

            // Act
            await controller.Create(company);

            //Assert
            var dbCompanies = await _dataContext.Companies.ToListAsync();
            dbCompanies.Should().HaveCount(1);
            dbCompanies.Should().ContainSingle();
            dbCompanies.Should().Contain(company);
        }

        public void Dispose()
        {
            _dataContext.Database.ExecuteSqlCommand((string)$"USE [master] DROP DATABASE [{_databaseName}]");
        }
    }
}
