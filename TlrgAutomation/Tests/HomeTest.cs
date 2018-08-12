using Tlrg.Pages.Tlrg;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System;

namespace Tlrg.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class HomeTest: BaseTest
    {

        [Test, Retry(2), TestCategory("Smoke")]
        public void CustomerSearchFieldDisplayed()
        {
            HomePage homePageElements = new HomePage(driver);
            Assert.That(homePageElements.CustomerSearchField.Displayed, Is.True);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void GridComponentDisplayed()
        {
            HomePage homePageElements = new HomePage(driver);
            Assert.That(homePageElements.GridComponent.Displayed, Is.True);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void LoadsDisplayed()
        {
            string customerId;
            int pageLoadIndex = 0;
            HomePage homePage = new HomePage(driver);
            customerId = env.TlrgCustomerId;
            if (customerId == "")
            {
                DataRow customerRow = homePage.GetRandomCustomerHasLoadsRow();
                customerId = customerRow["CustomerId"].ToString();
            }
            DataTable customerLoadsTable = homePage.GetCustomerLoadsTable(customerId);
            homePage.ClickAllTab();
            homePage.SearchAndSelectCustomer(customerId);
            for (int loadIndex=0; loadIndex < customerLoadsTable.Rows.Count; loadIndex++, pageLoadIndex++)
            {
                if (loadIndex != 0 && loadIndex % 10 == 0 && loadIndex < customerLoadsTable.Rows.Count)
                {
                    homePage.NextPage();
                    pageLoadIndex = 0;
                }
                DataRow row = customerLoadsTable.Rows[loadIndex];
                IList<string> expectedLoad = new List<string> {
                    row["LOAD ID"].ToString(),
                    row["CARRIER"].ToString(),
                    row["CARRIER RANK"].ToString(),
                    row["COGS"].ToString(),
                    row["STATUS"].ToString(),
                    row["ORIG CITY"].ToString(),
                    row["ORIG ST"].ToString(),
                    row["DEST CITY"].ToString(),
                    row["DEST ST"].ToString(),
                    row["PU DATE"].ToString()
                };
                IList<string> actualLoad = homePage.GetLoadAtIndex(pageLoadIndex);
                Assert.AreEqual(actualLoad, expectedLoad);
            }
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void FilterLoadsById()
        {
            string customerId;
            string loadId;
            var rand = new Random();
            HomePage homePage = new HomePage(driver);
            customerId = env.TlrgCustomerId;
            if (customerId == "")
            {
                DataRow customerRow = homePage.GetRandomCustomerHasLoadsRow();
                customerId = customerRow["CustomerId"].ToString();
            }
            DataTable customerLoadsTable = homePage.GetCustomerLoadsTable(customerId);
            DataRow row = customerLoadsTable.Rows[rand.Next(customerLoadsTable.Rows.Count)];
            loadId = row["LOAD ID"].ToString();
            homePage.ClickAllTab();
            homePage.SearchAndSelectCustomer(customerId);
            homePage.FilterLoads(homePage.LoadIdField, loadId);
            IList<string> actualLoad = homePage.GetLoadAtIndex(0);
            Assert.AreEqual(actualLoad[0], loadId);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void LoadDetailsDisplayed()
        {
            string customerId;
            string loadId;
            var rand = new Random();
            HomePage homePage = new HomePage(driver);
            customerId = env.TlrgCustomerId;
            if (customerId == "")
            {
                DataRow customerRow = homePage.GetRandomCustomerHasLoadsRow();
                customerId = customerRow["CustomerId"].ToString();
            }
            DataTable customerLoadsTable = homePage.GetCustomerLoadsTable(customerId);
            DataRow row = customerLoadsTable.Rows[rand.Next(customerLoadsTable.Rows.Count % 10)];
            loadId = row["LOAD ID"].ToString();
            homePage.ClickAllTab();
            homePage.SearchAndSelectCustomer(customerId);
            homePage.ClickOnLoad(loadId);
            DataTable loadDetailsTable = homePage.GetLoadDetailsTable(loadId);
            Assert.AreEqual(homePage.LoadDetails.Text, loadId);
            for (int i = 0; i < loadDetailsTable.Rows.Count; i++)
            {
                row = loadDetailsTable.Rows[i];
                string carrierName = row["CARRIER NAME"].ToString();
                string cogs = row["COGS"].ToString();
                string carrierId = row["CARRIER ID"].ToString();
                string actualLoadDetails = homePage.GetLoadDetailsAtIndex(i);
                Assert.IsTrue(actualLoadDetails.Contains(carrierName), actualLoadDetails + " does not contain " + carrierName);
                Assert.IsTrue(actualLoadDetails.Contains(cogs), actualLoadDetails + " does not contain " + cogs);
                Assert.IsTrue(actualLoadDetails.Contains(carrierId), actualLoadDetails + " does not contain " + carrierId);
            }
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void SelectLoads()
        {
            string customerId;
            int pageLoadCount = 0;
            var rand = new Random();
            HomePage homePage = new HomePage(driver);
            customerId = env.TlrgCustomerId;
            if (customerId == "")
            {
                DataRow customerRow = homePage.GetRandomCustomerHasLoadsRow();
                customerId = customerRow["CustomerId"].ToString();
            }
            DataTable customerLoadsTable = homePage.GetCustomerLoadsAutoStartTable(customerId);
            homePage.ClickAutoStartTab();
            homePage.SearchAndSelectCustomer(customerId);
            for (int i = 0; i < customerLoadsTable.Rows.Count; i++)
            {
                DataRow row = customerLoadsTable.Rows[i];
                string loadId = row["LOAD ID"].ToString();
                homePage.SelectLoad(loadId);
                pageLoadCount++;
                if ((i + 1) % 10 == 0 && i + 1 < customerLoadsTable.Rows.Count)
                {
                    break;
                }
            }
            Assert.AreEqual(homePage.CheckedResultsCount(), pageLoadCount);
            homePage.ClickSelectAllCheckbox();
            Assert.AreEqual(homePage.CheckedResultsCount(), 0);
        }
    }
}
