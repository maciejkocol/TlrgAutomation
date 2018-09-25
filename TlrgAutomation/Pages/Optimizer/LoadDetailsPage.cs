using System;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TlrgAutomation.Pages.Optimizer
{
    public class LoadDetailsPage : BasePage
    {
        public LoadDetailsPage() : base()
        {
            WaitForLoadDetailsPageToLoad();
        }

        // names
        public string OptimizerLookupPageTitle = "EchoGlobal Logistics - Echo Optimizer : Lookup";

        // labels and text
        public By LoadDetails = By.Id("lbltabLoadId");
        public By LoadSummary = By.XPath("//td[@class='blueCell' and contains(text(),'Load Summary')]");
        public By LoadWarning = By.XPath("//div[@id='read-only-div']");

        // buttons
        public By StartTlrgButton = By.Id("btnStartTLRG");
        
        /**
         * Method to wait for the main objects to load on the LoadDetails page.
         */
        public void WaitForLoadDetailsPageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            IWebElement loadSummaryElement = driver.FindElement(LoadSummary);
            Wait.Until(d => loadSummaryElement.Displayed);
        }

        /**
         * Method to validate whether load was displayed successfully.
         */
        public Boolean LoadDisplayed()
        {
            int attempts = 0;
            while (attempts < 30)
            {
                try
                {
                    if (driver.FindElementBy(LoadDetails).Displayed)
                    {
                        return true;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
                attempts++;
                Thread.Sleep(SleepTime);
            }
            return false;
        }

        /**
         * Method to get load Id displayed after it is created.
         */
        public string GetLoadIdDisplayed()
        {
            return driver.FindElementBy(LoadDetails).Text;
        }
    }
}
