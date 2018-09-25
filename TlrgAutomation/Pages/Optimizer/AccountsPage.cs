using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Data;

namespace TlrgAutomation.Pages.Optimizer
{
    public class AccountsPage : BasePage
    {
        public AccountsPage() : base()
        {
            driver.Navigate().GoToUrl(env.OptiURL + "/AccountsHome.aspx");
            AttemptOptimizerLogin(driver, env.OptiUser, env.OptiPassword);
            WaitForAccountsPageToLoad();
        }

        // fields
        public By AccountIdField = By.Id("ftbCustomerId");

        // tabs
        public By AccountSettingsTab = By.XPath("//label[@class='TabFont' and contains(text(),'Settings')]");

        // menus
        public By EnableTlrgSubMenu = By.XPath("//div[@class='account-settings-header']//*[contains(.,'Enable TLRG')]");

        // checkboxes
        public By EnableTlrgCheckbox = By.Id("cbEnableTlrg");

        // buttons
        public By SearchAccountButton = By.Id("btnSearch");
        public By EnableTlrgSaveButton = By.Id("btnEnableTlrgSave");
        public By NextPageButton = By.Id("ucPageNavBottom_bPostedNext");

        // search results
        public By AccountIdSearchResults = By.XPath("//tr[contains(@class,'Style')][@onmouseout]/td[2][starts-with(text(),'E')]");
        public By AccountNameSearchResults = By.XPath("//tr[contains(@class,'Style')][@onmouseout]/td[4]/a[starts-with(@id,'ucAccountsList_dgAccounts_ctl')]");

        /**
         * Method to wait for the main objects to load on the Accounts page.
         */
        public void WaitForAccountsPageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement AccountIdFieldElement = driver.FindElementBy(AccountIdField);
            Wait.Until(d => AccountIdFieldElement.Displayed);
        }

        /**
         * Method to click the Search account button.
         */
        public AccountsPage ClickSearchAccountButton()
        {
            ClickElement(SearchAccountButton);
            return this;
        }

        /**
         * Method to sync Customer with TLRG using Optimizer.
         */
        public AccountsPage SyncCustomerWithTlrg(string customerId)
        {
            SearchAndSelectAccountById(customerId);
            ClickElement(AccountSettingsTab);
            ClickElement(EnableTlrgSubMenu);
            // IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
            // string checkboxId = driver.FindElementBy(EnableTlrgCheckbox).GetAttribute("id");
            // if (!(Boolean)js.ExecuteScript("return document.getElementById('" + checkboxId + "').checked;"))
            if (!driver.FindElementBy(EnableTlrgCheckbox).Selected)
            {
                ClickElement(EnableTlrgCheckbox);
                ClickElement(EnableTlrgSaveButton);
                WaitUntilTlrgEnabled();
                Thread.Sleep(SleepTime * 3);
            }
            return this;
        }

        /**
         * Finds 'random' unsynced Customer and syncs it
         * returns CustomerId of newly synced customer
         */
        public String GetAndSyncNewCustomerWithTlrg()
        {
            DataRow customerRow = dbAccess.GetUnsyncedCustomerRow();
            String customerId = customerRow["CustomerId"].ToString();
            SyncCustomerWithTlrg(customerId);
            return customerId;
        }

        /**
         * Method to wait for TLRG Enabled checkbox to be checked. 
         */
        public AccountsPage WaitUntilTlrgEnabled()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement enableTlrgElement = driver.FindElementBy(EnableTlrgCheckbox);
            Wait.Until(d => enableTlrgElement.GetAttribute("checked"));
            return this;
        }

        /**
         * Method to search for and select a customer from search results.
         */
        public AccountsPage SearchAndSelectAccountById(string customerId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            SetFieldValue(AccountIdField, customerId);
            ClickSearchAccountButton();
            IList<IWebElement> accountIdElements = driver.FindElementsBy(AccountIdSearchResults);
            IList<IWebElement> accountNameElements = driver.FindElementsBy(AccountNameSearchResults);
            Wait.Until(d => accountIdElements.Count > 0);
            for (int i = 0; i < accountIdElements.Count; i++)
            {
                string accountId = accountIdElements[i].Text;
                if (accountId.Equals(customerId))
                {
                    accountNameElements[i].Click();
                    break;
                }
                if (i == accountIdElements.Count - 1)
                {
                    ClickElement(NextPageButton);
                    Thread.Sleep(SleepTime);
                    i = 0;
                }
            }
            return this;
        }
    }
}
