using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using TlrgAutomation.Managers;

namespace TlrgAutomation.Pages.Tlrg
{
    class HolidayScheduleTable
    {
        IWebDriver driver;
        public HolidayScheduleTable()
        {
            driver = Fixtures.GetWebDriver();
        }

        /*
         * Ensures that the table is actually on the page before attempting to 
         * create the object
         */
        public HolidayScheduleTable WaitForHolidayTableToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            Wait.Until(d => ScheduleTable.Displayed);
            return this;
        }

        public IWebElement ScheduleTable => driver.FindElementBy(By.XPath("//div[contains(@id, 'contenttable')]"));
        public IList<HolidayScheduleRow> HolidayRows => BuildRows();

        /*
         * Builds the HolidayRows list from the rows in the Settings UI.
         */
        public IList<HolidayScheduleRow> BuildRows()
        {
            IList<IWebElement> rows = driver.FindElements(By.XPath("//div[contains(@id, 'row') and not(contains(@id, 'add'))]"));
            IList <HolidayScheduleRow> rowsToReturn = new List<HolidayScheduleRow>();
            HolidayScheduleRow holidayScheduleRow = new HolidayScheduleRow();
            foreach (IWebElement row in rows)
            { 
                IList<IWebElement> cells = row.FindElements(By.XPath("//div[@role='gridcell']"));
                if(cells[0 + (3 * rows.IndexOf(row))].GetAttribute("class").Contains("cleared") ||
                    cells[0 + (3 * rows.IndexOf(row))].GetAttribute("class").Contains("empty")) 
                {
                    break;
                }
                holidayScheduleRow.date = Convert.ToDateTime(cells[0 + (3 * rows.IndexOf(row))].GetAttribute("title"));
                holidayScheduleRow.label = cells[1 + (3 * rows.IndexOf(row))].GetAttribute("title");
                holidayScheduleRow.removeButton = cells[2 + (3 * rows.IndexOf(row))];
                rowsToReturn.Add(holidayScheduleRow);
            }
            return rowsToReturn;
        }

        /*
         * Retrieve a row from the Holiday Schedule settings page by the Holiday Name.
         */
        public HolidayScheduleRow GetRowByLabel(string label)
        {
            foreach(HolidayScheduleRow row in HolidayRows)
            {
                if(row.label == label)
                {
                    return row;
                }
            }
            throw new Exception("Row with label " + label + " does not exist in the holiday schedule table.");
        }

        /*
         * Verify that a given holiday exists in the Holiday Settings.
         */
        public Boolean HolidayExistsInTable(string label)
        {
            foreach (HolidayScheduleRow row in HolidayRows)
            {
                if (row.label == label)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
