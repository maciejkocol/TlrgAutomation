using System;
using System.Collections.Generic;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;

namespace TlrgAutomation.Pages.Optimizer
{
    public class CarrierPage : BasePage
    {
        public CarrierPage() : base()
        {
            driver.Navigate().GoToUrl(env.OptiURL + "/EWSHome.aspx?setRealm=CARRIER");
            AttemptOptimizerLogin(driver, env.OptiUser, env.OptiPassword);
            SwitchToOptimizerEwsFrame(driver);
            WaitForCarrierPageToLoad();
        }

        // links
        public IList<IWebElement> CarrierFolders => driver.FindElements(By.XPath("//li/span[@class='folder'][text()]"));
        public IList<IWebElement> CarrierFolderDraggables => driver.FindElementsBy(By.XPath("//li/span[@class='folder'][text()]/..//span[@class='file draggable grabbable ui-draggable']"));

        // fields
        new public IWebElement CarrierSearchField => driver.FindElementBy(By.Id("search-field"));

        // buttons
        public IWebElement CarrierEdiEditButton => driver.FindElementBy(By.XPath("//div[starts-with(@id,'edi')]//input[@id='btnEdit']"));
        public IWebElement CarrierEdiSaveButton => driver.FindElementBy(By.XPath("//div[starts-with(@id,'edi')]//input[@id='edi_edit_submit']"));
        public IWebElement CarrierProfileEditButton => driver.FindElementBy(By.XPath("//div[starts-with(@id,'carrierprofile')]//input[@id='btnEdit']"));

        // checkboxes
        public IWebElement Edi204TLAutoTenderCheckbox => driver.FindElementBy(By.XPath("//input[@class='checkbox' and @id='Outbound204TLAutoTender']"));

        // search results
        public IList<IWebElement> CarrierSearchResults => driver.FindElementsBy(By.XPath("//ul[@role='listbox']/li[@role='menuitem']/a[text()]"));

        /**
         * Method to select the Outbound 204 TL Auto Tender (EDI) checkbox. 
         */
        public CarrierPage ClickEdi204TLAutoTenderCheckbox()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => Edi204TLAutoTenderCheckbox.Displayed);
            Edi204TLAutoTenderCheckbox.Click();
            Thread.Sleep(SleepTime);
            return this;
        }

        /**
         * Method to click the Edit button in EDI settings.
         */
        public CarrierPage ClickCarrierEdiEditButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierEdiEditButton.Displayed);
            CarrierEdiEditButton.Click();
            Thread.Sleep(SleepTime);
            return this;
        }

        /**
         * Method to click the Save button in EDI settings. 
         */
        public CarrierPage ClickCarrierEdiSaveButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierEdiSaveButton.Displayed);
            CarrierEdiSaveButton.Click();
            Wait.Until(d => CarrierEdiEditButton.Displayed);
            Thread.Sleep(SleepTime);
            return this;
        }

        /**
         * Method to click the Edit button in Profile Info settings. 
         */
        public CarrierPage ClickCarrierProfileEditButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierProfileEditButton.Displayed);
            CarrierProfileEditButton.Click();
            return this;
        }

        /**
         * Method to wait for the navigation objects to load on the Carrier page.
         */
        public CarrierPage WaitForCarrierPageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierSearchField.Displayed);
            return this;
        }

        /**
         * Method to search for a Carrier by Id. Switches to carrier search field and enters carrier id.
         */
        public new CarrierPage SearchCarrier(string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierSearchField.Displayed);
            CarrierSearchField.Clear();
            CarrierSearchField.SendKeys(carrierId);
            return this;
        }

        /**
         * Method to select a carrier by id from search results.
         */
        public new CarrierPage SelectCarrier(string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierSearchResults.Count > 0);
            foreach (IWebElement searchResult in CarrierSearchResults)
            {
                Wait.Until(d => searchResult.Displayed);
                if (searchResult.Text.Contains(carrierId))
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
        public new CarrierPage SearchAndSelectCarrier(string carrierId)
        {
            SearchCarrier(carrierId);
            SelectCarrier(carrierId);
            return this;
        }

        /**
         * Method to click on an expandable folder.
         */
        public CarrierPage AccessFolder(string name)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
            Wait.Until(d => CarrierFolders.Count > 0);
            foreach (IWebElement carrierFolder in CarrierFolders)
            {
                Wait.Until(d => carrierFolder.Displayed);
                if (carrierFolder.Text == name)
                {
                    carrierFolder.Click();
                }
            }
            return this;
        }

        /**
         * Method to double-click on a draggable item under a folder.
         */
        public CarrierPage AccessFolderDraggable(string name)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CarrierFolderDraggables.Count > 0);
            foreach (IWebElement carrierFolderDraggable in CarrierFolderDraggables)
            {
                Wait.Until(d => carrierFolderDraggable.Displayed);
                if (carrierFolderDraggable.Text == name)
                {
                    Actions action = new Actions(driver);
                    action.DoubleClick(carrierFolderDraggable);
                    action.Perform();
                }
            }
            return this;
        }
    }
}
