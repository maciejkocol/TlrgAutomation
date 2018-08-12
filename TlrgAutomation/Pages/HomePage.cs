using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tlrg.Pages.Tlrg
{
    public class HomePage: BasePage
    {
        public HomePage(IWebDriver driver) : base(driver)
        {
            driver.Navigate().GoToUrl(env.TlrgURL);
            WaitForHomePageToLoad();
        }

        // main component
        public IWebElement GridComponent => driver.FindElementBy(By.XPath("(//load-details-grid-component[not(@hidden)] | //load-automation-grid-component[not(@hidden)])"));

        // dialogs
        public IWebElement StartAutomationDialog => driver.FindElementBy(By.XPath("//ngb-modal-window[@role='dialog' and contains(.,'Start Automation')]"));
        
        // menus and submenus
        public IWebElement HomeNavItem => driver.FindElementBy(By.XPath("//li[starts-with(@class,'nav-item') and contains(.,'Home')]"));
        public IWebElement PricingNavItem => driver.FindElementBy(By.XPath("//li[starts-with(@class,'nav-item') and contains(.,'Pricing')]"));
        public IWebElement PricingUploadSubItem => driver.FindElementBy(By.XPath("//li[starts-with(@class,'dropdown-item') and contains(.,'Upload')]"));
        public IWebElement PricingManageSubItem => driver.FindElementBy(By.XPath("//li[starts-with(@class,'dropdown-item') and contains(.,'Manage')]"));
        public IWebElement SettingsNavItem => driver.FindElementBy(By.XPath("//li[starts-with(@class,'nav-item') and contains(.,'Settings')]"));
        
        // tabs
        public IWebElement AllTab => driver.FindElementBy(By.XPath("//div[starts-with(@class,'tab-button') and starts-with(.,'All')]"));
        public IWebElement CompletedTab => driver.FindElementBy(By.XPath("//div[starts-with(@class,'tab-button') and starts-with(.,'Completed')]"));
        public IWebElement InProgressTab => driver.FindElementBy(By.XPath("//div[starts-with(@class,'tab-button') and starts-with(.,'In Progress')]"));
        public IWebElement CancelledTab => driver.FindElementBy(By.XPath("//div[starts-with(@class,'tab-button') and starts-with(.,'Cancelled')]"));
        public IWebElement FellThroughTab => driver.FindElementBy(By.XPath("//div[starts-with(@class,'tab-button') and starts-with(.,'Fell Through')]"));
        public IWebElement ArchivedTab => driver.FindElementBy(By.XPath("//div[starts-with(@class,'tab-button') and starts-with(.,'Archived')]"));
        public IWebElement AutoStartTab => driver.FindElementBy(By.XPath("//div[starts-with(@class,'tab-button') and starts-with(.,'Auto Start')]"));

        // buttons
        public IWebElement StartAutomationButton => GridComponent.FindElementBy(By.XPath("//button[@type='button' and starts-with(.,'Start Automation')]"));
        public IWebElement RefreshButton => GridComponent.FindElementBy(By.XPath("//button[@type='button' and starts-with(.,'Refresh')]"));
        public IWebElement SelectAllCheckbox => GridComponent.FindElementBy(By.XPath("//div[contains(@id,'columntable')]//div[contains(@class,'checkbox-default')]"));
        public IWebElement NextButton => GridComponent.FindElementBy(By.XPath("//div[@type='button' and @aria-disabled='false']/div[contains(@class,'arrow-right')]"));

        // fields
        public IWebElement CustomerSearchField => driver.FindElementBy(By.Id("txtCustomerSearch"));
        public IWebElement LoadIdField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Load ID']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement CarrierField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Carrier']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement CogsField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='COGS']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement OrigCityField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Orig City']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement OrigStField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Orig St']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement DestCityField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Dest City']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement DestStField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Dest St']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement PuDateField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Pu Date']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement CustomerNameField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Customer Name']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement CustomerENumberField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Customer E#']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement OrigZipField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Orig Zip']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement OrigCountryField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Orig Country']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement DestZipField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Dest Zip']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement DestCountryField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Dest Country']/preceding-sibling::*)+1]//input[@autocomplete='off']"));
        public IWebElement StatusField => GridComponent.FindElementBy(By.XPath("//div[starts-with(@id,'filterrow')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Status']/preceding-sibling::*)+1]//input[@autocomplete='off']"));

        // results
        public IList<IWebElement> CustomerSearchResults => driver.FindElementsBy(By.XPath("//div[@class='search-results']/div[@class='search-number']"));
        public IWebElement NoLoadResults => GridComponent.FindElementBy(By.XPath("//span[text()='No data to display']"));
        public IList<IWebElement> LoadResults => GridComponent.FindElementsBy(By.XPath("//div[contains(@id,'contenttable')]/div[@role='row']"));
        public IList<IWebElement> LoadDetailResults => GridComponent.FindElementsBy(By.XPath("//div[starts-with(@class,'load-details-info-container')]"));
        public IList<IWebElement> LoadIdResults => GridComponent.FindElementsBy(By.XPath("//div[starts-with(@id,'contenttable')]/div/div[count(ancestor::jqxgrid//div[@role='columnheader' and .='Load ID']/preceding-sibling::*)+1]/*[self::div or self::span][text()]"));
        public IList<IWebElement> CheckedResults => GridComponent.FindElements(By.XPath("//div[contains(@id,'contenttable')]/div[@role='row']//div[contains(@class,'checkbox-default')]//span[contains(@class,'checked')]"));
        
        // load details
        public IWebElement DetailsPopout => driver.FindElementBy(By.XPath("//load-details-popout/div[@class='load-details-popout-container' and not(contains(@style,'100%'))]"));
        public IWebElement LoadDetails => DetailsPopout.FindElementBy(By.XPath("//span[@class='load-details-loadid']"));

        // helper paths
        public By LoadResultsNarrowByText => By.XPath("div[@role='gridcell']/div[.]");
        public By LoadDetailResultsNarrowByText => By.XPath("*[(starts-with(@class,'load-details-carrier'))]");
        public By LoadCheckbox => By.XPath("ancestor::div[@role='row']//div[contains(@class,'checkbox-default')]");

        // queries
        public string RandomCustomerHasLoadsQuery => 
            "SELECT TOP 1 * " +
            "FROM XPlatform.Customer.Customer t1 " +
            "LEFT JOIN XPlatform.Routing.Load t2 on t2.CustomerId = t1.CustomerId " +
            "WHERE NOT ISNULL(t2.loadId, '') = '' " +
            "ORDER BY NEWID()";
        public string RandomLoadForCustomerQuery =>
            "SELECT TOP 1 * " +
            "FROM XPlatform.Customer.Customer t1 " +
            "LEFT JOIN XPlatform.Routing.Load t2 on t2.CustomerId = t1.CustomerId " +
            "WHERE NOT ISNULL(t2.loadId, '') = '' AND t1.CustomerId = 'E9704'" +
            "ORDER BY NEWID()";
        public string CustomerLoadsQuery =>
            "SELECT DISTINCT t1.LoadId AS 'LOAD ID', " +
            "CASE " +
                "WHEN t2.CurrentCarrierName IS NULL THEN " +
                    "CASE " +
                        "WHEN t3.RoutingKey IS NULL THEN " +
                        "'No Routes Found' " +
                        "WHEN t2.RouteStatus = 'Not Started' THEN " +
                        "'Routing Not Started' " +
                        "WHEN t2.RouteStatus = 'In Progress' OR t2.RouteStatus = 'Cancelled' THEN " +
                        "'Ready for next carrier' " +
                        "WHEN t2.RouteStatus = 'Fall Through' THEN " +
                        "'' " +
                        "END " +
                "ELSE t2.CurrentCarrierName  + ' (' + t2.CurrentCarrierId + ')' " +
                "END AS 'CARRIER', " +
            "CASE " +
                "WHEN t2.CurrentCarrierRank LIKE '%out of%' " +
                    "THEN REPLACE(t2.CurrentCarrierRank, 'out ', '') " +
                        "WHEN t2.CurrentCarrierRank IS NULL " +
                            "THEN '0 of ' + CONVERT(VARCHAR(10), ( " +
                                "SELECT COALESCE(MAX(Rank), 0) " +
                                "FROM XPlatform.Routing.Route r1 " +
                                "WHERE r1.RoutingKey = t2.RoutingKey " +
                                ")) " +
                "END AS 'CARRIER RANK', " +
            "CASE " +
                "WHEN t2.CurrentCarrierCogs IS NULL " +
                    "THEN '$--' " +
                    "ELSE FORMAT(t2.CurrentCarrierCogs, '$####.##') " +
                "END AS 'COGS', " +
            "t2.RouteStatus AS 'STATUS', " +
            "t1.StartOriginCity AS 'ORIG CITY', " +
            "t1.StartOriginState AS 'ORIG ST', " +
            "t1.EndDestinationCity AS 'DEST CITY', " +
            "t1.EndDestinationState AS 'DEST ST', " +
            "FORMAT(t1.PickUpDate, 'M/d/yyyy') AS 'PU DATE' " +
            "FROM XPlatform.Routing.Load t1 " +
            "LEFT JOIN XPlatform.Routing.Routing t2 on t2.RoutingKey = t1.RoutingKey " +
            "LEFT JOIN XPlatform.Routing.Route t3 on t3.RoutingKey = t2.RoutingKey " +
            "WHERE t1.CustomerId = '{0}' AND t2.RouteStatus != 'Archived'  " +
            "ORDER BY t1.LoadId DESC ";
        public string CustomerLoadsAutoStartQuery =>
            "SELECT DISTINCT t1.LoadId AS 'LOAD ID', " +
            "t1.StartOriginCity AS 'ORIG CITY', " +
            "t1.StartOriginState AS 'ORIG ST', " +
            "t1.StartOriginPostalCode AS 'ORIG ZIP', " +
            "t1.StartOriginCountry AS 'ORIG COUNTRY', " +
            "t1.EndDestinationCity AS 'DEST CITY', " +
            "t1.EndDestinationState AS 'DEST ST', " +
            "t1.EndDestinationPostalCode AS 'DEST ZIP', " +
            "t1.EndDestinationCountry AS 'DEST COUNTRY', " +
            "t1.Status AS 'STATUS' " +
            "FROM XPlatform.Load.Load t1 " +
            "WHERE t1.CustomerId = '{0}' AND (t1.Status = 'PendingUpdated' OR t1.Status = 'Pending') " +
            "ORDER BY t1.LoadId DESC ";
        public string LoadDetailsQuery =>
            "SELECT DISTINCT t3.Id, t3.CarrierName AS 'CARRIER NAME', FORMAT(t3.TotalCOGs, '$####.##') AS 'COGS', t3.CarrierId AS 'CARRIER ID' FROM " +
            "XPlatform.Routing.Route t1 " +
            "LEFT JOIN XPlatform.Routing.Routing t2 on t1.RoutingKey = t2.RoutingKey " +
            "LEFT JOIN XPlatform.Routing.Route t3 on t3.RoutingKey = t2.RoutingKey " +
            "WHERE t1.LoadId = '{0}' AND t2.RouteStatus != 'Archived' " +
            "ORDER BY t3.Id ASC ";

        /**
         * Method to wait for the navigation objects to load on the Home page.
         */
        public void WaitForHomePageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => HomeNavItem.Displayed);
            Wait.Until(d => GridComponent.Displayed);
        }

        /**
         * Method to search for and select a customer by id. 
         * Switches to customer search field, enters customer id, and selects it from search results.
         */
        public void SearchAndSelectCustomer(string customerId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CustomerSearchField.Displayed);
            CustomerSearchField.Clear();
            CustomerSearchField.SendKeys(customerId);
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
         * Method to filter loads displayed in results. Filtering is done by entering text in a filter field.
         */
        public void FilterLoads(IWebElement filterField, string filterString)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => filterField.Displayed);
            filterField.Clear();
            filterField.SendKeys(filterString);
            Thread.Sleep(SleepTime);
        }

        /**
         * Method to get the number of checked results.
         */
        public int CheckedResultsCount()
        {
            return CheckedResults.Count();
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
            for (int i=0; i<LoadResults.Count; i++)
            {
                if (i == index)
                {
                    IWebElement loadResult = LoadResults[i];
                    actualLoad = new List<string>();
                    LoadRecords = loadResult.FindElementsBy(LoadResultsNarrowByText);
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
            return LoadDetailResults[index].Text;
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
        public void ClickOnLoad(string loadId)
        {
            foreach (IWebElement loadIdResult in LoadIdResults)
            {
                if (loadIdResult.Text == loadId)
                {
                    loadIdResult.Click();
                    break;
                }
            }
        }

        /**
         * Method to select a checkbox next to a specific load based on Load Id. 
         * Works only in the Auto Start tab.
         */
        public void SelectLoad(string loadId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => LoadIdResults.First().Displayed);
            foreach (IWebElement loadIdResult in LoadIdResults)
            {
                if (loadIdResult.Text == loadId)
                {
                    loadIdResult.FindElementBy(LoadCheckbox).Click();
                    break;
                }
            }
        }

        /**
         * Method to click on Home menu item.
         */
        public void ClickHomeNavItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => HomeNavItem.Displayed);
            HomeNavItem.Click();
        }

        /**
         * Method to click on Pricing menu item.
         */
        public void ClickPricingNavItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => PricingNavItem.Displayed);
            PricingNavItem.Click();
        }

        /**
         * Method to click on Settings menu item.
         */
        public void ClickSettingsNavItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SettingsNavItem.Displayed);
            SettingsNavItem.Click();
        }

        /**
         * Method to click on Pricing > Upload submenu.
         */
        public void ClickPricingUploadSubItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => PricingUploadSubItem.Displayed);
            PricingUploadSubItem.Click();
        }

        /**
         * Method to click on Pricing > Manage submenu.
         */
        public void ClickPricingManageSubItem()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => PricingManageSubItem.Displayed);
            PricingManageSubItem.Click();
        }

        /**
         * Method to click on the All tab.
         */
        public void ClickAllTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => AllTab.Displayed);
            AllTab.Click();
        }

        /**
         * Method to click on the Completed tab.
         */
        public void ClickCompletedTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CompletedTab.Displayed);
            CompletedTab.Click();
        }

        /**
         * Method to click on the In Progress tab.
         */
        public void ClickInProgressTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => InProgressTab.Displayed);
            InProgressTab.Click();
        }

        /**
         * Method to click on the Cancelled tab.
         */
        public void ClickCancelledTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => CancelledTab.Displayed);
            CancelledTab.Click();
        }

        /**
         * Method to click on the Fell Through tab.
         */
        public void ClickFellThroughTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => FellThroughTab.Displayed);
            FellThroughTab.Click();
        }

        /**
         * Method to click on the Archived tab.
         */
        public void ClickArchivedTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => ArchivedTab.Displayed);
            ArchivedTab.Click();
        }

        /**
         * Method to click on the Auto Start tab.
         */
        public void ClickAutoStartTab()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => AutoStartTab.Displayed);
            AutoStartTab.Click();
        }

        /**
         * Method to click the Start Automation button. Starts automation for selected loads.
         */
        public void ClickStartAutomationButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => StartAutomationButton.Displayed);
            StartAutomationButton.Click();
        }

        /**
         * Method to click the Refresh button. Refreshes load search results.
         */
        public void ClickRefreshButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => RefreshButton.Displayed);
            RefreshButton.Click();
        }

        /**
         * Method to click the select-all checkbox for load results. Selects or disselects all load results.
         */
        public void ClickSelectAllCheckbox()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => SelectAllCheckbox.Displayed);
            SelectAllCheckbox.Click();
        }

        /**
         * Method to click the next button. Takes user to next page of load results.
         */
        public void NextPage()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => NextButton.Displayed);
            NextButton.Click();
            Wait.Until(d => NextButton.Displayed);
        }

        /**
         * Method to get a random Customer record with loads.
         */
        public DataRow GetRandomCustomerHasLoadsRow()
        {
            return GetData(RandomCustomerHasLoadsQuery).Rows[0];
        }

        /**
         * Method to get all load records for a particular Customer.
         */
        public DataTable GetCustomerLoadsTable(string customerId)
        {
            string query = string.Format(CustomerLoadsQuery, customerId);
            return GetData(query);
        }

        /**
         * Method to get pending load records for a particular Customer.
         */
        public DataTable GetCustomerLoadsAutoStartTable(string customerId)
        {
            string query = string.Format(CustomerLoadsAutoStartQuery, customerId);
            return GetData(query);
        }

        /**
         * Method to get load detail records for a particular Customer.
         */
        public DataTable GetLoadDetailsTable(string customerId)
        {
            string query = string.Format(LoadDetailsQuery, customerId);
            return GetData(query);
        }
    }
}
