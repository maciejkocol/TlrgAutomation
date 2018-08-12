using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.ObjectModel;

namespace Tlrg
{
    public static class WebDriverHelper
    {
        public static ReadOnlyCollection<IWebElement> FindElementsBy(this IWebDriver driver, By by, int timeout = 10)
        {
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(timeout),
                PollingInterval = TimeSpan.FromMilliseconds(300)
            };
            try
            {
                wait.Until(d => d.FindElements(by).Count > 0);
                return driver.FindElements(by);
            }
            catch (Exception)
            {
                throw new NoSuchElementException("Unable to find element, locator: \"" + by.ToString() + "\".");
            }
        }

        public static ReadOnlyCollection<IWebElement> FindElementsBy(this IWebElement element, By by, int timeout = 10)
        {
            IWait<IWebElement> wait = new DefaultWait<IWebElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(timeout),
                PollingInterval = TimeSpan.FromMilliseconds(300)
            };
            try
            {
                wait.Until(e => e.FindElements(by).Count > 0);
                return element.FindElements(by);
            }
            catch (Exception)
            {
                throw new NoSuchElementException("Unable to find element, locator: \"" + by.ToString() + "\".");
            }
        }

        public static IWebElement FindElementBy(this IWebDriver driver, By by, int timeout = 10)
        {
            IWait<IWebDriver> wait = new DefaultWait<IWebDriver>(driver)
            {
                Timeout = TimeSpan.FromSeconds(timeout),
                PollingInterval = TimeSpan.FromMilliseconds(300)
            };
            try
            {
                wait.Until(d => d.FindElements(by).Count > 0);
                return driver.FindElement(by);
            }
            catch (Exception)
            {
                throw new NoSuchElementException("Unable to find element, locator: \"" + by.ToString() + "\".");
            }
        }

        public static IWebElement FindElementBy(this IWebElement element, By by, int timeout = 10)
        {
            IWait<IWebElement> wait = new DefaultWait<IWebElement>(element)
            {
                Timeout = TimeSpan.FromSeconds(timeout),
                PollingInterval = TimeSpan.FromMilliseconds(300)
            };
            try
            {
                wait.Until(e => e.FindElements(by).Count > 0);
                return element.FindElement(by);
            }
            catch (Exception)
            {
                throw new NoSuchElementException("Unable to find element, locator: \"" + by.ToString() + "\".");
            }
        }
    }
}
