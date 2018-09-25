using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.IO;
using System.Threading;
using System.Collections.Generic;

namespace TlrgAutomation.Pages.Tlrg
{
    public class PricingUploadPage : BasePage
    {
        public PricingUploadPage() : base()
        {
            driver.Navigate().GoToUrl(env.TlrgURL + "/pricing-upload");
            WaitForPricingUploadPageToLoad();
        }

        // grids
        public IWebElement RateUploadGrid => driver.FindElementBy(By.Id("pricing-upload-rate"));
        public IWebElement FuelUploadGrid => driver.FindElementBy(By.Id("pricing-upload-fuel"));
        public IWebElement ServicesUploadGrid => driver.FindElementBy(By.Id("pricing-upload-services"));
        public IWebElement ValidationUploadGrid => driver.FindElementBy(By.Id("pricing-upload-validation"));

        // dropdowns
        public IWebElement TemplateDropdown => driver.FindElementBy(By.Id("pricing-template-dropdown"));

        // buttons
        public IWebElement RateUploadButton => driver.FindElementBy(By.XPath("//button[@class='upload-button' and @for='rateFile']"));
        public IWebElement FuelUploadButton => driver.FindElementBy(By.XPath("//button[@class='upload-button' and @for='fuelFile']"));
        public IWebElement UseEchoFuelTableButton => driver.FindElementBy(By.XPath("//button[contains(@class, 'use-echo-fuel-table')]"));
        public IWebElement ClearSpecialServicesButton => driver.FindElementBy(By.XPath("//button[contains(@class, 'clear-services-button')]"));
        public IWebElement ServicesUploadButton => driver.FindElementBy(By.XPath("//button[@class='upload-button' and @for='ssFile']"));
        public IWebElement EffectiveFromDateButton => driver.FindElementBy(By.XPath("(//div[@class='form-group row']//div[@class='jqx-icon jqx-icon-calendar'])[1]"));
        public IWebElement EffectiveToDateButton => driver.FindElementBy(By.XPath("(//div[@class='form-group row']//div[@class='jqx-icon jqx-icon-calendar'])[2]"));
        public IWebElement CollidingRatesConfirmButton => driver.FindElementBy(By.XPath("//button[text()='Confirm']"));
        public IWebElement ConfirmUploadButton => driver.FindElementBy(By.XPath("//button[text()='Confirm']"));
        public IWebElement RateWizardUploadButton => driver.FindElementBy(By.XPath("//button[starts-with(@class,'rateschedule-wizard-button') and text()='Upload']"));
        public IList<IWebElement> ArrowButtons => driver.FindElementsBy(By.XPath("//i[starts-with(@class,'fa fa-arrow-circle-o-')]"));
        public IWebElement LeftArrowButton => ArrowButtons[0];
        public IWebElement RightArrowButton => ArrowButtons[1];

        // links
        public By ExistingRateScheduleWarning = By.XPath("//i[@class='fa fa-info-circle']");

        // cells
        public IWebElement DefaultSelectedFromDateCell => driver.FindElementBy(By.XPath("(//td[@role='gridcell' and @aria-selected='true'])[1]"));
        public IWebElement DefaultSelectedToDateCell => driver.FindElementBy(By.XPath("(//td[@role='gridcell' and @aria-selected='true'])[2]"));

        // fields
        public IWebElement EffectiveFromDateField => driver.FindElementBy(By.XPath("(//div[@class='form-group row']//input[starts-with(@id,'inputjqxWidget')])[1]"));
        public IWebElement EffectiveToDateField => driver.FindElementBy(By.XPath("(//div[@class='form-group row']//input[starts-with(@id,'inputjqxWidget')])[2]"));
        public IWebElement ScheduleNameField => driver.FindElementBy(By.Name("ScheduleName"));
        public IWebElement ViewStatusField => driver.FindElementBy(By.Name("viewStatus"));
        public IWebElement LineHaulSchedulesField => driver.FindElementBy(By.Name("lineHaulSchedules"));
        public IList<IWebElement> LineHaulSchedulesFieldOptions => LineHaulSchedulesField.FindElementsBy(By.TagName("option"));

        // text
        public IWebElement LineHaulLastUpdated => driver.FindElementBy(By.XPath("//div[@id='pricing-upload-rate']//div[contains(@class, 'upload-date')]"));
        public IWebElement FuelLastUpdated => driver.FindElementBy(By.XPath("//div[@id='pricing-upload-fuel']//div[contains(@class, 'upload-date')]"));
        public IWebElement ServicesLastUpdated => driver.FindElementBy(By.XPath("//div[@id='pricing-upload-services']//div[contains(@class, 'upload-date')]"));
        public IWebElement UsingEchoFuelTableMessage => driver.FindElementBy(By.XPath("//div[@class='upload-none-exists ng-star-inserted' and contains(text(), 'fuel table')]"));
        public IWebElement NoSpecialServicesText => driver.FindElementBy(By.XPath("//div[@class='upload-none-exists ng-star-inserted' and contains(text(), 'No special services available')]"));
        public IWebElement UploadSuccessfulMessage => driver.FindElementBy(By.XPath("//div[@class='successful-upload-text' and text()='Upload successful.']"));
        
        // files
        public IWebElement RateUploadFile => driver.FindElementBy(By.Id("rateFile"));
        public IWebElement FuelUploadFile => driver.FindElementBy(By.Id("fuelFile"));
        public IWebElement SpecialServicesFile => driver.FindElementBy(By.Id("ssFile"));

        // values
        public string ViewStatusValue = "Production";

        //LoadingIcon
        public IWebElement Loader => driver.FindElementBy(By.XPath("//div[@class='loader']"));

        /**
         * Method to wait for the navigation objects to load on the Pricing Upload page.
         */
        public PricingUploadPage WaitForPricingUploadPageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => RateUploadGrid.Displayed);
            Wait.Until(d => FuelUploadGrid.Displayed);
            Wait.Until(d => ServicesUploadGrid.Displayed);
            Wait.Until(d => ValidationUploadGrid.Displayed);
            return this;
        }

        /**
         * Method to upload line haul rates for a specific customer and carrier.
         */
        public PricingUploadPage UploadLineHaulRates(string customerId, string carrierId, string scheduleName)
        {
            UploadRates(customerId, carrierId, scheduleName, false);
            return this;
        }

        /**
         * Method to upload multi line haul rates for a specific customer and carrier.
         */
        public PricingUploadPage UploadLineHaulMultiRates(string customerId, string carrierId, string scheduleName)
        {
            UploadRates(customerId, carrierId, scheduleName, true);
            return this;
        }

        /**
         * Method to upload rates for a specific customer and carrier.
         */
        public void UploadRates(string customerId, string carrierId, string scheduleName, bool isMultiStop)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(60));
            string rateUploadFilePath;
            SearchAndSelectCustomer(customerId);
            SearchAndSelectCarrier(carrierId);
            if (isMultiStop)
            {
                rateUploadFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\..\\Resources\\RateUploadMultiStop.csv";
            }
            else
            {
                rateUploadFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\..\\Resources\\RateUpload.csv";
            }
            RateUploadFile.SendKeys(Path.GetFullPath(rateUploadFilePath));
            SelectEffectiveDates();
            Wait.Until(d => RightArrowButton.Displayed);
            RightArrowButton.Click();
            FillNameAndStatus(scheduleName);
            Wait.Until(d => RateWizardUploadButton.Displayed);
            RateWizardUploadButton.Click();
            Wait.Until(d => LineHaulSchedulesField.Text.Contains(scheduleName));
        }

        /**
         * Method to enter name and select view status during rate upload.
         */
        public PricingUploadPage FillNameAndStatus(string scheduleName)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => ScheduleNameField.Displayed);
            SetFieldValue(ScheduleNameField, scheduleName);
            ViewStatusField.SendKeys(ViewStatusValue);
            return this;
        }

        /**
         * Method to select effective from and to dates during rate upload.
         */
        public PricingUploadPage SelectEffectiveDates()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            SelectEffectiveFromDate();
            SelectEffectiveToDate();
            Thread.Sleep(SleepTime/2);
            Wait.Until(d => ArrowButtons.Count > 0);
            if (ArrowButtons.Count < 2)
            {
                IWebElement scheduleWarning = driver.FindElementBy(ExistingRateScheduleWarning);
                scheduleWarning.Click();
                CollidingRatesConfirmButton.Click();
            }
            return this;
        }

        /**
         * Method to select effective from date from date picker.
         */
        public PricingUploadPage SelectEffectiveFromDate()
        {
            EffectiveFromDateField.SendKeys(DateTime.Now.ToString("yyyy-MM-dd"));
            return this;
        }

        /**
         * Method to select effective to date from date picker.
         */
        public PricingUploadPage SelectEffectiveToDate()
        {
            EffectiveToDateField.SendKeys(DateTime.Now.AddMonths(1).ToString("yyyy-MM-dd"));
            return this;
        }

        /**
         * Method to upload fuel schedule for a specific customer and carrier.
         */
        public PricingUploadPage UploadFuelSchedule(string customerId, string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            SearchAndSelectCustomer(customerId);
            SearchAndSelectCarrier(carrierId);
            string fuelUploadFilePath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\..\\Resources\\FuelUpload.csv";
            FuelUploadFile.SendKeys(Path.GetFullPath(fuelUploadFilePath));
            Wait.Until(d => ConfirmUploadButton.Displayed);
            ConfirmUploadButton.Click();
            Wait.Until(d => UploadSuccessfulMessage.Displayed);
            return this;
        }

        /**
         * Method to upload special services for a specific customer and carrier.
         */
        public PricingUploadPage UploadSpecialServices(string customerId, string carrierId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            SearchAndSelectCustomer(customerId);
            SearchAndSelectCarrier(carrierId);
            string sServicesUploadPath = AppDomain.CurrentDomain.BaseDirectory.ToString() + "..\\..\\Resources\\ServicesUpload.csv";
            SpecialServicesFile.SendKeys(Path.GetFullPath(sServicesUploadPath));
            Wait.Until(d => ConfirmUploadButton.Displayed);
            ConfirmUploadButton.Click();
            Wait.Until(d => UploadSuccessfulMessage.Displayed);
            return this;
        }
    }
}
