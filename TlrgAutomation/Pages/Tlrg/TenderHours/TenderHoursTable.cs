using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using TlrgAutomation.Managers;

namespace TlrgAutomation.Pages.Tlrg
{
    class TenderHoursTable: BasePage
    {
        IWebDriver driver;

        public TenderHoursTable()
        {
            driver = Fixtures.GetWebDriver();
        }

        public IWebElement TenderHourContainer => driver.FindElementBy(By.XPath("//div[@class='tender-hour-container']"));
        public IList<IWebElement> TenderHourGroupRows => TenderHourContainer.FindElements(By.XPath("//div[starts-with(@class,'tender-hour-group')]"));
        public IList<TenderHoursRow> TenderHoursRows => BuildRows();

        public By tenderHoursLabel = By.XPath("label[@class='tender-hour-label']");
        public By tenderHoursStart = By.XPath("jqxdatetimeinput[contains(@name,'Start')]/div[starts-with(@class,'ms-grid-start-hour')]");
        public By tenderHoursEnd = By.XPath("jqxdatetimeinput[contains(@name,'End')]/div[starts-with(@class,'ms-grid-end-hour')]");
        public By tenderHoursInput = By.XPath("div/input[starts-with(@id,'inputjqxWidget')]");
        public By tenderHourCheckbox = By.XPath("div[@class='tender-hours-check']//span[@class='fa-stack']");
        public By tenderHourCheck = By.XPath("i[starts-with(@class,'fa fa-check')]");

        public IList<TenderHoursRow> BuildRows()
        {
            IList<TenderHoursRow> rowsToReturn = new List<TenderHoursRow>();
            TenderHoursRow tenderHoursRow;
            foreach (IWebElement row in TenderHourGroupRows)
            {
                tenderHoursRow = new TenderHoursRow();
                IWebElement dayOfWeekElement = row.FindElement(tenderHoursLabel);
                IWebElement startTimeElement = row.FindElement(tenderHoursStart);
                IWebElement endTimeElement = row.FindElement(tenderHoursEnd);
                SetRowField(tenderHoursRow, "DayOfWeek", dayOfWeekElement.Text);
                SetRowField(tenderHoursRow, "StartTime", startTimeElement.GetAttribute("value"));
                SetRowField(tenderHoursRow, "EndTime", endTimeElement.GetAttribute("value"));
                SetRowField(tenderHoursRow, "IsOpen", IsOpen(row).ToString());
                rowsToReturn.Add(tenderHoursRow);
            }
            return rowsToReturn;
        }

        public TenderHoursTable SetTenderHours(string dayOfWeek, string startTime, string endTime)
        {
            SetOpen(dayOfWeek);
            foreach (IWebElement row in TenderHourGroupRows)
            {
                IWebElement dayOfWeekElement = row.FindElement(tenderHoursLabel);
                if (dayOfWeekElement.Text.Contains(dayOfWeek))
                {
                    IWebElement startTimeElement = row.FindElement(tenderHoursStart).FindElement(tenderHoursInput);
                    IWebElement endTimeElement = row.FindElement(tenderHoursEnd).FindElement(tenderHoursInput);
                    startTimeElement.Click();
                    Thread.Sleep(SleepTime / 6);
                    startTimeElement.SendKeys(Keys.ArrowLeft);
                    Thread.Sleep(SleepTime / 6);
                    startTimeElement.SendKeys(startTime);
                    endTimeElement.Click();
                    Thread.Sleep(SleepTime / 6);
                    endTimeElement.SendKeys(Keys.ArrowLeft);
                    Thread.Sleep(SleepTime / 6);
                    endTimeElement.SendKeys(endTime);
                }
            }
            return this;
        }

        public TenderHoursTable SetOpen(string dayOfWeek)
        {
            SetOpenClose(dayOfWeek, true);
            return this;
        }

        public TenderHoursTable SetClosed(string dayOfWeek)
        {
            SetOpenClose(dayOfWeek, false);
            return this;
        }

        private void SetOpenClose(string dayOfWeek, bool open)
        {
            foreach (IWebElement row in TenderHourGroupRows)
            {
                IWebElement dayOfWeekElement = row.FindElement(tenderHoursLabel);
                if (dayOfWeekElement.Text.Contains(dayOfWeek))
                {
                    IWebElement tenderHourCheckboxElement = row.FindElement(tenderHourCheckbox);
                    if (IsOpen(row))
                    {
                        if (!open)
                        {
                            tenderHourCheckboxElement.Click();
                        }
                    }
                    else
                    {
                        if (open)
                        {
                            tenderHourCheckboxElement.Click();
                        }
                    }
                }
            }
        }

        private bool IsOpen(IWebElement row)
        {
            return !row.FindElement(tenderHourCheckbox).FindElement(tenderHourCheck).GetAttribute("class").Contains("hidden");
        }

        private void SetRowField(TenderHoursRow row, string property, string value)
        {
            Type type = row.GetType();
            foreach (PropertyInfo info in type.GetProperties())
            {
                if (info.Name == property && info.CanWrite)
                {
                    info.SetValue(row, value, null);
                    break;
                }
            }
        }

        public TenderHoursRow GetRowByDayOfWeek(string dayOfWeek)
        {
            foreach (TenderHoursRow row in TenderHoursRows)
            {
                if (row.DayOfWeek == dayOfWeek)
                {
                    return row;
                }
            }
            throw new Exception("Row with label " + dayOfWeek + " does not exist in the tender hours table.");
        }
    }
}
