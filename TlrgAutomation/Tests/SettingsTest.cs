using TlrgAutomation.Pages.Tlrg;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using System.Data;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace TlrgAutomation.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class SettingsTest : BaseTest
    {
        [Test, Retry(2), Category("Smoke")]
        public void CustomerSettingsInitialSetup()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow customerRow = dbAccess.GetRandomSyncedCustomerRow();
            string customerId = customerRow["CustomerId"].ToString();
            Random rnd = new Random();
            string carrierResponseTime = rnd.Next(1, 10).ToString();
            string schedulingLeadTime = rnd.Next(100, 150).ToString();
            string submissionDeadline = rnd.Next(5, 20).ToString();
            string notificationEmail = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            settingsPage.SearchAndSelectCustomer(customerId)
                .ClickCustomerSettingsGeneralLink();
            string expectedTimeZone = settingsPage.GetRandomTimeZone();
            settingsPage.SetCustomerGeneralFields(carrierResponseTime, schedulingLeadTime, submissionDeadline, notificationEmail, expectedTimeZone)
                .ClickSaveButton()
                .WaitForSaveConfirmation()
                .Refresh()
                .ClickCustomerSettingsGeneralLink()
                .SearchAndSelectCustomer(customerId);
            Assert.AreEqual(carrierResponseTime, settingsPage.CarrierResponseTimeField.GetAttribute("value"));
            Assert.AreEqual(schedulingLeadTime, settingsPage.SchedulingLeadTimeField.GetAttribute("value"));
            Assert.AreEqual(submissionDeadline, settingsPage.SubmissionDeadlineField.GetAttribute("value"));
            Assert.AreEqual(notificationEmail, settingsPage.NotificationEmailField.GetAttribute("value"));
            Assert.AreEqual(expectedTimeZone, settingsPage.GetTimeZone());
        }

        [Test, Retry(2), Category("Smoke")]
        public void CustomerSettingsFindCustomerByENumber()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow customerRow = dbAccess.GetRandomSyncedCustomerRow();
            string customerId = customerRow["CustomerId"].ToString();
            settingsPage.ClickCustomerSettingsGeneralLink()
                .SearchCustomer(customerId);
            Assert.IsTrue(settingsPage.DisplayedInResults(settingsPage.CustomerIdSearchResults, customerId),
                "Customer search results do not contain " + customerId);
        }

        [Test, Retry(2), Category("Smoke"), Ignore("Find customer by name is not yet functional.")]
        public void CustomerSettingsFindCustomerByName()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow customerRow = dbAccess.GetRandomSyncedCustomerRow();
            string customerName = customerRow["CustomerName"].ToString();
            settingsPage.ClickCustomerSettingsGeneralLink()
                .SearchCustomer(customerName);
            //settingsPageElements.SearchCustomer(Regex.Replace(customerName.Split()[0], @"[^0-9a-zA-Z\ ]+", ""));
            Assert.IsTrue(settingsPage.DisplayedInResults(settingsPage.CustomerNameSearchResults, customerName),
                "Customer search results do not contain " + customerName);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CustomerSettingsSetTenderHours()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow customerRow = dbAccess.GetRandomCustomerWithoutInheritSettings();
            string customerId = customerRow["CustomerId"].ToString();
            settingsPage.ClickTenderHoursLink()
                .SearchAndSelectCustomer(customerId)
                //.SetTenderHours(settingsPage.GetDayOfWeek(), "08:30", "17:30")
                .SetRandomTenderHours()
                .ClickSaveButton()
                .WaitForSaveConfirmation();
            Assert.That(settingsPage.SaveConfirmationDialog.Displayed, Is.True);
            TenderHoursTable tenderHoursTable = new TenderHoursTable();
            IList<TenderHoursRow> expectedRows = tenderHoursTable.BuildRows();
            settingsPage
                .Refresh()
                .ClickTenderHoursLink()
                .SearchAndSelectCustomer(customerId);
            foreach (TenderHoursRow expectedRow in expectedRows)
            {
                TenderHoursRow actualRow = tenderHoursTable.GetRowByDayOfWeek(expectedRow.DayOfWeek);
                Assert.AreEqual(expectedRow.StartTime, actualRow.StartTime);
                Assert.AreEqual(expectedRow.EndTime, actualRow.EndTime);
                Assert.AreEqual(expectedRow.IsOpen, actualRow.IsOpen);
            }
        }

        [Test, Retry(2), Category("Smoke")]
        public void CustomerSettingsSyncNewCustomer()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow customerRow = dbAccess.GetUnsyncedCustomerRow();
            string customerId = customerRow["CustomerId"].ToString();
            settingsPage.ClickCustomerSettingsGeneralLink()
                .SearchCustomer(customerId);
            Assert.AreEqual(settingsPage.CustomerIdSearchResults[0].Text, "No customers found");
            Pages.Optimizer.AccountsPage optimizerAccountsPageElements = new Pages.Optimizer.AccountsPage();
            optimizerAccountsPageElements.SyncCustomerWithTlrg(customerId);
            settingsPage = new SettingsPage();
            customerRow = dbAccess.GetSyncedCustomerRow(customerId);
            string isTlrgEnabled = customerRow["IsTLRGEnabled"].ToString();
            Assert.IsTrue(Convert.ToBoolean(isTlrgEnabled));
            settingsPage.Refresh()
                .ClickCustomerSettingsGeneralLink()
                .SearchCustomer(customerId);
            Assert.AreEqual(settingsPage.CustomerIdSearchResults[0].Text, customerId);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CustomerSettingsCogsSellSideActive()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow customerRow = dbAccess.GetUnsyncedCustomerRow();
            string customerId = customerRow["CustomerId"].ToString();
            settingsPage.ClickCustomerSettingsGeneralLink()
                .SearchCustomer(customerId);
            Assert.AreEqual(settingsPage.CustomerIdSearchResults[0].Text, "No customers found");
            Pages.Optimizer.AccountsPage optimizerAccountsPageElements = new Pages.Optimizer.AccountsPage();
            optimizerAccountsPageElements.SyncCustomerWithTlrg(customerId);
            settingsPage = new SettingsPage()
                .Refresh()
                .ClickCustomerSettingsGeneralLink()
                .SearchAndSelectCustomer(customerId);
            Assert.That(settingsPage.CogsActiveSellSideCheck.Displayed, Is.True);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsPrimaryEmail()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            string primaryEmailActual = "";
            string primaryEmailExpected = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId)
                .SetFieldValue(settingsPage.PrimaryDispatchEmailField, primaryEmailExpected)
                .ClickSaveButton()
                .Refresh()
                .ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId);
            primaryEmailActual = settingsPage.GetFieldValue(settingsPage.PrimaryDispatchEmailField);
            Assert.AreEqual(primaryEmailActual, primaryEmailExpected);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsSecondaryEmail()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierRowWithPrimaryEmail();
            string carrierId = carrierRow["CarrierId"].ToString();
            string secondaryEmailActual = "";
            string secondaryEmailExpected = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId)
                .SetFieldValue(settingsPage.SecondaryDispatchEmailField, secondaryEmailExpected)
                .ClickSaveButton()
                .Refresh()
                .ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId);
            secondaryEmailActual = settingsPage.GetFieldValue(settingsPage.SecondaryDispatchEmailField);
            Assert.AreEqual(secondaryEmailActual, secondaryEmailExpected);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsTertiaryEmail()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierRowWithPrimaryEmail();
            string carrierId = carrierRow["CarrierId"].ToString();
            string tertiaryEmailActual = "";
            string tertiaryEmailExpected = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId)
                .SetFieldValue(settingsPage.TertiaryDispatchEmailField, tertiaryEmailExpected)
                .ClickSaveButton()
                .Refresh()
                .ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId);
            tertiaryEmailActual = settingsPage.GetFieldValue(settingsPage.TertiaryDispatchEmailField);
            Assert.AreEqual(tertiaryEmailActual, tertiaryEmailExpected);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsScacDisplayed()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            string carrierScac = carrierRow["SCAC"].ToString();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId);
            Assert.AreEqual(settingsPage.CarrierScac.GetAttribute("value"), carrierScac);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsTlrgActive()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId);
            Assert.That(settingsPage.CarrierTlrgActive.Displayed, Is.True);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsSaveConfirmation()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            string primaryEmailExpected = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId)
                .SetFieldValue(settingsPage.PrimaryDispatchEmailField, primaryEmailExpected)
                .ClickSaveButton()
                .WaitForSaveConfirmation();
            Assert.That(settingsPage.SaveConfirmationDialog.Displayed, Is.True);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsInvalidEmail()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId)
                .SetFieldValue(settingsPage.PrimaryDispatchEmailField, "qa@echo.");
            Assert.That(settingsPage.InvalidEmailMessage.Displayed, Is.True);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsSaveDisabled()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId)
                .SetFieldValue(settingsPage.PrimaryDispatchEmailField, "qa@echo.");
            Assert.That(settingsPage.SaveButton.Enabled, Is.False);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsLastUpdated()
        {
            string actualUpdate = "";
            string expectedUpdate = "";
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId)
                .SetFieldValue(settingsPage.PrimaryDispatchEmailField, "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com")
                .ClickSaveButton();

            expectedUpdate = DateTime.Now.ToString("MM-dd-yy HH:mm");
            settingsPage.Refresh()
                .ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId);
            actualUpdate = Regex.Match(settingsPage.CarrierLastUpdated.Text, @"\d{2}\-\d{2}\-\d{2} \d{2}\:\d{2}").Value;
            Assert.That((DateTime.Parse(expectedUpdate) - DateTime.Parse(actualUpdate)).TotalSeconds, Is.LessThanOrEqualTo(5));
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsCarrierContacts()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataTable carrierTable = dbAccess.GetRandomCarrierContactRows();
            string carrierId = carrierTable.Rows[0]["CarrierId"].ToString();
            String firstName = carrierTable.Rows[0]["FirstName"].ToString();
            String lastName = carrierTable.Rows[0]["LastName"].ToString();
            String phone = carrierTable.Rows[0]["Phone"].ToString();
            String email = carrierTable.Rows[0]["Email"].ToString();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId);
            
            Assert.That(settingsPage.DisplayedInCarrierContacts(firstName + lastName + phone + email), Is.True);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsSyncNewCarrier()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetUnsyncedCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPage.SyncNewCarrier(carrierId)
                .Refresh()
                .ClickCarrierSettingsGeneralLink()
                .SearchCarrier(carrierId);
            Assert.That(settingsPage.CarrierDisplayed(carrierId), Is.True);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsUpdateSyncedCarrier()
        {
            Boolean ediEnabledBefore;
            Boolean ediEnabledAfter;
            string user = env.OptiUser;
            string pass = env.OptiPassword;
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomCarrierWithScacRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId);
            ediEnabledBefore = settingsPage.EdiEnabled();
            Pages.Optimizer.CarrierPage optimizerCarrierPage = new Pages.Optimizer.CarrierPage();
            optimizerCarrierPage.SearchAndSelectCarrier(carrierId)
                .AccessFolder("Company Information")
                .AccessFolderDraggable("EDI")
                .ClickCarrierEdiEditButton()
                .ClickEdi204TLAutoTenderCheckbox()
                .ClickCarrierEdiSaveButton();
            settingsPage = new SettingsPage();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .ClickSyncCarrierToggle()
                .SetFieldValue(settingsPage.CarrierSyncField, carrierId)
                .ClickUpdateSyncedCarriersButton()
                .Refresh()
                .ClickCarrierSettingsGeneralLink()
                .SearchAndSelectCarrier(carrierId);
            ediEnabledAfter = settingsPage.EdiEnabled();
            Assert.AreNotEqual(ediEnabledBefore, ediEnabledAfter);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CarrierSettingsFindCarrierByLNumber()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomSyncedCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchCarrier(carrierId);
            Assert.IsTrue(settingsPage.DisplayedInResults(settingsPage.CarrierIdSearchResults, carrierId), "Carrier search results do not contain " + carrierId);
        }

        [Test, Retry(2), Category("Smoke"), Ignore("Find carrier by name is not yet functional.")]
        public void CarrierSettingsFindCarrierByName()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow carrierRow = dbAccess.GetRandomSyncedCarrierRow();
            string carrierName = carrierRow["CarrierName"].ToString();
            settingsPage.ClickCarrierSettingsGeneralLink()
                .SearchCarrier(carrierName);
            Assert.IsTrue(settingsPage.DisplayedInResults(settingsPage.CarrierNameSearchResults, carrierName), "Carrier search results do not contain " + carrierName);
        }

        [Test, Retry(2), Category("Smoke")]
        public void CustomerIncorporateHolidayScheduleTest()
        {
            SettingsPage settingsPage = new SettingsPage();
            DataRow customerRow = dbAccess.GetRandomCustomerWithoutInheritSettings();
            string customerId = customerRow["CustomerId"].ToString();
            string holidayName = "Holiday 1";
            DateTime today = DateTime.Now;
            string formattedTodaysDate = string.Format("{0:M/d/yyyy}", today);

            settingsPage
                .ClickHolidayScheduleLink()
                .SearchAndSelectCustomer(customerId)
                .SetFieldValue(settingsPage.HolidayDescriptionField, "X");
            settingsPage.SetFieldValue(settingsPage.HolidayDescriptionField, holidayName);
            settingsPage.HolidayDateFieldSet.SendKeys(settingsPage.GetTodaysDate());
            settingsPage.ClickButton(settingsPage.AddHolidayButton)
                .ClickSaveButton();
            HolidayScheduleTable scheduleTable = new HolidayScheduleTable();
            Assert.That(scheduleTable.HolidayExistsInTable(holidayName));
            Assert.That(scheduleTable.GetRowByLabel(holidayName).date.ToShortDateString() == formattedTodaysDate);
            settingsPage
                .Refresh()
                .ClickButton(settingsPage.CustomerSettingsHolidayScheduleLink)
                .SearchAndSelectCustomer(customerId);
            Assert.That(scheduleTable.HolidayExistsInTable(holidayName));
            scheduleTable
                .GetRowByLabel(holidayName)
                .removeButton.Click();
            settingsPage.ClickSaveButton()
                .Refresh()
                .ClickHolidayScheduleLink()
                .SearchAndSelectCustomer(customerId);
            Assert.False(scheduleTable.HolidayExistsInTable(holidayName));

        }
    }
}
