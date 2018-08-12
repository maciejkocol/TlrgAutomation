using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Tlrg.Tests
{
    public class BaseTest
    {
        [ThreadStatic]
        protected static IWebDriver driver;

        static protected EnvironmentManager.Environment env;
        
        private static readonly List<string> _processesToCheck =
        new List<string>
        {
            "opera",
            "chrome",
            "firefox",
            "ie",
            "gecko",
            "phantomjs",
            "edge",
            "microsoftwebdriver",
            "webdriver"
        };
        public static DateTime? TestRunStartTime { get; set; }
        public static void FinishHim()
        {
            //driver?.Dispose();
            var processes = Process.GetProcesses();
            foreach (var process in processes)
            {
                try
                {
                    Debug.WriteLine(process.ProcessName);
                    if (process.StartTime > TestRunStartTime)
                    {
                        var shouldKill = false;
                        foreach (var processName in _processesToCheck)
                        {
                            if (process.ProcessName.ToLower().Contains(processName))
                            {
                                shouldKill = true;
                                break;
                            }
                        }
                        if (shouldKill)
                        {
                            process.Kill();
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            CleanBrowserInstance();
            env = EnvironmentManager.GetEnvironment(TestContext.Parameters["TLRGEnv"] ?? "DEV2");
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            CleanBrowserInstance();
            FinishHim();
        }

        [SetUp]
        protected virtual void Setup()
        {
            TestRunStartTime = DateTime.Now;
            ChromeOptions options = new ChromeOptions();
            if (env.Headless)
            {
                options.AddArgument("--headless");
                options.AddArgument("window-size=1980,1080");
                options.AddArgument("--disable-gpu");
                options.AddArgument("--disable-extensions");
            }
            driver = new ChromeDriver(options);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

        public static void CleanBrowserInstance()
        {
            foreach (Process P in Process.GetProcessesByName("chromedriver"))
                P.Kill();
            foreach (Process P in Process.GetProcessesByName("IEDriverServer"))
                P.Kill();
        }
    }
}
