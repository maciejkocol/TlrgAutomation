using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using NUnit.Framework;
using TlrgAutomation.Pages.Optimizer;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium.Interactions;
using TlrgAutomation.Managers;
using SeleniumExtras.PageObjects;
using System.Collections.Generic;
using System.Globalization;

namespace TlrgAutomation.Pages
{
    public class BasePage
    {
        public IWebDriver driver;
        public EnvironmentManager.Environment env;
        public LoadManager.Load load;

        public DatabaseAccessManager dbAccess;

        public BasePage()
        {
            this.driver = Fixtures.GetWebDriver();
            PageFactory.InitElements(driver, this);
            env = EnvironmentManager
                .GetEnvironment(TestContext.Parameters["TLRGEnv"] ?? Properties.RunSettings.Default.Run_Environment);
            load = LoadManager.GetLoad();
            dbAccess = new DatabaseAccessManager();
        }

        // default sleep time in milliseconds
        public const int SleepTime = 2000;

        public static By EwsFrameOptimizer = By.Id("ewsFrame"); // ews frame in Optimizer
        public static By MenuFrameOptimizer = By.Id("topMenuFrame"); // menu frame in Optimizer
        public static By WarehouseOptimizer = By.Id("WarehouseMaintenanceIframe"); // warehouse maintenance frame in Optimizer

        // fields
        public IWebElement CarrierSearchField => driver.FindElementBy(By.Id("txtCarrierSearch"));
        public IWebElement CustomerSearchField => driver.FindElementBy(By.Id("txtCustomerSearch"));

        // search results
        public IList<IWebElement> CarrierIdSearchResults => driver.FindElementsBy(By.XPath("//div[starts-with(@class,'search-results-container') and not(contains(@class,'empty'))]//div[starts-with(@class,'search-number') or contains(.,'No carriers found')]"));
        public IList<IWebElement> CarrierNameSearchResults => driver.FindElementsBy(By.XPath("//div[starts-with(@class,'search-results-container') and not(contains(@class,'empty'))]//div[starts-with(@class,'search-name') or contains(.,'No carriers found')]"));
        public IList<IWebElement> CustomerIdSearchResults => driver.FindElementsBy(By.XPath("//div[starts-with(@class,'search-results-container') and not(contains(@class,'empty'))]//div[starts-with(@class,'search-number') or contains(.,'No customers found')]"));
        public IList<IWebElement> CustomerNameSearchResults => driver.FindElementsBy(By.XPath("//div[starts-with(@class,'search-results-container') and not(contains(@class,'empty'))]//div[starts-with(@class,'search-name') or contains(.,'No customers found')]"));

        /**
         * Method to switch to a EWS frame within Optimizer.
         */
        public static void SwitchToOptimizerEwsFrame(IWebDriver Driver)
        {
            SwitchToFrame(EwsFrameOptimizer, Driver);
        }

        /**
         * Method to switch to a Warehouse frame within Optimizer.
         */
        public static void SwitchToOptimizerWarehouseFrame(IWebDriver Driver)
        {
            SwitchToFrame(WarehouseOptimizer, Driver);
        }

        /**
         * Method to switch to a Menu frame within Optimizer.
         */
        public static void SwitchToOptimizerMenuFrame(IWebDriver Driver)
        {
            SwitchToFrame(MenuFrameOptimizer, Driver);
        }

        /**
         * Method to switch to a specific iframe within Optimizer.
         */
        public static void SwitchToFrame(By ByFrame, IWebDriver Driver)
        {
            WebDriverWait Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(10))
            {
                PollingInterval = TimeSpan.FromMilliseconds(300)
            };
            try
            {
                Wait.Until(d => d.FindElements(ByFrame).Count > 0);
                IWebElement parentFrame = Driver.FindElementBy(ByFrame);
                Driver.SwitchTo().Frame(parentFrame);
            }
            catch (Exception)
            {
                throw new NoSuchElementException("Unable to find element, locator: \"" + ByFrame.ToString() + "\".");
            }
        }

        /**
         * Method to log into Optimizer, if needed.
         */
        public static void AttemptOptimizerLogin(IWebDriver Driver, string user, string pass)
        {
            LoginPage optimizerLoginPageElements = new LoginPage();
            if (optimizerLoginPageElements.PageDisplayed())
            {
                optimizerLoginPageElements.Login(user, pass);
            }
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
            Wait.Until(d => CarrierIdSearchResults[0].Displayed);
        }

        /**
         * Method to select a carrier by id from search results.
         */
        public void SelectCarrier(string carrierId)
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
            Wait.Until(d => CustomerIdSearchResults[0].Displayed);
        }

        /**
         * Method to select a customer by id from search results.
         */
        public void SelectCustomer(string customerId)
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
        }

        /**
         * Method to search for and select a customer from search results.
         */
        public BasePage SearchAndSelectCustomer(string customerId)
        {
            SearchCustomer(customerId);
            SelectCustomer(customerId);
            return this;
        }
        
        /**
         * Method to click a page object by locator.
         */
        public BasePage ClickElement(By locator)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    ScrollToElement(locator);
                    IWebElement element = driver.FindElementBy(locator);
                    Wait.Until(d => element.Displayed);
                    element.Click();
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
                attempts++;
                Thread.Sleep(SleepTime);
            }
            Thread.Sleep(SleepTime / 5);
            return this;
        }

        /**
         * Method to scroll to a page object.
         */
        public void ScrollToElement(By locator)
        {
            IWebElement element = driver.FindElementBy(locator);
            Actions actions = new Actions(driver);
            actions.MoveToElement(element);
            actions.Perform();
        }

        /**
         * Method to input desired value into a field by locator.
         */
        public void SetFieldValue(By locator, string text)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    SetValue(driver.FindElementBy(locator), text);
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
                attempts++;
                Thread.Sleep(SleepTime);
            }
        }

        /**
         * Method to input desired value into a field by web element.
         */
        public BasePage SetFieldValue(IWebElement field, string text)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    Wait.Until(d => field.Displayed);
                    SetValue(field, text);
                    break;
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
         * Method to input desired value into a field.
         */
        public void SetValue(IWebElement field, string text)
        {
            int textLength = field.GetAttribute("value").Length;
            field.Clear();
            field.Click();
            for (int i = 0; i < textLength; i++)
            {
                field.SendKeys(Keys.ArrowRight);
                field.SendKeys(Keys.Backspace);
            }
            field.SendKeys(text);
        }

        /**
         * Method to get value from a text field.
         */
        public string GetFieldValue(IWebElement field)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => field.Displayed);
            return field.GetAttribute("value");
        }

        /**
         * Method to check if an element exists.
         */
        public Boolean ElementExists(By locator)
        {
            return driver.FindElements(locator).Count > 0;
        }

        /**
         * Method to switch to a particular window.
         */
        public void SwitchToWindow(String pageTitle)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            foreach (string handle in driver.WindowHandles)
            {
                driver.SwitchTo().Window(handle);
                if (driver.Title.ToLower().Contains(pageTitle.ToLower()))
                {
                    break;
                }
            }
        }

        /**
         * Method to wake the current window.
         */
        public void AwakeWindow()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            string currentWindow = driver.CurrentWindowHandle;
            foreach (string handle in driver.WindowHandles)
            {
                if (!handle.Contains(currentWindow))
                {
                    driver.SwitchTo().Window(handle);
                }
            }
            driver.SwitchTo().Window(currentWindow);
        }

        /**
         * Method to kill a particular window.
         */
        public void KillWindow(string pageTitle)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            foreach (string handle in driver.WindowHandles)
            {
                driver.SwitchTo().Window(handle);
                if (driver.Title.ToLower().Contains(pageTitle.ToLower()))
                {
                    //((JavascriptExecutor)driver).executeScript("window.close()")
                    driver.Close();
                    break;
                }
            }
        }

        public string GetFormattedDate(string date, int days, int hours)
        {
            string expectedDateFormat = "MM/dd/yyyy H:mm";
            DateTime dt;
            if (DateTime.TryParseExact(date, expectedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return date;
            }
            return DateTime.Today.AddDays(days).AddHours(hours).ToString(expectedDateFormat);
        }

        public IList<string> GetDaysOfWeek()
        {
            IList<string> daysOfWeek = new List<string>();
            daysOfWeek.Add("Monday");
            daysOfWeek.Add("Tuesday");
            daysOfWeek.Add("Wednesday");
            daysOfWeek.Add("Thursday");
            daysOfWeek.Add("Friday");
            daysOfWeek.Add("Saturday");
            daysOfWeek.Add("Sunday");
            return daysOfWeek;
        }

        public string GetDayOfWeek()
        {
            return DateTime.Now.ToString("dddd");
        }
    }
}
