using HRMasterASP.Controllers;
using HRMasterASP.EntityFramework;
using HRMasterASP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System.IO;
using Xunit;
using Shouldly;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HRMasterUnitTests
{
    public class JobApplicationControllerTests
    {
        [Fact]
        public async Task job_appliaction_create_valid_model_should_return_joboffer_details_pageAsync()
        {
            var mockRepo = new Mock<DataContext>();
            var mockConfig = new Mock<IConfiguration>();

            var controller = new JobApplicationController(mockRepo.Object, mockConfig.Object);

            var file = TestHelpers.CreateIFormFileMock();

            var jobApplication = new JobApplicationCreateView()
            {
                ContactAgreement = true,
                CvFile = file.Object,
                DateOfBirth = new System.DateTime(1997, 01, 07),
                Description = TestHelpers.GetLoremIpsum(),
                EmailAddress = "test@test.com",
                FirstName = "First",
                LastName = "Last",
                JobOfferName = "Test job offer",
                OfferId = 1,
                PhoneNumber = "123456789"
            };

            var result = await controller.Create(jobApplication);
            var redirectToActionResult = result.ShouldBeOfType<RedirectToActionResult>();
            redirectToActionResult.ControllerName.ShouldBe("JobOffer");
            redirectToActionResult.ActionName.ShouldBe("Details");
            mockRepo.Verify();
        }

        [Fact]
        public async Task job_appliaction_create_model_without_firstname_and_validation_error_should_return_view_with_same_model()
        {
            var mockRepo = new Mock<DataContext>();
            var mockConfig = new Mock<IConfiguration>();

            var controller = new JobApplicationController(mockRepo.Object, mockConfig.Object);

            var jobApplication = new JobApplicationCreateView()
            {
                ContactAgreement = true,
                CvFile = TestHelpers.CreateIFormFileMock().Object,
                DateOfBirth = new System.DateTime(1997, 01, 07),
                Description = TestHelpers.GetLoremIpsum(),
                EmailAddress = "test@test.com",
                FirstName = "",
                LastName = "Last",
                JobOfferName = "Test job offer",
                OfferId = 1,
                PhoneNumber = "123456789"
            };

            controller.ModelState.AddModelError("FirstName", "First Name is Required");
            var result = await controller.Create(jobApplication);
            var viewResult = result.ShouldBeOfType<ViewResult>();
            viewResult.Model.ShouldBeOfType<JobApplicationCreateView>();
            viewResult.Model.ShouldBeSameAs(jobApplication);
            mockRepo.Verify();
        }
    }
}
