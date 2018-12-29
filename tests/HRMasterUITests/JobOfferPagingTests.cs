using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace HRMasterUITests
{
    public class JobOfferPagingTests : IDisposable
    {

        IWebDriver _driver;

        public JobOfferPagingTests()
        {
            _driver = new ChromeDriver("C:/");
        }

        [Fact]
        public void different_link_first_joboffers_on_first_and_second_page()
        {
            _driver.Navigate().GoToUrl("https://localhost:44373/JobOffer/Index");
            _driver.Manage().Window.Maximize();

            IWebElement firstPageFirstJob = _driver.FindElement(By.XPath("//*[@id='table-body']/tr[1]/td[1]/a"));
            var firstHref = firstPageFirstJob.GetAttribute("href");

            IWebElement nextPageButton = _driver.FindElement(By.XPath("//li[@id='next-page']/a"));
            nextPageButton.Click();

            System.Threading.Thread.Sleep(2000);

            IWebElement secondPageFirstJob = _driver.FindElement(By.XPath("//*[@id='table-body']/tr[1]/td[1]/a"));
            var secondHref = secondPageFirstJob.GetAttribute("href");

             Assert.NotEqual(secondHref, firstHref);
        }

        public void Dispose()
        {
            _driver.Dispose();
        }
    }
}
