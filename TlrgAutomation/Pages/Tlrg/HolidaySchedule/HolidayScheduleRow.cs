using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlrgAutomation.Pages.Tlrg
{
    class HolidayScheduleRow
    {
        public DateTime date { get; set; }
        public string label { get; set; }
        public IWebElement removeButton { get; set; }

        public HolidayScheduleRow()
        {
        }


    }
}
