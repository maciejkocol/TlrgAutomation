using Tlrg.Pages.Tlrg;
using NUnit.Framework;
using System;
using System.Text.RegularExpressions;
using System.Data;

namespace Tlrg.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class SettingsTest: BaseTest
    {

        [Test, Retry(2), TestCategory("Smoke")]
        public void CustomerSettingsSyncNewCustomer()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow customerRow = settingsPageElements.GetUnsyncedCustomerRow();
            string customerId = customerRow["CustomerId"].ToString();
            settingsPageElements.ClickCustomerSettingsGeneralLink();
            settingsPageElements.SearchCustomer(customerId);
            Assert.AreEqual(settingsPageElements.CustomerSearchResults[0].Text, "No customers found");
            Pages.Optimizer.AccountsPage optimizerAccountsPageElements = new Pages.Optimizer.AccountsPage(driver);
            optimizerAccountsPageElements.SyncCustomerWithTlrg(customerId);
            settingsPageElements = new SettingsPage(driver);
            customerRow = settingsPageElements.GetSyncedCustomerRow(customerId);
            Assert.IsTrue(Convert.ToBoolean(customerRow["IsTLRGEnabled"].ToString()));
            settingsPageElements.Refresh();
            settingsPageElements.ClickCustomerSettingsGeneralLink();
            settingsPageElements.SearchCustomer(customerId);
            Assert.AreEqual(settingsPageElements.CustomerSearchResults[0].Text, customerId);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsPrimaryEmail()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            string primaryEmailActual = "";
            string primaryEmailExpected = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            settingsPageElements.SetFieldValue(settingsPageElements.PrimaryDispatchEmailField, primaryEmailExpected);
            settingsPageElements.ClickSaveButton();
            settingsPageElements.Refresh();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            primaryEmailActual = settingsPageElements.GetFieldValue(settingsPageElements.PrimaryDispatchEmailField);
            Assert.AreEqual(primaryEmailActual, primaryEmailExpected);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsSecondaryEmail()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierRowWithPrimaryEmail();
            string carrierId = carrierRow["CarrierId"].ToString();
            string secondaryEmailActual = "";
            string secondaryEmailExpected = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            settingsPageElements.SetFieldValue(settingsPageElements.SecondaryDispatchEmailField, secondaryEmailExpected);
            settingsPageElements.ClickSaveButton();
            settingsPageElements.Refresh();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            secondaryEmailActual = settingsPageElements.GetFieldValue(settingsPageElements.SecondaryDispatchEmailField);
            Assert.AreEqual(secondaryEmailActual, secondaryEmailExpected);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsTertiaryEmail()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierRowWithPrimaryEmail();
            string carrierId = carrierRow["CarrierId"].ToString();
            string tertiaryEmailActual = "";
            string tertiaryEmailExpected = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            settingsPageElements.SetFieldValue(settingsPageElements.TertiaryDispatchEmailField, tertiaryEmailExpected);
            settingsPageElements.ClickSaveButton();
            settingsPageElements.Refresh();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            tertiaryEmailActual = settingsPageElements.GetFieldValue(settingsPageElements.TertiaryDispatchEmailField);
            Assert.AreEqual(tertiaryEmailActual, tertiaryEmailExpected);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsScacDisplayed()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            string carrierScac = carrierRow["SCAC"].ToString();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            Assert.AreEqual(settingsPageElements.CarrierScac.GetAttribute("value"), carrierScac);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsTlrgActive()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            Assert.That(settingsPageElements.CarrierTlrgActive.Displayed, Is.True);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsSaveConfirmation()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            string primaryEmailExpected = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            settingsPageElements.SetFieldValue(settingsPageElements.PrimaryDispatchEmailField, primaryEmailExpected);
            settingsPageElements.ClickSaveButton();
            Assert.That(settingsPageElements.SaveConfirmationDialog.Displayed, Is.True);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsInvalidEmail()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            settingsPageElements.SetFieldValue(settingsPageElements.PrimaryDispatchEmailField, "qa@echo.");
            Assert.That(settingsPageElements.InvalidEmailMessage.Displayed, Is.True);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsSaveDisabled()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            settingsPageElements.SetFieldValue(settingsPageElements.PrimaryDispatchEmailField, "qa@echo.");
            Assert.That(settingsPageElements.SaveButton.Enabled, Is.False);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsLastUpdated()
        {
            string actualUpdate = "";
            string expectedUpdate = "";
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            settingsPageElements.SetFieldValue(settingsPageElements.PrimaryDispatchEmailField, "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com");
            settingsPageElements.ClickSaveButton();
            expectedUpdate = DateTime.Now.ToString("MM-dd-yy HH:mm");
            settingsPageElements.Refresh();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            actualUpdate = Regex.Match(settingsPageElements.CarrierLastUpdated.Text, @"\d{2}\-\d{2}\-\d{2} \d{2}\:\d{2}").Value;
            Assert.That((DateTime.Parse(expectedUpdate) - DateTime.Parse(actualUpdate)).TotalSeconds, Is.LessThanOrEqualTo(5));
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsCarrierContacts()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataTable carrierTable = settingsPageElements.GetRandomCarrierContactRows();
            string carrierId = carrierTable.Rows[0]["CarrierId"].ToString();
            string firstName = "";
            string lastName = "";
            string phone = "";
            string email = "";
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            firstName = carrierTable.Rows[0]["FirstName"].ToString();
            lastName = carrierTable.Rows[0]["LastName"].ToString();
            phone = carrierTable.Rows[0]["Phone"].ToString();
            email = carrierTable.Rows[0]["Email"].ToString();
            Assert.That(settingsPageElements.DisplayedInCarrierContacts(firstName), Is.True);
            Assert.That(settingsPageElements.DisplayedInCarrierContacts(lastName), Is.True);
            Assert.That(settingsPageElements.DisplayedInCarrierContacts(phone), Is.True);
            Assert.That(settingsPageElements.DisplayedInCarrierContacts(email), Is.True);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsSyncNewCarrier()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetUnsyncedCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.ClickSyncCarrierToggle();
            settingsPageElements.SetFieldValue(settingsPageElements.CarrierSyncField, carrierId);
            settingsPageElements.ClickSyncCarriersButton();
            settingsPageElements.Refresh();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchCarrier(carrierId);
            Assert.That(settingsPageElements.CarrierDisplayed(carrierId), Is.True);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsUpdateSyncedCarrier()
        {
            Boolean ediEnabledBefore;
            Boolean ediEnabledAfter;
            string user = env.OptiUser;
            string pass = env.OptiPassword;
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetRandomCarrierWithScacRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            ediEnabledBefore = settingsPageElements.EdiEnabled();
            Pages.Optimizer.CarrierPage optimizerCarrierPageElements = new Pages.Optimizer.CarrierPage(driver);
            optimizerCarrierPageElements.SearchAndSelectCarrier(carrierId);
            optimizerCarrierPageElements.AccessFolder("Company Information");
            optimizerCarrierPageElements.AccessFolderDraggable("EDI");
            optimizerCarrierPageElements.ClickCarrierEdiEditButton();
            optimizerCarrierPageElements.ClickEdi204TLAutoTenderCheckbox();
            optimizerCarrierPageElements.ClickCarrierEdiSaveButton();
            settingsPageElements = new SettingsPage(driver);
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.ClickSyncCarrierToggle();
            settingsPageElements.SetFieldValue(settingsPageElements.CarrierSyncField, carrierId);
            settingsPageElements.ClickUpdateSyncedCarriersButton();
            settingsPageElements.Refresh();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchAndSelectCarrier(carrierId);
            ediEnabledAfter = settingsPageElements.EdiEnabled();
            Assert.AreNotEqual(ediEnabledBefore, ediEnabledAfter);
        }

        [Test, Retry(2), TestCategory("Smoke")]
        public void CarrierSettingsUpdateUnsyncedCarrier()
        {
            SettingsPage settingsPageElements = new SettingsPage(driver);
            DataRow carrierRow = settingsPageElements.GetUnsyncedCarrierRow();
            string carrierId = carrierRow["CarrierId"].ToString();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.ClickSyncCarrierToggle();
            settingsPageElements.SetFieldValue(settingsPageElements.CarrierSyncField, carrierId);
            settingsPageElements.ClickUpdateSyncedCarriersButton();
            settingsPageElements.Refresh();
            settingsPageElements.ClickCarrierSettingsGeneralLink();
            settingsPageElements.SearchCarrier(carrierId);
            Assert.That(settingsPageElements.CarrierDisplayed(carrierId), Is.True);
        }
    }
}
        