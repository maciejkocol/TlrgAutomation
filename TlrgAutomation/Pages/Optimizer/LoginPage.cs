using System;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TlrgAutomation.Pages.Optimizer
{
    public class LoginPage : BasePage
    {
        private const string PageTitle = "Echo Global Logistics - Echo Optimizer : Login";

        public LoginPage() : base()
        {
            WaitForLoginPageToLoad();
        }

        // fields
        public IWebElement UsernameField => driver.FindElementBy(By.Id("ftbUserName"));
        public IWebElement PasswordField => driver.FindElementBy(By.Id("ftbPassword"));

        // buttons
        public IWebElement GoButton => driver.FindElementBy(By.Id("btnSubmit"));

        /**
         * Method to wait for the main objects to load on the Login page.
         */
        public void WaitForLoginPageToLoad()
        {
            if (PageDisplayed())
            {
                WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                Wait.Until(d => UsernameField.Displayed);
                Wait.Until(d => PasswordField.Displayed);
                Wait.Until(d => GoButton.Displayed);
            }
        }

        /**
         * Method to check if current page is login.
         */
        public Boolean PageDisplayed()
        {
            return (driver.Title.Equals(PageTitle));
        }

        /**
         * Method to click the Go button and initiate login.
         */
        public void ClickGoButton()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => GoButton.Displayed);
            GoButton.Click();
        }

        /**
         * Method to log into Optimizer with a username and password.
         */
        public void Login(string user, string pass)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            UsernameField.Clear();
            UsernameField.SendKeys(user);
            PasswordField.Clear();
            PasswordField.SendKeys(pass);
            ClickGoButton();
            Thread.Sleep(SleepTime);
        }
    }
}
