using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TlrgAutomation.Pages.Tlrg
{
    public class SettingsPage : BasePage
    {
        public SettingsPage() : base()
        {
            driver.Navigate().GoToUrl(env.TlrgURL + "/settings");
            WaitForSettingsPageToLoad();
        }

        // indicators
        public string ediEnabledIndicator = "yes";

        // links
        public IWebElement SettingsNavItem => driver.FindElementBy(By.Id("settings-nav-link"));
        public IWebElement CarrierSettingsGeneralLink => driver.FindElementBy(By.Id("carrier-general-settings-link"));
        public IWebElement CarrierSettingsOperationHoursLink => driver.FindElementBy(By.Id("carrier-operationhours-settings-link"));
        public IWebElement CustomerSettingsGeneralLink => driver.FindElementBy(By.Id("customer-general-settings-link"));
        public IWebElement CustomerSettingsTenderHoursLink => driver.FindElementBy(By.Id("customer-tenderhours-settings-link"));
        public IWebElement CustomerSettingsHolidayScheduleLink => driver.FindElementBy(By.Id("customer-holidayschedule-settings-link"));
        public IWebElement CustomerSettingsAutomationLink => driver.FindElementBy(By.Id("customer-automation-settings-link"));
        public IWebElement CustomerSettingsAuctionSettingsLink => driver.FindElementBy(By.Id("customer-auction-settings-link"));
        public IWebElement CustomerSettingsAuctionContactsLink => driver.FindElementBy(By.Id("customer-auctionContacts-settings-link"));

        // labels
        public IWebElement CarrierEdiEnabled => driver.FindElementBy(By.XPath("//label[contains(@class,'edi-enabled')]"));
        public IWebElement CarrierTlrgActive => driver.FindElementBy(By.Name("isTLRGEnabled"));
        public IWebElement CarrierScac => driver.FindElementBy(By.Name("carrierSCAC"));
        public IList<IWebElement> CarrierContacts => driver.FindElements(By.XPath("//div[starts-with(@class,'carrier-settings-carrier-contact ')]"));
        public IWebElement CarrierLastUpdated => driver.FindElementBy(By.XPath("//div[contains(@class,'last-updated')]"));

        // toggles
        public IWebElement SettingsMenuCollapse => driver.FindElementBy(By.Id("menu-collapse"));

        // fields
        public IWebElement PrimaryDispatchEmailField => driver.FindElementBy(By.Name("primaryDispatchEmail"));
        public IWebElement SecondaryDispatchEmailField => driver.FindElementBy(By.Name("secondaryDispatchEmail"));
        public IWebElement TertiaryDispatchEmailField => driver.FindElementBy(By.Name("tertiaryDispatchEmail"));
        public IWebElement TimeZoneField => driver.FindElementBy(By.Name("TimeZone"));
        public IWebElement CarrierSyncField => driver.FindElementBy(By.Id("txtCarrierSyncId"));
        public IWebElement HolidayDescriptionField => driver.FindElementBy(By.Id("txtDescription"));
        public IWebElement HolidayDateField => driver.FindElementBy(By.XPath("//div[contains(@class, 'datetimeinput')]"));
        public IWebElement HolidayDateFieldSet => driver.FindElementBy(By.XPath("//input[contains(@id, 'inputjqxWidget')]"));
        public IWebElement AuctionResponseTimeField => driver.FindElementBy(By.Id("customer-carrier-auction-response-time"));
        public IWebElement AuctionTenderResponseTimeField => driver.FindElementBy(By.Id("customer-carrier-auction-tender-response-time"));
        public IWebElement AuctionBidThresholdRateLimitField => driver.FindElementBy(By.Id("customer-auction-bid-threshold-contract-rate"));
        public IWebElement AuctionBidThresholdPerMileField => driver.FindElementBy(By.Id("customer-auction-bid-threshold-per-mile"));
        public IWebElement CarrierResponseTimeField => driver.FindElementBy(By.Id("txtCustomerCarrierResponseTime"));
        public IWebElement SchedulingLeadTimeField => driver.FindElementBy(By.Id("txtCustomerSchedulingLeadTime"));
        public IWebElement SubmissionDeadlineField => driver.FindElementBy(By.Id("txtCustomerSubmissionDeadline"));
        public IWebElement NotificationEmailField => driver.FindElementBy(By.Id("txtNotificationEmail"));
        public IList<IWebElement> TenderHourGroups => driver.FindElements(By.XPath("//div[starts-with(@class,'tender-hour-group')]"));

        // checkboxes
        By InheritSettingsCheck = By.XPath("//div[@class='inherit-settings-check']//i[starts-with(@class,'fa fa-check') and not(contains(@class,'hidden'))]");
        public IWebElement CogsActiveSellSideCheck => driver.FindElement(By.XPath("//i[@class='fa fa-check fa-stack-2x text-success']"));
        
        // radio buttons
        public IWebElement AuctionOffRadioButton => driver.FindElementBy(By.Id("auction-off"));
        public IWebElement AuctionOnlyRadioButton => driver.FindElementBy(By.Id("auction-only"));
        public IWebElement AuctionUponFallThroughRadioButton => driver.FindElementBy(By.Id("auction-upon-fallthrough"));

        // buttons
        public IWebElement SaveButton => driver.FindElementBy(By.XPath("//button[@type='button' and text()='Save']"));
        public IWebElement AddHolidayButton => driver.FindElementBy(By.XPath("//button[@type='button' and text()='Add Holiday']"));
        public IWebElement UpdateSyncedCarriersButton => driver.FindElementBy(By.XPath("//button[@type='button' and text()='Update Synced Carriers']"));
        public IWebElement SyncCarriersButton => driver.FindElementBy(By.XPath("//button[@type='button' and text()='Sync Carriers']"));
        public IWebElement SyncCarrierToggle => driver.FindElementBy(By.XPath("//span[text()='Sync Carrier Ids']"));

        // messages
        public IWebElement SaveConfirmationDialog => driver.FindElementBy(By.XPath("//div[text()='Update Request Sent']"));
        public IWebElement InvalidEmailMessage => driver.FindElementBy(By.XPath("//div[not(contains(@class,'hide-error')) and text()='Primary Dispatch Email is not valid.']"));
        
        /**
         * Method to wait for the navigation objects to load on the Settings page.
         */
        public SettingsPage WaitForSettingsPageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SettingsNavItem.Displayed);
            return this;
        }

        /**
         * Method to click the Save button and wait.
         */
        public SettingsPage ClickSaveButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SaveButton.Displayed);
            SaveButton.Click();
            return this;
            //WaitForSaveConfirmation();
        }

        public new SettingsPage SetFieldValue(IWebElement locator, String value)
        {
            return (SettingsPage)(base.SetFieldValue(locator, value));
        }

        /**
         * Method to wait for save confirmation.
         */
        public SettingsPage WaitForSaveConfirmation()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SaveConfirmationDialog.Displayed);
            return this;
        }

        public SettingsPage ClickButton(IWebElement button)
        {
            button.Click();
            return this;
        }

        /**
         * Method to sync a new carrier.
         */
        public SettingsPage SyncNewCarrier(string carrierId)
        {
            ClickCarrierSettingsGeneralLink();
            ClickSyncCarrierToggle();
            SetFieldValue(CarrierSyncField, carrierId);
            ClickSyncCarriersButton();
            return this;
        }

        /**
         * Method to set customer tender hours for a given day.
         */
        public SettingsPage SetTenderHours(string dayOfWeek, string startTime, string endTime)
        {
            TenderHoursTable tenderHoursTable = new TenderHoursTable()
                .SetTenderHours(dayOfWeek, startTime, endTime);
            return this;
        }

        /**
         * Method to set random customer tender hours for a week. Always sets current day to open.
         */
        public SettingsPage SetRandomTenderHours()
        {
            Random rnd = new Random();
            string startTime = rnd.Next(4, 10).ToString().PadLeft(2, '0') + ":" + rnd.Next(1, 59).ToString().PadLeft(2, '0');
            string endTime = rnd.Next(13, 23).ToString() + ":" + rnd.Next(1, 59).ToString().PadLeft(2, '0');
            string today = GetDayOfWeek();
            TenderHoursTable tenderHoursTable = new TenderHoursTable();
            foreach (string dayOfWeek in GetDaysOfWeek())
            {
                bool isOpen = (rnd.Next(100) < 50);
                if (today.Equals(dayOfWeek) || isOpen)
                {
                    tenderHoursTable.SetTenderHours(dayOfWeek, startTime, endTime);
                }
                else
                {
                    tenderHoursTable.SetClosed(dayOfWeek);
                }
            }
            return this;
        }

        /**
         * Method to click the Update Synced Carriers button and wait.
         */
        public SettingsPage ClickUpdateSyncedCarriersButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => UpdateSyncedCarriersButton.Displayed);
            UpdateSyncedCarriersButton.Click();
            Thread.Sleep(SleepTime);
            return this;
        }

        /**
         * Method to click the Sync Carriers button and wait.
         */
        public SettingsPage ClickSyncCarriersButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SyncCarriersButton.Displayed);
            SyncCarriersButton.Click();
            Thread.Sleep(SleepTime);
            return this;
        }

        /**
         * Method to click the sync toggle under Carrier Settings. 
         */
        public SettingsPage ClickSyncCarrierToggle()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SyncCarrierToggle.Displayed);
            SyncCarrierToggle.Click();
            return this;
        }

        /**
         * Method to click the General link under Carrier Settings. 
         */
        public SettingsPage ClickCarrierSettingsGeneralLink()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierSettingsGeneralLink.Displayed);
            CarrierSettingsGeneralLink.Click();
            return this;
        }

        /**
         * Method to click the General link under Customer Settings. 
         */
        public SettingsPage ClickCustomerSettingsGeneralLink()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CustomerSettingsGeneralLink.Displayed);
            CustomerSettingsGeneralLink.Click();
            return this;
        }

        public SettingsPage ClickHolidayScheduleLink()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CustomerSettingsHolidayScheduleLink.Displayed);
            CustomerSettingsHolidayScheduleLink.Click();
            return this;
        }

        /**
         * Method to click the Tender Hours link under Customer Settings. 
         */
        public SettingsPage ClickTenderHoursLink()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CustomerSettingsTenderHoursLink.Displayed);
            CustomerSettingsTenderHoursLink.Click();
            return this;
        }

        /**
         * Method to check if a list of web elements contains specific text.
         */
        public Boolean DisplayedInResults(IList<IWebElement> elementList, string text)
        {
            foreach (IWebElement element in elementList)
            {
                if (element.Text == text)
                {
                    return true;
                }
            }
            return false;
        }
        
        /**
         * Method to search for a Carrier by Id. Switches to carrier search field and enters carrier id.
         */
        public new SettingsPage SearchCarrier(string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierSearchField.Displayed);
            CarrierSearchField.Clear();
            CarrierSearchField.SendKeys(carrierId);
            Wait.Until(d => CarrierIdSearchResults[0].Displayed);
            return this;
        }

        /**
         * Method to select a carrier by id from search results.
         */
        public new SettingsPage SelectCarrier(string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierIdSearchResults.Count > 0);
            foreach (IWebElement searchResult in CarrierIdSearchResults)
            {
                if (searchResult.Text == carrierId)
                {
                    searchResult.Click();
                    break;
                }
            }
            return this;
        }

        /**
         * Method to search for and select a carrier from search results.
         */
        public new SettingsPage SearchAndSelectCarrier(string carrierId)
        {
            SearchCarrier(carrierId);
            SelectCarrier(carrierId);
            return this;
        }

        /**
         * Method to search for a Customer by Id. Switches to customer search field and enters customer id.
         */
        public new SettingsPage SearchCustomer(string customerId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CustomerSearchField.Displayed);
            CustomerSearchField.Clear();
            CustomerSearchField.SendKeys(customerId);
            Wait.Until(d => CustomerIdSearchResults[0].Displayed);
            return this;
        }

        /**
         * Method to select a customer by id from search results.
         */
        public new SettingsPage SelectCustomer(string customerId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CustomerIdSearchResults.Count > 0);
            foreach (IWebElement searchResult in CustomerIdSearchResults)
            {
                if (searchResult.Text == customerId)
                {
                    searchResult.Click();
                    break;
                }
            }
            return this;
        }

        /**
         * Method to search for and select a customer from search results.
         */
        public new SettingsPage SearchAndSelectCustomer(string customerId)
        {
            SearchCustomer(customerId);
            SelectCustomer(customerId);
            return this;
        }

        /**
         * Method to check if a Carrier appears in the search results.
         */
        public Boolean CarrierDisplayed(string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierIdSearchResults.Count > 0);
            foreach (IWebElement searchResult in CarrierIdSearchResults)
            {
                if (searchResult.Text == carrierId)
                {
                    return true;
                }
            }
            return false;
        }

        /**
         * Method to check if contact appears in Carrier Contacts.
         */
        public Boolean DisplayedInCarrierContacts(string searchContact)
        {
            if (searchContact == "" && CarrierContacts.Count == 0)
            {
                return true;
            }
            foreach (IWebElement contact in CarrierContacts)
            {
                string carrierContact = Regex.Replace(contact.Text, @"[ /(/)\t\s/-]+", "");
                searchContact = Regex.Replace(searchContact, @"[ /(/)\t\s/-]+", "");
                if (carrierContact.ToLower().Contains(searchContact.ToLower()))
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
         * Method to set Customer Settings > General fields using specific values.
         */
        public SettingsPage SetCustomerGeneralFields(string carrierResponseTime, string schedulingLeadTime, string submissionDeadline, string notificationEmail, string timeZone)
        {
            DisableInheritSettings();
            SetFieldValue(CarrierResponseTimeField, carrierResponseTime);
            SetFieldValue(SchedulingLeadTimeField, schedulingLeadTime);
            SetFieldValue(SubmissionDeadlineField, submissionDeadline);
            SetFieldValue(NotificationEmailField, notificationEmail);
            SetTimeZone(timeZone);
            return this;
        }

        /**
         * Method to set Customer Settings > General fields using random values.
         */
        public SettingsPage SetCustomerGeneralFields()
        {
            Random rnd = new Random();
            string carrierResponseTime = rnd.Next(1, 10).ToString();
            string schedulingLeadTime = rnd.Next(100, 150).ToString();
            string submissionDeadline = rnd.Next(5, 20).ToString();
            string notificationEmail = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + "@echo.com";
            string timeZone = GetRandomTimeZone();
            SetCustomerGeneralFields(carrierResponseTime, schedulingLeadTime, submissionDeadline, notificationEmail, timeZone);
            return this;
        }
        
        /**
         * Method to disable the inheriting of settings from parent account.
         */
        public SettingsPage DisableInheritSettings()
        {
            Thread.Sleep(SleepTime / 2);
            if (ElementExists(InheritSettingsCheck))
            {
                driver.FindElementBy(InheritSettingsCheck).Click();
            }
            return this;
        }

        /**
         * Method to randomly set the Time Zone.
         */
        public SettingsPage SetRandomTimeZone()
        {
            SelectElement selector = new SelectElement(TimeZoneField);
            Random rnd = new Random();
            selector.SelectByIndex(rnd.Next(1, selector.Options.Count));
            return this;
        }

        /**
         * Method to set a specific Time Zone.
         */
        public SettingsPage SetTimeZone(string timeZone)
        {
            SelectElement selector = new SelectElement(TimeZoneField);
            selector.SelectByText(timeZone);
            return this;
        }

        /**
         * Method to randomly get the Time Zone.
         */
        public string GetRandomTimeZone()
        {
            SelectElement selector = new SelectElement(TimeZoneField);
            Random rnd = new Random();
            return selector.Options[rnd.Next(1, selector.Options.Count)].Text;
        }

        /**
         * Method to get the set Time Zone.
         */
        public string GetTimeZone()
        {
            SelectElement selector = new SelectElement(TimeZoneField);
            return selector.SelectedOption.Text;
        }

        public string GetTodaysDate(int daysPlusMinus=0)
        {
            string expectedDateFormat = "yyyy-MM-dd";
            return DateTime.Today.AddDays(daysPlusMinus).ToString(expectedDateFormat);
        }

        /**
         * Method to refresh the current page.
         */
        public SettingsPage Refresh()
        {
            Thread.Sleep(SleepTime * 2);
            driver.Navigate().Refresh();
            Thread.Sleep(SleepTime * 2);
            return this;
        }
    }
}
