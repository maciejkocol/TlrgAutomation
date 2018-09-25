using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TlrgAutomation.Managers
{
    public static class Fixtures
    {

        [ThreadStatic]
        public static IWebDriver driver;

        public static EnvironmentManager.Environment env;
        public static RequestManager requestManager = new RequestManager();

        public static void InitializeEnvironment()
        {
            env = EnvironmentManager.GetEnvironment(TestContext.Parameters["TLRGEnv"] ?? Properties.RunSettings.Default.Run_Environment);
        }

        public static EnvironmentManager.Environment GetEnvironment()
        {
            return env;
        }

        /*
         * Creates the web driver with chrome flags, this Fixtures class makes the 
         * WebDriver accessible to all classes even those which do not extend the base classes.
         */
        public static void InitializeWebDriver()
        {
            InitializeEnvironment();
            ChromeOptions options = new ChromeOptions();
            if (env.Headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("window-size=1980,1080");
                //options.AddArgument("--disable-gpu");
                options.AddArgument("--disable-extensions");
            }
            driver = new ChromeDriver(options);
            
        }

        public static IWebDriver GetWebDriver()
        {
            return driver;
        }

        public static RequestManager GetRequestManager()
        {
            return requestManager;
        }


    }
}
