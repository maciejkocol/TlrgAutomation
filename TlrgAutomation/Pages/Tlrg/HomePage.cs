using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TlrgAutomation.Pages.Tlrg
{
    public class HomePage : BasePage
    {
        public HomePage() : base()
        {
            driver.Navigate().GoToUrl(env.TlrgURL);
            WaitForHomePageToLoad();
        }

        // main component
        public IWebElement RoutingGuidesGrid => driver.FindElementBy(By.XPath("//app-load-details-grid[not(@hidden)]"));
        public IWebElement AutoStartGrid => driver.FindElementBy(By.XPath("//app-load-automation-grid[not(@hidden)]"));

        // dialogs
        public IWebElement StartDialog => driver.FindElementBy(By.XPath("//ngb-modal-window[@role='dialog' and contains(.,'Start Automation')]"));

        // menus and submenus
        public IWebElement HomeNavItem => driver.FindElementBy(By.Id("home-nav-link"));
        public IWebElement PricingNavItem => driver.FindElementBy(By.ClassName("nav-dropdown-toggle"));
        public IWebElement PricingUploadSubItem => driver.FindElementBy(By.Id("pricing-upload-nav-link"));
        public IWebElement PricingManageSubItem => driver.FindElementBy(By.Id("pricing-manage-nav-link"));
        public IWebElement SettingsNavItem => driver.FindElementBy(By.Id("settings-nav-link"));

        // tabs
        public IWebElement RoutingGuidesTab => driver.FindElementBy(By.Id("all-tab"));
        public IWebElement CompletedTab => driver.FindElementBy(By.Id("completed-tab"));
        public IWebElement InProgressTab => driver.FindElementBy(By.Id("in-progress-tab"));
        public IWebElement CancelledTab => driver.FindElementBy(By.Id("cancelled-tab"));
        public IWebElement FellThroughTab => driver.FindElementBy(By.Id("fall-through-tab"));
        public IWebElement ArchivedTab => driver.FindElementBy(By.Id("archived-tab"));
        public IWebElement AutoStartTab => driver.FindElementBy(By.Id("auto-start-tab"));
        public IWebElement ReverseAuctionTab => driver.FindElementBy(By.Id("reverse-auction-tab"));

        // buttons
        public IWebElement StartButton => driver.FindElementBy(By.XPath("//button[@aria-label='Start']"));
        public IWebElement RefreshButton => driver.FindElementBy(By.XPath("//button[@aria-label='Refresh']"));
        public IWebElement SelectAllCheckbox => driver.FindElementBy(By.XPath("//mat-checkbox[@id='mat-checkbox-1']//div[starts-with(@class,'mat-checkbox-inner-container')]"));
        public IWebElement NextButton => driver.FindElementBy(By.XPath("//button[@type='button' and @aria-label='Next page']"));

        // fields
        public IWebElement LoadIdField => driver.FindElementBy(By.Id("loadIdInput"));
        public IWebElement CarrierField => driver.FindElementBy(By.Id("carrierInput"));
        public IWebElement CogsField => driver.FindElementBy(By.Id("coGsInput"));
        public IWebElement OrigCityField => driver.FindElementBy(By.Id("originCityInput"));
        public IWebElement OrigStField => driver.FindElementBy(By.Id("originStateInput"));
        public IWebElement DestCityField => driver.FindElementBy(By.Id("destinationCityInput"));
        public IWebElement DestStField => driver.FindElementBy(By.Id("destinationStateInput"));
        public IWebElement PuDateField => driver.FindElementBy(By.Id("pickUpDateInput"));
        public IWebElement OrigZipField => driver.FindElementBy(By.Id("originZipInput"));
        public IWebElement OrigCountryField => driver.FindElementBy(By.Id("originCountryInput"));
        public IWebElement DestZipField => driver.FindElementBy(By.Id("destinationZipInput"));
        public IWebElement DestCountryField => driver.FindElementBy(By.Id("destinationCountryInput"));
        public IWebElement StatusField => driver.FindElementBy(By.Id("statusInput"));
        
        // load details
        public IWebElement DetailsPopout => driver.FindElementBy(By.XPath("//app-load-details-popout/div[starts-with(@class,'load-details-popout-container') and not(contains(@style,'100%'))]"));
        public IWebElement LoadRoutingDetails => DetailsPopout.FindElement(By.XPath("//app-routing-details[not(contains(@class,'hide'))]"));
        public IWebElement LoadAuctionDetails => DetailsPopout.FindElement(By.XPath("//app-auction-details[not(contains(@class,'hide'))]"));
        public IWebElement LoadRoutingDetailsHeader => DetailsPopout.FindElement(By.XPath("//app-auction-details[not(contains(@class,'hide'))]//span[starts-with(@class,'loadid')]"));
        public IWebElement LoadAuctionDetailsHeader => DetailsPopout.FindElement(By.XPath("//app-auction-details[not(contains(@class,'hide'))]//span[starts-with(@class,'loadid')]"));

        // results
        public IList<IWebElement> LoadResults => RoutingGuidesGrid.FindElementsBy(By.XPath("//mat-row[@role='row']"));
        public IList<IWebElement> LoadRoutingDetailResults => DetailsPopout.FindElementsBy(By.XPath("//app-routing-details[not(contains(@class,'hide'))]//div[starts-with(@class,'details-carrier ')]"));
        public IList<IWebElement> LoadAuctionDetailResults => DetailsPopout.FindElementsBy(By.XPath("//app-auction-details[not(contains(@class,'hide'))]//div[starts-with(@class,'details-carrier ')]"));
        public IList<IWebElement> LoadIdResults => RoutingGuidesGrid.FindElementsBy(By.XPath("//mat-cell[starts-with(@id,'loadId')]"));
        public IList<IWebElement> AutostartCheckboxes => AutoStartGrid.FindElements(By.XPath("//mat-checkbox[starts-with(@id,'startWaterfall')]"));

        /**
         * Method to wait for the navigation objects to load on the Home page.
         */
        public HomePage WaitForHomePageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => HomeNavItem.Displayed);
            Wait.Until(d => RoutingGuidesGrid.Displayed);
            return this;
        }

        /**
         * Method to wait for newly created load to appear in XPlatform DB.
         */
        public HomePage WaitForStartTlrgLoad(string loadId, string customerId, string routeStatus)
        {
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    DataRow customerRow = dbAccess.GetCustomerLoadRow(loadId, customerId, routeStatus);
                    if (customerRow["LoadId"].ToString().Contains(loadId))
                    {
                        break;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
                attempts++;
                Thread.Sleep(SleepTime);
            }
            return this;
        }

        /**
         * Method to filter loads displayed in results. Filtering is done by entering text in a filter field.
         */
        public HomePage FilterLoads(IWebElement filterField, string filterString)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => filterField.Displayed);
            filterField.Clear();
            filterField.SendKeys(filterString);
            Thread.Sleep(SleepTime * 2);
            return this;
        }

        public new HomePage SearchAndSelectCustomer(string customerId)
        {
            return (HomePage)(base.SearchAndSelectCustomer(customerId));
        }

        /**
         * Method to get the number of checked results.
         */
        public int CheckedResultsCount()
        {
            int count = 0;
            foreach (IWebElement checkbox in AutostartCheckboxes)
            {
                if (checkbox.GetAttribute("class").Contains("checked"))
                {
                    count++;
                }
            }
            return count;
        }

        /**
         * Method to get particular load values at the specified index. 
         */
        public IList<string> GetLoadAtIndex(int index)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => LoadResults.Count > 0);
            IList<string> actualLoad = null;
            IList<IWebElement> LoadRecords = null;
            for (int i = 0; i < LoadResults.Count; i++)
            {
                if (i == index)
                {
                    IWebElement loadResult = LoadResults[i];
                    actualLoad = new List<string>();
                    LoadRecords = loadResult.FindElementsBy(By.XPath("mat-cell"));
                    foreach (IWebElement loadRecord in LoadRecords)
                    {
                        actualLoad.Add(loadRecord.Text);
                    }
                    return actualLoad;
                }
            }
            return new List<string>();
        }

        /**
         * Method to get details of a particular load at the specified index.
         */
        public string GetLoadDetailsAtIndex(int index)
        {
            return LoadAuctionDetailResults[index].Text;
        }

        /**
         * Method to calculate the CoGs for a load.
         */
        public string GetLoadCogs(string loadId, double cpm, double tarp, double stop, double rpm, double min)
        {
            DataRow customerRow = dbAccess.GetLoadDistanceRow(loadId);
            double distance = double.Parse(customerRow["Distance"].ToString());
            double fuel = distance * rpm;
            double team = distance * cpm;
            double total = fuel + team + tarp + stop + min;
            return "$" + Math.Round(total, 2).ToString();
        }

        /**
         * Method to tell if a specific Load Id is displayed in results.
         */
        public Boolean LoadIdDisplayed(string loadId)
        {
            foreach (IWebElement loadIdResult in LoadIdResults)
            {
                if (loadIdResult.Text == loadId)
                    return true;
            }
            return false;
        }

        /**
         * Method to click on a Load Id to view load details. Works in all tabs, other than Auto Start. 
         */
        public HomePage ClickOnLoad(string loadId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => LoadIdResults.First().Displayed);
            foreach (IWebElement loadIdResult in LoadIdResults)
            {
                if (loadIdResult.Text == loadId)
                {
                    loadIdResult.Click();
                    break;
                }
            }
            return this;
        }

        /**
         * Method to get the load ID displayed in load details pane. 
         */
        public string GetLoadIdFromAuctionDetails()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => LoadAuctionDetailsHeader.Displayed);
            return Regex.Replace(LoadAuctionDetailsHeader.Text, @"[^\d]", "");
        }

        /**
         * Method to select a checkbox next to a specific load based on Load Id. 
         * Works only in the Auto Start tab.
         */
        public HomePage SelectLoad(string loadId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            foreach (IWebElement checkbox in AutostartCheckboxes)
            {
                if (checkbox.GetAttribute("id").Contains(loadId))
                {
                    checkbox.Click();
                    break;
                }
            }
            return this;
        }

        /**
         * Method to click on Home menu item.
         */
        public HomePage ClickHomeNavItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => HomeNavItem.Displayed);
            HomeNavItem.Click();
            return this;
        }

        /**
         * Method to click on Pricing menu item.
         */
        public HomePage ClickPricingNavItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => PricingNavItem.Displayed);
            PricingNavItem.Click();
            return this;
        }

        /**
         * Method to click on Settings menu item.
         */
        public HomePage ClickSettingsNavItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SettingsNavItem.Displayed);
            SettingsNavItem.Click();
            return this;
        }

        /**
         * Method to click on Pricing > Upload submenu.
         */
        public HomePage ClickPricingUploadSubItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => PricingUploadSubItem.Displayed);
            PricingUploadSubItem.Click();
            return this;
        }

        /**
         * Method to click on Pricing > Manage submenu.
         */
        public HomePage ClickPricingManageSubItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => PricingManageSubItem.Displayed);
            PricingManageSubItem.Click();
            return this;
        }

        /**
         * Method to click on the All tab.
         */
        public HomePage ClickAllTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => RoutingGuidesTab.Displayed);
            RoutingGuidesTab.Click();
            return this;
        }

        /**
         * Method to click on the Completed tab.
         */
        public HomePage ClickCompletedTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CompletedTab.Displayed);
            CompletedTab.Click();
            return this;
        }

        /**
         * Method to click on the In Progress tab.
         */
        public HomePage ClickInProgressTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => InProgressTab.Displayed);
            InProgressTab.Click();
            return this;
        }

        /**
         * Method to click on the Cancelled tab.
         */
        public HomePage ClickCancelledTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CancelledTab.Displayed);
            CancelledTab.Click();
            return this;
        }

        /**
         * Method to click on the Fell Through tab.
         */
        public HomePage ClickFellThroughTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => FellThroughTab.Displayed);
            FellThroughTab.Click();
            return this;
        }

        /**
         * Method to click on the Archived tab.
         */
        public HomePage ClickArchivedTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => ArchivedTab.Displayed);
            ArchivedTab.Click();
            return this;
        }

        /**
         * Method to click on the Auto Start tab.
         */
        public HomePage ClickAutoStartTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => AutoStartTab.Displayed);
            AutoStartTab.Click();
            return this;
        }

        /**
         * Method to click the Start Automation button. Starts automation for selected loads.
         */
        public HomePage ClickStartAutomationButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => StartButton.Displayed);
            StartButton.Click();
            return this;
        }

        /**
         * Method to click the Refresh button. Refreshes load search results.
         */
        public HomePage ClickRefreshButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => RefreshButton.Displayed);
            RefreshButton.Click();
            return this;
        }

        /**
         * Method to click the select-all checkbox for load results. Selects or disselects all load results.
         */
        public HomePage ClickSelectAllCheckbox()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SelectAllCheckbox.Displayed);
            Thread.Sleep(SleepTime / 2);
            SelectAllCheckbox.Click();
            Thread.Sleep(SleepTime / 2);
            return this;
        }

        /**
         * Method to click the next button. Takes user to next page of load results.
         */
        public HomePage NextPage()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => NextButton.Displayed);
            NextButton.Click();
            Wait.Until(d => NextButton.Displayed);
            return this;
        }
    }
}
