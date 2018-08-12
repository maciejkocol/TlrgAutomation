using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tlrg.Pages.Tlrg
{
    public class SettingsPage : BasePage
    {
        public SettingsPage(IWebDriver driver) : base(driver)
        {
            driver.Navigate().GoToUrl(env.TlrgURL + "/settings");
            WaitForSettingsPageToLoad();
        }

        // indicators
        public string ediEnabledIndicator = "yes";

        // links
        public IWebElement SettingsNavItem => driver.FindElementBy(By.XPath("//li[starts-with(@class,'nav-item') and contains(.,'Settings')]"));
        public IWebElement CarrierSettingsGeneralLink => driver.FindElementBy(By.XPath("//div[@class='setting-menu carrier'][span[text()='Carrier Settings']]//li[text()='General']"));
        public IWebElement CustomerSettingsGeneralLink => driver.FindElementBy(By.XPath("//div[@class='setting-menu'][span[text()='Customer Settings']]//li[text()='General']"));

        // labels
        public IWebElement CarrierEdiEnabled => driver.FindElementBy(By.XPath("//label[contains(@class,'edi-enabled')]"));
        public IWebElement CarrierTlrgActive => driver.FindElementBy(By.Name("isTLRGEnabled"));
        public IWebElement CarrierScac => driver.FindElementBy(By.Name("carrierSCAC"));
        public IList<IWebElement> CarrierContacts => driver.FindElementsBy(By.XPath("//div[(contains(@class,'carrier-settings-carrier-contact'))] | //label[contains(@class,'carrier-contact-label ')]"));
        public IWebElement CarrierLastUpdated => driver.FindElementBy(By.XPath("//div[contains(@class,'last-updated')]"));

        // fields
        public IWebElement PrimaryDispatchEmailField => driver.FindElementBy(By.Name("primaryDispatchEmail"));
        public IWebElement SecondaryDispatchEmailField => driver.FindElementBy(By.Name("secondaryDispatchEmail"));
        public IWebElement TertiaryDispatchEmailField => driver.FindElementBy(By.Name("tertiaryDispatchEmail"));
        public IWebElement TimeZoneField => driver.FindElementBy(By.Name("TimeZone"));
        public IWebElement CarrierSearchField => driver.FindElementBy(By.Id("txtCarrierSearch"));
        public IWebElement CustomerSearchField => driver.FindElementBy(By.Id("txtCustomerSearch"));
        public IWebElement CarrierSyncField => driver.FindElementBy(By.Id("txtCarrierSyncId"));

        // buttons
        public IWebElement SaveButton => driver.FindElementBy(By.XPath("//button[@type='button' and text()='Save']"));
        public IWebElement UpdateSyncedCarriersButton => driver.FindElementBy(By.XPath("//button[@type='button' and text()='Update Synced Carriers']"));
        public IWebElement SyncCarriersButton => driver.FindElementBy(By.XPath("//button[@type='button' and text()='Sync Carriers']"));
        public IWebElement SyncCarrierToggle => driver.FindElementBy(By.XPath("//span[text()='Sync Carrier Ids']"));

        // messages
        public IWebElement SaveConfirmationDialog => driver.FindElementBy(By.XPath("//div[text()='Update Request Sent']"));
        public IWebElement InvalidEmailMessage => driver.FindElementBy(By.XPath("//div[not(contains(@class,'hide-error')) and text()='Primary Dispatch Email is not valid.']"));

        // search results
        public IList<IWebElement> CarrierSearchResults => driver.FindElementsBy(By.XPath("//div[@class='search-results']/div[@class='search-number']"));
        public IList<IWebElement> CustomerSearchResults => driver.FindElementsBy(By.XPath("//div[@class='search-results-container']//div[@class='search-number'] | //div[@class='search-results-container']//div[@class='search-results' and contains(text(),'No customers found')]"));

        // queries
        public string RandomCarrierQuery => "SELECT TOP 1 * " +
                "FROM XPlatform.Carrier.Carrier " +
                "ORDER BY NEWID()";
        public string UnsyncedCarrierQuery => "SELECT TOP 1 t1.* " +
                "FROM EchoOptimizer.dbo.tblCarrier t1 " +
                "LEFT JOIN XPlatform.Carrier.Carrier t2 ON t2.CarrierId = t1.CarrierId " +
                "WHERE t2.CarrierId IS NULL";
        public string UnsyncedCustomerQuery => "SELECT TOP 1 t1.CustomerId, t2.IsTLRGEnabled " +
                "FROM EchoOptimizer.dbo.tblCustomer t1 " +
                "LEFT JOIN XPlatform.Customer.Customer t2 ON t2.CustomerId = t1.CustomerId " +
                "WHERE t2.IsTLRGEnabled IS NULL " +
                "ORDER BY NEWID()";
        public string SyncedCustomerQuery => "SELECT TOP 1 t1.CustomerId, t2.IsTLRGEnabled " +
                "FROM EchoOptimizer.dbo.tblCustomer t1 " +
                "LEFT JOIN XPlatform.Customer.Customer t2 ON t2.CustomerId = t1.CustomerId " +
                "WHERE t1.CustomerId = '{0}'";
        public string RandomCarrierWithScacQuery => "SELECT TOP 1 * " +
                "FROM XPlatform.Carrier.Carrier " +
                "WHERE NOT ISNULL(SCAC, '') = '' " +
                "ORDER BY NEWID()";
        public string RandomCarrierContactQuery => "SELECT TOP 1 t1.CarrierId, t2.* " +
                "FROM XPlatform.Carrier.Carrier t1 " +
                "LEFT JOIN XPlatform.Carrier.Contact t2 " +
                "ON t2.AggregateCarrierId = t1.Id " +
                "WHERE ISNULL(t2.title, '') = '' " +
                "ORDER BY NEWID()";
        public string RandomCarrierWithPrimaryEmailQuery => "SELECT TOP 1 t1.* " +
                "FROM XPlatform.Carrier.Carrier t1 " +
                "LEFT JOIN XPlatform.Carrier.Contact t2 " +
                "ON t2.AggregateCarrierId = t1.Id " +
                "WHERE t2.Title = 'PRIMARY' " +
                "ORDER BY NEWID()";

        /**
         * Method to wait for the navigation objects to load on the Settings page.
         */
        public void WaitForSettingsPageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SettingsNavItem.Displayed);
        }

        /**
         * Method to click the Save button and wait.
         */
        public void ClickSaveButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SaveButton.Displayed);
            SaveButton.Click();
            WaitForSaveConfirmation();
        }

        /**
         * Method to wait for save confirmation.
         */
        public void WaitForSaveConfirmation()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SaveConfirmationDialog.Displayed);
        }

        /**
         * Method to click the Update Synced Carriers button and wait.
         */
        public void ClickUpdateSyncedCarriersButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => UpdateSyncedCarriersButton.Displayed);
            UpdateSyncedCarriersButton.Click();
            Thread.Sleep(SleepTime);
        }

        /**
         * Method to click the Sync Carriers button and wait.
         */
        public void ClickSyncCarriersButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SyncCarriersButton.Displayed);
            SyncCarriersButton.Click();
            Thread.Sleep(SleepTime);
        }

        /**
         * Method to click the sync toggle under Carrier Settings. 
         */
        public void ClickSyncCarrierToggle()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SyncCarrierToggle.Displayed);
            SyncCarrierToggle.Click();
        }

        /**
         * Method to click the General link under Carrier Settings. 
         */
        public void ClickCarrierSettingsGeneralLink()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierSettingsGeneralLink.Displayed);
            CarrierSettingsGeneralLink.Click();
        }

        /**
         * Method to click the General link under Customer Settings. 
         */
        public void ClickCustomerSettingsGeneralLink()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CustomerSettingsGeneralLink.Displayed);
            CustomerSettingsGeneralLink.Click();
        }

        /**
         * Method to search for a Carrier by Id. Switches to carrier search field and enters carrier id.
         */
        public void SearchCarrier(string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierSearchField.Displayed);
            CarrierSearchField.Clear();
            CarrierSearchField.SendKeys(carrierId);
        }

        /**
         * Method to select a carrier by id from search results.
         */
        public void SelectCarrier(string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierSearchResults.Count > 0);
            foreach (IWebElement searchResult in CarrierSearchResults)
            {
                if (searchResult.Text == carrierId)
                {
                    searchResult.Click();
                    break;
                }
            }
        }

        /**
         * Method to search for and select a carrier from search results.
         */
        public void SearchAndSelectCarrier(string carrierId)
        {
            SearchCarrier(carrierId);
            SelectCarrier(carrierId);
        }

        /**
         * Method to search for a Customer by Id. Switches to customer search field and enters customer id.
         */
        public void SearchCustomer(string customerId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CustomerSearchField.Displayed);
            CustomerSearchField.Clear();
            CustomerSearchField.SendKeys(customerId);
        }

        /**
         * Method to select a customer by id from search results.
         */
        public void SelectCustomer(string customerId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CustomerSearchResults.Count > 0);
            foreach (IWebElement searchResult in CustomerSearchResults)
            {
                if (searchResult.Text == customerId)
                {
                    searchResult.Click();
                    break;
                }
            }
        }

        /**
         * Method to search for and select a customer from search results.
         */
        public void SearchAndSelectCustomer(string customerId)
        {
            SearchCustomer(customerId);
            SelectCustomer(customerId);
        }

        /**
         * Method to check if a Carrier appears in the search results.
         */
        public Boolean CarrierDisplayed(string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierSearchResults.Count > 0);
            foreach (IWebElement searchResult in CarrierSearchResults)
            {
                if (searchResult.Text == carrierId)
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * Method to check if a given text appears in the contacts.
         */
        public Boolean DisplayedInCarrierContacts(string text)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierContacts.Count > 0);
            foreach (IWebElement contact in CarrierContacts)
            {
                string contactText = Regex.Replace(contact.Text, @"[ /(/)\t\s/-]+", "");
                if (contactText.ToLower().Contains(text.ToLower()))
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * Method to check if carrier is EDI enabled.
         */
        public Boolean EdiEnabled()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierEdiEnabled.Displayed);
            return CarrierEdiEnabled.Text.ToLower().Contains(ediEnabledIndicator);
        }

        /**
         * Method to get a random Carrier record.
         */
        public DataRow GetRandomCarrierRow()
        {
            return GetData(RandomCarrierQuery).Rows[0];
        }

        /**
         * Method to get a random Carrier record that has not been synced with TLRG.
         */
        public DataRow GetUnsyncedCarrierRow()
        {
            return GetData(UnsyncedCarrierQuery).Rows[0];
        }

        /**
         * Method to get a random Customer record that has not been synced with TLRG.
         */
        public DataRow GetUnsyncedCustomerRow()
        {
            return GetData(UnsyncedCustomerQuery).Rows[0];
        }

        /**
         * Method to get a Customer record that has been synced with TLRG.
         */
        public DataRow GetSyncedCustomerRow(String customerId)
        {
            string query = string.Format(SyncedCustomerQuery, customerId);
            return GetData(query).Rows[0];
        }

        /**
         * Method to get a random Carrier record with SCAC.
         */
        public DataRow GetRandomCarrierWithScacRow()
        {
            return GetData(RandomCarrierWithScacQuery).Rows[0];

        }

        /**
         * Method to get contacts for a random Carrier.
         */
        public DataTable GetRandomCarrierContactRows()
        {
            return GetData(RandomCarrierContactQuery);
        }

        /**
         * Method to get a random Carrier record with Primary Email.
         */
        public DataRow GetRandomCarrierRowWithPrimaryEmail()
        {
            return GetData(RandomCarrierWithPrimaryEmailQuery).Rows[0];
        }

        /**
         * Method to refresh the current page.
         */
        public void Refresh()
        {
            Thread.Sleep(SleepTime * 2);
            driver.Navigate().Refresh();
            Thread.Sleep(SleepTime * 2);
        }
    }
}
