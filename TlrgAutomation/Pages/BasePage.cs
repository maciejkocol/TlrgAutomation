using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Data.SqlClient;
using System.Data;
using NUnit.Framework;
using Tlrg.Pages.Optimizer;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium.Interactions;

namespace Tlrg.Pages
{
    public class BasePage
    {
        public IWebDriver driver;
        public EnvironmentManager.Environment env;
        private static string DBConnectionString;

        public BasePage(IWebDriver driver)
        {
            this.driver = driver;
            SeleniumExtras.PageObjects.PageFactory.InitElements(driver, this);
            env = EnvironmentManager.GetEnvironment(TestContext.Parameters["TLRGEnv"] ?? "DEV2");
            DBConnectionString = "Data Source=" + env.DBPath + ";" +
            "Initial Catalog=XPlatform;" +
            "Integrated Security=True";
        }

        // default sleep time in milliseconds
        public const int SleepTime = 2000;

        public static By EwsFrameOptimizer = By.Id("ewsFrame"); // ews frame in Optimizer
        public static By MenuFrameOptimizer = By.Id("topMenuFrame"); // menu frame in Optimizer
        public static By WarehouseOptimizer = By.Id("WarehouseMaintenanceIframe"); // warehouse maintenance frame in Optimizer

        /**
         * Method to retreive data from the DB.
         */
        public static DataTable GetData(string query)
        {
            DataTable dataTable = new DataTable();
            // establish connection
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    // create data adapter
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // queries the database and returns the result to the datatable
                        da.Fill(dataTable);
                        conn.Close();
                        da.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dataTable;
        }

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
            LoginPage optimizerLoginPageElements = new LoginPage(Driver);
            if (optimizerLoginPageElements.PageDisplayed())
            {
                optimizerLoginPageElements.Login(user, pass);
            }
        }

        /**
         * Method to click a page object by locator.
         */
        public void ClickElement(By locator)
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
        public void SetFieldValue(IWebElement field, string text)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
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
    }
}
