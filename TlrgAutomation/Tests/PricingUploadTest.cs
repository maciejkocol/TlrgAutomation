using TlrgAutomation.Pages.Tlrg;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using System.Data;
using OpenQA.Selenium.Support.UI;

namespace TlrgAutomation.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class PricingUploadTest : BaseTest
    {
        [Test, Retry(2), Category("Smoke")]
        public void RateUploadTest()
        {
            DataRow customerRow = dbAccess.GetRandomSyncedCustomerRow();
            DataRow carrierRow = dbAccess.GetRandomSyncedCarrierRow();
            string customerId = customerRow["CustomerId"].ToString();
            string carrierId = carrierRow["CarrierId"].ToString();
            SettingsPage settingsPageElements = new SettingsPage();
            settingsPageElements.ClickCustomerSettingsGeneralLink()
                .SearchAndSelectCustomer(customerId)
                .SetCustomerGeneralFields()
                .SetTimeZone("(GMT -6:00) Central Time (US & Canada), Mexico City")
                .ClickSaveButton()
                .WaitForSaveConfirmation()
                .Refresh();
            PricingUploadPage pricingUploadPage = new PricingUploadPage();
            string expectedLineHaulName = "Test" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            pricingUploadPage.UploadLineHaulRates(customerId, carrierId, expectedLineHaulName);
            string expectedLastUpdated = DateTime.Now.ToString("MM-dd-yy HH:mm");
            string actualLastUpdated = pricingUploadPage.LineHaulLastUpdated.Text.Replace("Last Updated: ", "");
            string actualLineHaulName = pricingUploadPage.LineHaulSchedulesFieldOptions[0].Text;
            Assert.AreEqual(expectedLineHaulName, actualLineHaulName);
            //Assert.That((DateTime.Parse(expectedLastUpdated) - DateTime.Parse(actualLastUpdated)).TotalMinutes, 
            //    Is.LessThanOrEqualTo(1), "Last Updated time is not within the last minute\n");
            Assert.That(pricingUploadPage.UploadSuccessfulMessage.Displayed, 
                Is.True, "'Upload successful' message is missing\n");
        }

        [Test, Retry(2), Category("Smoke")]
        public void FuelUploadTest()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            DataRow customerRow = dbAccess.GetRandomSyncedCustomerRow();
            DataRow carrierRow = dbAccess.GetRandomSyncedCarrierRow();
            string customerId = customerRow["CustomerId"].ToString();
            string carrierId = carrierRow["CarrierId"].ToString();
            SettingsPage settingsPageElements = new SettingsPage();
            settingsPageElements.ClickCustomerSettingsGeneralLink()
                .SearchAndSelectCustomer(customerId)
                .SetCustomerGeneralFields()
                .SetTimeZone("(GMT -6:00) Central Time (US & Canada), Mexico City")
                .ClickSaveButton()
                .WaitForSaveConfirmation()
                .Refresh();
            PricingUploadPage pricingUploadPage = new PricingUploadPage();
            pricingUploadPage
                .UploadFuelSchedule(customerId, carrierId);
            Assert.That(pricingUploadPage.UploadSuccessfulMessage.Displayed, Is.True, "'Upload successful' message is missing\n");
            string expectedLastUpdated = DateTime.Now.ToString("MM-dd-yy HH:mm");
            Wait.Until(d => pricingUploadPage.UseEchoFuelTableButton.Enabled);
            string actualLastUpdated = pricingUploadPage.FuelLastUpdated.Text.Replace("Last Updated: ", "");
            //Assert.That((DateTime.Parse(expectedLastUpdated) - DateTime.Parse(actualLastUpdated)).TotalMinutes,
            //    Is.LessThanOrEqualTo(1), "Last Updated time is not within the last minute\n");
            pricingUploadPage
                .UseEchoFuelTableButton.Click();
            pricingUploadPage.ConfirmUploadButton.Click();
            Assert.True(Wait.Until(d => !pricingUploadPage.UseEchoFuelTableButton.Enabled));
        }

        [Test, Retry(2), Category("Smoke")]
        public void SpecialServiceUploadTest()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            DataRow customerRow = dbAccess.GetRandomSyncedCustomerRow();
            DataRow carrierRow = dbAccess.GetRandomSyncedCarrierRow();
            string customerId = customerRow["CustomerId"].ToString();
            string carrierId = carrierRow["CarrierId"].ToString();
            SettingsPage settingsPageElements = new SettingsPage();
            settingsPageElements.ClickCustomerSettingsGeneralLink()
                .SearchAndSelectCustomer(customerId)
                .SetCustomerGeneralFields()
                .SetTimeZone("(GMT -6:00) Central Time (US & Canada), Mexico City")
                .ClickSaveButton()
                .WaitForSaveConfirmation()
                .Refresh();
            PricingUploadPage pricingUploadPage = new PricingUploadPage();
            pricingUploadPage
                .UploadSpecialServices(customerId, carrierId);
            Assert.That(pricingUploadPage.UploadSuccessfulMessage.Displayed, Is.True, "'Upload successful' message is missing\n");
            string expectedLastUpdated = DateTime.Now.ToString("MM-dd-yy HH:mm");
            Wait.Until(d => pricingUploadPage.ClearSpecialServicesButton.Enabled);
            string actualLastUpdated = pricingUploadPage.ServicesLastUpdated.Text.Replace("Last Updated: ", "");
            //Assert.That((DateTime.Parse(expectedLastUpdated) - DateTime.Parse(actualLastUpdated)).TotalMinutes,
            //    Is.LessThanOrEqualTo(1), "Last Updated time is not within the last minute\n");
            pricingUploadPage
                .ClearSpecialServicesButton.Click();
            Assert.True(Wait.Until(d => !pricingUploadPage.ClearSpecialServicesButton.Enabled));
        }
    }
}
