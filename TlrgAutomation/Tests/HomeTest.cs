using TlrgAutomation.Pages.Tlrg;
using NUnit.Framework;
using System.Collections.Generic;
using System.Data;
using System;
using System.Text.RegularExpressions;

namespace TlrgAutomation.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class HomeTest : BaseTest
    {
        [Test, Retry(2), Category("Smoke")]
        public void CustomerSearchFieldDisplayed()
        {
            HomePage homePageElements = new HomePage();
            Assert.That(homePageElements.CustomerSearchField.Displayed, Is.True);
        }
        
        [Test, Retry(2), Category("Smoke")]
        public void LoadsDisplayed()
        {
            int pageLoadIndex = 0;
            HomePage homePage = new HomePage();
            string customerId = env.TlrgCustomerId;
            customerId = (customerId == "") ? dbAccess.GetRandomCustomerHasLoadsRow()["CustomerId"].ToString() : customerId;
            DataTable customerLoadsTable = dbAccess.GetCustomerLoadsTable(customerId);
            homePage.ClickAllTab()
                .SearchAndSelectCustomer(customerId);
            for (int loadIndex = 0; loadIndex < customerLoadsTable.Rows.Count; loadIndex++, pageLoadIndex++)
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
                Assert.AreEqual(expectedLoad, actualLoad);
            }
        }

        [Test, Retry(2), Category("Smoke")]
        public void FilterLoadsById()
        {
            string loadId;
            var rand = new Random();
            HomePage homePage = new HomePage();
            string customerId = env.TlrgCustomerId;
            customerId = (customerId == "") ? dbAccess.GetRandomCustomerHasLoadsRow()["CustomerId"].ToString() : customerId;
            DataTable customerLoadsTable = dbAccess.GetCustomerLoadsTable(customerId);
            DataRow row = customerLoadsTable.Rows[rand.Next(customerLoadsTable.Rows.Count)];
            loadId = row["LOAD ID"].ToString();
            homePage.ClickAllTab()
                .SearchAndSelectCustomer(customerId)
                .FilterLoads(homePage.LoadIdField, loadId);
            IList<string> actualLoad = homePage.GetLoadAtIndex(0);
            Assert.AreEqual(actualLoad[0], loadId);
        }

        [Test, Retry(2), Category("Smoke")]
        public void LoadAuctionDetailsDisplayed()
        {
            var rand = new Random();
            HomePage homePage = new HomePage();
            string customerId = env.TlrgCustomerId;
            customerId = (customerId == "") ? dbAccess.GetRandomCustomerHasLoadsRow()["CustomerId"].ToString(): customerId;
            DataTable customerLoadsTable = dbAccess.GetCustomerLoadsTable(customerId);
            DataRow row = customerLoadsTable.Rows[rand.Next(customerLoadsTable.Rows.Count % 10)];
            string expectedLoadId = row["LOAD ID"].ToString();
            DataTable loadDetailsTable = dbAccess.GetLoadDetailsTable(expectedLoadId);
            homePage.ClickAllTab()
                .SearchAndSelectCustomer(customerId)
                .ClickOnLoad(expectedLoadId);
            string actualLoadId = homePage.GetLoadIdFromAuctionDetails();
            Assert.AreEqual(actualLoadId, expectedLoadId);
            for (int i = 0; i < loadDetailsTable.Rows.Count; i++)
            {
                row = loadDetailsTable.Rows[i];
                string carrierName = row["CARRIER NAME"].ToString();
                string carrierId = row["CARRIER ID"].ToString();
                string actualLoadDetails = homePage.GetLoadDetailsAtIndex(i);
                Assert.IsTrue(actualLoadDetails.Contains(carrierName), actualLoadDetails + " does not contain " + carrierName);
                Assert.IsTrue(actualLoadDetails.Contains(carrierId), actualLoadDetails + " does not contain " + carrierId);
            }
        }

        [Test, Retry(2), Category("Smoke")]
        public void SelectLoads()
        {
            int pageLoadCount = 0;
            var rand = new Random();
            HomePage homePage = new HomePage();
            string customerId = env.TlrgCustomerId;
            customerId = (customerId == "") ? dbAccess.GetRandomCustomerHasLoadsRow()["CustomerId"].ToString() : customerId;
            DataTable customerLoadsTable = dbAccess.GetCustomerLoadsAutoStartTable(customerId);
            homePage.ClickAutoStartTab()
                .SearchAndSelectCustomer(customerId);
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
