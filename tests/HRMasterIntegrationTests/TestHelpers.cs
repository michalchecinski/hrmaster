using HRMasterASP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HRMasterIntegrationTests
{
    public class TestHelpers
    {
        internal static IConfiguration GetIConfiguration(string outputPath)
        {
            return new ConfigurationBuilder()
                            .SetBasePath(outputPath)
                            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                            .Build();

        }

        internal static AppSettings GetAppSettings(string outputPath)
        {
            return GetIConfiguration(outputPath).GetSection("AppSettings").Get<AppSettings>();
        }

        internal static string GetTestPath(string relativePath)
        {
            var codeBaseUrl = new Uri(Assembly.GetExecutingAssembly().CodeBase);
            var codeBasePath = Uri.UnescapeDataString(codeBaseUrl.AbsolutePath);
            var dirPath = Path.GetDirectoryName(codeBasePath);
            return Path.Combine(dirPath, relativePath);
        }

        internal static void SetAdminUser(Controller controller, AppSettings appSettings)
        {
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                 new Claim(appSettings.UserIdClaimName, GetIConfiguration(GetTestPath("")).GetSection("TestData")["AdminUserClaim"])
            }));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
        }

        internal static IFormFile GetIFormFile()
        {
            var filePath = GetTestPath("Doc.pdf");
            var file = File.OpenRead(filePath);
           return new FormFile(file, 0, file.Length, "Doc.pdf", filePath);
        }
    }
}
