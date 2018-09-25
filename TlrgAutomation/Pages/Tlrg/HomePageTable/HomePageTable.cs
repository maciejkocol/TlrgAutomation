using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TlrgAutomation.Managers;

namespace TlrgAutomation.Pages.Tlrg
{
    class HomePageTable
    {
        IWebDriver driver;

        public HomePageTable()
        {
            driver = Fixtures.GetWebDriver();
        }

        public IWebElement HomeContentTable => driver.FindElementBy(By.XPath("//div[contains(@id, 'contenttable')]"));
        public IList<HomePageRow> HomePageRows => BuildRows();

        /*
         * Builds out the rows of the Home Page table from the UI into a List of row objects.
         */
        public IList<HomePageRow> BuildRows()
        {
            List<string> columns = new List<string>(new string[] { "LoadId", "Carrier", "CarrierRank",
            "Cogs", "Status", "OriginCity", "OriginState", "DestinationCity", "DestinationState", "PUDate"});
            List<string> xpath_params = new List<string>(new string[] { "loadId", "carrier", "currentCarrierRank", "coGs",
                "status", "startOriginCity", "startOriginState", "endDestinationCity", "endDestinationState", "pickUpDate" });

            IList<IWebElement> rows = driver.FindElements(By.XPath("//app-load-details-grid//mat-table//mat-row[@role='row']"));
            IList<HomePageRow> rowsToReturn = new List<HomePageRow>();
            HomePageRow homePageRow;
            foreach (IWebElement row in rows)
            {
                homePageRow = new HomePageRow();
                foreach (string column in columns)
                {
                    string columnValue = row.FindElements(
                        By.XPath("//app-load-details-grid//mat-table//mat-row//mat-cell[@role='gridcell' and starts-with(@id,'" + xpath_params[columns.IndexOf(column)] + "')]"))[rows.IndexOf(row)].GetAttribute("title");
                    SetRowField(homePageRow, column, columnValue);
                    //if (columns.IndexOf(column) / (columns.Count - 1) == 1)
                    //{
                    //    break;
                    //}
                }
                rowsToReturn.Add(homePageRow);
            }
            return rowsToReturn;

        }

        private void SetRowField(HomePageRow row, string property, string value)
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

        public HomePageRow GetRowByLoadId(string loadId)
        {
            foreach (HomePageRow row in HomePageRows)
            {
                if (row.LoadId == loadId)
                {
                    return row;
                }
            }
            throw new Exception("Row with label " + loadId + " does not exist in the home page table.");
        }
    }
}
