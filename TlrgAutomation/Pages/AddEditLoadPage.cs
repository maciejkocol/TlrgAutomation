using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Tlrg.Pages.Optimizer
{
    public class AddEditLoadPage: BasePage
    {
        public AddEditLoadPage(IWebDriver driver) : base(driver)
        {
            driver.Navigate().GoToUrl(env.OptiURL + "/AddEditLoad.aspx");
            AttemptOptimizerLogin(driver, env.OptiUser, env.OptiPassword);
            SwitchToWindow(OptimizerLookupPageTitle);
            WaitForAddEditLoadPageToLoad();
        }

        // names
        public string OptimizerLookupPageTitle = "EchoGlobal Logistics - Echo Optimizer : Lookup";
        public string ShipmentBuilderWarehouseNameValue = "Test";
        public string NoContacts = "No Contacts";
        public string ShipmentBuilderWarehouseAddContactPhoneValue = "5555555555";
        public string ShipmentBuilderWarehouseAddContactFirstNameValue = "Tester";
        
        // fields
        public By AccountNameField = By.Id("ftbSearchName");
        public By AccountIdField = By.Id("ftbSearchId");
        public By ExpectedRevenueField = By.Id("ctbExpectedRevenue");
        public By ExpectedCostField = By.Id("ctbExpectedCost");
        public By ShipmentBuilderWarehouseSearchAutocompleteField = By.Id("warehouseAutocomplete");
        public By ShipmentBuilderWarehouseManagementAddressSearchAutocompleteField = By.Id("WarehouseAddressSearchInput");
        public By ShipmentBuilderWarehouseManagementNameSearchAutocompleteField = By.Id("SelectedWarehouse_DisplayText");
        public By ShipmentBuilderWarehouseNameField = By.Id("CurrentWarehouseAccount_Name");
        public By ShipmentBuilderWarehouseAddContactPhoneField = By.Id("ContactInput_WorkPhone");
        public By ShipmentBuilderWarehouseAddContactFirstNameField = By.Id("ContactInput_GivenName");
        public By ShipmentBuilderAppointmentStartField = By.Id("AppointmentStart");
        public By ShipmentBuilderAppointmentStartTimeField = By.Id("AppointmentStartTime");
        public By ShipmentBuilderAppointmentEndField = By.Id("AppointmentEnd");
        public By ShipmentBuilderAppointmentEndTimeField = By.Id("AppointmentEndTime");
        public By ShipmentBuilderTotalMilesField = By.Id("totalMiles");
        public By ShipmentBuilderProductDescriptionField = By.Id("ProductDescription");
        public By ShipmentBuilderMinWeightField = By.Id("EstMinWeight");
        public By ShipmentBuilderMaxWeightField = By.Id("EstMaxWeight");
        public By ShipmentBuilderMaxBuyField = By.Id("MaxBuy");
        public By ShipmentBuilderSpecialInstructionsIntField = By.Id("spInstrInt");
        public By ShipmentBuilderSpecialInstructionsExtField = By.Id("spInstrExt");
        
        // labels and text
        public By ShipmentBuilderSavedLabel = By.XPath("//label[text()='Shipment Saved']");
        public By ShipmentBuilderSavedConfirmation = By.XPath("//b[contains(.,'Shipment') and contains(.,'has been created')]");
        public By ShipmentBuilderWarehouseFound = By.XPath("//div[@id='warehouse-found-display' and @style='']");
        public By ShipmentBuilderHasContacts = By.XPath("(//p[text()='No Contacts']) | (//button[text()='Remove contact'])");
        
        // buttons
        public By SearchAccountButton = By.Id("btnSearch");
        public By ShipmentBuilderSaveButton = By.Id("btnShipmentBuilder");
        public By ShipmentBuilderAccountButton = By.Id("account");
        public By ShipmentBuilderStopsButton = By.Id("stops");
        public By ShipmentBuilderShipmentDatesButton = By.Id("shipmentDates");
        public By ShipmentBuilderEquipmentButton = By.Id("equipment");
        public By ShipmentBuilderItemsButton = By.Id("items");
        public By ShipmentBuilderMoneyButton = By.Id("money");
        public By ShipmentBuilderReferenceNumbersButton = By.Id("referenceNumbers");
        public By ShipmentBuilderNotesButton = By.Id("notes");
        public By ShipmentBuilderPrimaryContactButton = By.Id("primarycontact");
        public By ShipmentBuilderShipmentAssignmentButton = By.Id("reserveshipment");
        public By ShipmentBuilderWarehouseSearchButton = By.Id("warehouseSearch");
        public By ShipmentBuilderUseWarehouseButton = By.Id("saveBtn");
        public By ShipmentBuilderAddContactButton = By.XPath("//button[text()='Add contact']");
        public By ShipmentBuilderUpdateCargoValueButton = By.Id("updateCargoValueBtn");
        public By ShipmentBuilderUpdateTenderTypeButton = By.Id("updateTenderTypeValueBtn");
        public By ShipmentBuilderAddPickButton = By.Id("addpick");
        public By ShipmentBuilderAddDropButton = By.Id("adddrop");
        public By ShipmentBuilderParentSubmitButton = By.XPath("//div[contains(@class,'shipmentBuilderChildren') and not(contains(@class,'ManagementChildren'))]//input[@id='submit']");
        public By ShipmentBuilderChildSubmitButton = By.XPath("//div[contains(@class,'shipmentBuilderChildren') and contains(@class,'ManagementChildren')]//input[@id='submit']");
        public By ShipmentBuilderAddContactSubmitButton = By.XPath("//form[@id='contact-form']//button[text()='Submit']");
        public By ShipmentBuilderEquipmentVanButton = By.Id("EquipmentId1");
        public By ShipmentBuilderEquipmentFlatbedButton = By.Id("EquipmentId25");
        public By ShipmentBuilderEquipmentStepdeckButton = By.Id("EquipmentId27");
        public By ShipmentBuilderEquipmentReeferButton = By.Id("EquipmentId42");
        public By ShipmentBuilderEquipmentSubmitButton = By.XPath("//button/span[text()='Submit']");
        public By ShipmentBuilderAddItemButton = By.Id("additem");
        public By ShipmentBuilderCarrierSalesYesButton = By.Id("btnCarrierSalesYes");
        public By ShipmentBuilderCarrierSalesNoButton = By.Id("btnCarrierSalesNo");
        public By ShipmentBuilderSubmitButton = By.XPath("//div[@id='shipment']//input[@id='submit']");
        public By ShipmentBuilderViewInOptimizerButton = By.Id("btnViewInOptimizer");
        public By ShipmentBuilderActiveDatePickerDay = By.XPath("//td[contains(@class,'datepicker-days-cell-over')]//a[contains(@class,'ui-state-active')]");
        
        // dropdowns
        public By ModeDropdown = By.Id("ucPickMode_ddlMode");
        public By TenderTypeDropdown = By.Id("updateTenderTypeSelectId");

        // dropdown options
        public By ModeDropdownOptions = By.XPath("//option[@value and text()]");
        public By TenderTypeDropdownOptions = By.XPath("//option[@value and text()]");

        // autocomplete options
        public By WarehouseAutocompleteOptions = By.XPath("//div[@id='warehouse-search-display']//span[@class='tt-dropdown-menu']//span[@class='tt-suggestions']/div[@class='tt-suggestion']/p[text()]");

        // search results
        public By AccountSearchResults = By.XPath("//tr[contains(@class,'Style')][@onmouseout]/td/a[contains(@id,'AccountName')][text()]");

        /**
         * Method to wait for the main objects to load on the AddEditLoad page.
         */
        public void WaitForAddEditLoadPageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement accountNameFieldElement = driver.FindElementBy(AccountNameField);
            IWebElement accountIdFieldElement = driver.FindElementBy(AccountIdField);
            Wait.Until(d => accountNameFieldElement.Displayed);
            Wait.Until(d => accountIdFieldElement.Displayed);
        }

        /**
         * Method to click the Search account button.
         */
        public void ClickSearchAccountButton()
        {
            ClickElement(SearchAccountButton);
        }

        /**
         * Method to save created load. 
         */
        public void SaveLoad()
        {
            ClickElement(ShipmentBuilderSaveButton);
        }

        /**
         * Method to update the cargo value of a load.
         */
        public void UpdateCargoValue(string value)
        {
            ClickElement(ShipmentBuilderAccountButton);
            ClickElement(ShipmentBuilderUpdateCargoValueButton);
            EnterIntoAlert(value);
            AcceptAlert();
        }

        /**
         * Method to update the tender type of a load.
         */
        public void UpdateTenderType(string type)
        {
            ClickElement(ShipmentBuilderUpdateTenderTypeButton);
            SelectItemFromDropdown(type, TenderTypeDropdown, TenderTypeDropdownOptions);
        }

        /**
         * Method to add pick-up stop for a load.
         */
        public void AddPickStop(string warehouseG, string pickStart, string pickEnd)
        {
            ClickElement(ShipmentBuilderStopsButton);
            ClickElement(ShipmentBuilderAddPickButton);
            SearchAndSelectWarehouse(warehouseG);
            SetFieldValue(ShipmentBuilderAppointmentStartField, pickStart.Split(' ')[0]);
            SelectActiveDayFromDatePicker();
            SetFieldValue(ShipmentBuilderAppointmentStartTimeField, pickStart.Split(' ')[1]);
            SetFieldValue(ShipmentBuilderAppointmentEndField, pickEnd.Split(' ')[0]);
            SelectActiveDayFromDatePicker();
            SetFieldValue(ShipmentBuilderAppointmentEndTimeField, pickEnd.Split(' ')[1]);
            ClickElement(ShipmentBuilderChildSubmitButton);
        }

        /**
         * Method to add drop-off stop for a load.
         */
        public void AddDropStop(string warehouseG, string dropStart, string dropEnd)
        {
            ClickElement(ShipmentBuilderAddDropButton);
            SearchAndSelectWarehouse(warehouseG);
            SetFieldValue(ShipmentBuilderAppointmentStartField, dropStart.Split(' ')[0]);
            SelectActiveDayFromDatePicker();
            SetFieldValue(ShipmentBuilderAppointmentStartTimeField, dropStart.Split(' ')[1]);
            SetFieldValue(ShipmentBuilderAppointmentEndField, dropEnd.Split(' ')[0]);
            SelectActiveDayFromDatePicker();
            SetFieldValue(ShipmentBuilderAppointmentEndTimeField, dropEnd.Split(' ')[1]);
            ClickElement(ShipmentBuilderChildSubmitButton);
            ClickElement(ShipmentBuilderParentSubmitButton);
        }

        /**
         * Method to select equipment type for a load.
         */
        public void EnterEquipment(string equipment)
        {
            ClickElement(ShipmentBuilderEquipmentButton);
            switch (equipment.ToLower().Replace(" ", ""))
            {
                case "flatbed":
                    ClickElement(ShipmentBuilderEquipmentFlatbedButton);
                    break;
                case "stepdeck":
                    ClickElement(ShipmentBuilderEquipmentStepdeckButton);
                    break;
                case "reefer":
                    ClickElement(ShipmentBuilderEquipmentReeferButton);
                    break;
                default:
                    ClickElement(ShipmentBuilderEquipmentVanButton);
                    break;
            }
            ClickElement(ShipmentBuilderEquipmentSubmitButton);
        }

        /**
         * Method to add an item to a load.
         */
        public void AddItem(string description, string minWeight, string maxWeight)
        {
            ClickElement(ShipmentBuilderItemsButton);
            ClickElement(ShipmentBuilderAddItemButton);
            SetFieldValue(ShipmentBuilderProductDescriptionField, description);
            SetFieldValue(ShipmentBuilderMinWeightField, minWeight);
            SetFieldValue(ShipmentBuilderMaxWeightField, maxWeight);
            ClickElement(ShipmentBuilderChildSubmitButton);
            ClickElement(ShipmentBuilderParentSubmitButton);
        }

        /**
         * Method to add money to a load.
         */
        public void AddMoney(string maxBuy)
        {
            ClickElement(ShipmentBuilderMoneyButton);
            SetFieldValue(ShipmentBuilderMaxBuyField, maxBuy);
            ClickElement(ShipmentBuilderParentSubmitButton);
        }

        /**
         * Method to enter notes for a load.
         */
        public void EnterNotes(string intNotes, string extNotes)
        {
            ClickElement(ShipmentBuilderNotesButton);
            SetFieldValue(ShipmentBuilderSpecialInstructionsIntField, intNotes);
            SetFieldValue(ShipmentBuilderSpecialInstructionsExtField, extNotes);
            ClickElement(ShipmentBuilderParentSubmitButton);
        }

        /**
         * Method to assign a load to carrier sales.
         */
        public void AssignToCarrierSales(string assign)
        {
            ClickElement(ShipmentBuilderShipmentAssignmentButton);
            if (assign.ToLower().Equals("no"))
            {
                ClickElement(ShipmentBuilderCarrierSalesNoButton);
            } else {
                ClickElement(ShipmentBuilderCarrierSalesYesButton);
            }
            ClickElement(ShipmentBuilderParentSubmitButton);
        }

        /**
         * Method to submit load as shipment.
         */
        public void SubmitShipment()
        {
            ClickElement(ShipmentBuilderSubmitButton);
        }

        /**
         * Method to view shipment in Optimizer.
         */
        public void ViewShipment()
        {
            ClickElement(ShipmentBuilderViewInOptimizerButton);
        }

        /**
         * Method to validate whether load was created successfully.
         */
        public Boolean LoadCreated()
        {
            int attempts = 0;
            while (attempts < 60)
            {
                try
                {
                    if (driver.FindElementBy(ShipmentBuilderSavedLabel).Displayed && driver.FindElementBy(ShipmentBuilderSavedConfirmation).Displayed)
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
         * Method to enter text into an alert.
         */
        public void EnterIntoAlert(string alertText)
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                if (alert != null)
                {
                    alert.SendKeys(alertText);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        /**
         * Method to accept an alert.
         */
        public void AcceptAlert()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                if (alert != null)
                {
                    alert.Accept();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        /**
         * Method to select the highlighted date in the date picker. 
         */
        public void SelectActiveDayFromDatePicker()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int attempts = 0;
            while (attempts < 10)
            {
                Thread.Sleep(SleepTime/4);
                try
                {
                    IWebElement element = driver.FindElementBy(ShipmentBuilderActiveDatePickerDay);
                    Wait.Until(d => element.Displayed);
                    element.Click();
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
                attempts++;
            }
        }
    
        /**
         * Method to click on an item by text it displays.
         */
        public void SelectItemByText(string text, By locator)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    IList<IWebElement> elements = driver.FindElementsBy(locator);
                    foreach (IWebElement element in elements)
                    {
                        if (element.Text.Equals(text))
                        {
                            element.Click();
                            break;
                        }
                    }
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
                attempts++;
                Thread.Sleep(SleepTime);
            }
        }

        /**
         * Method to click on an item by its relative index.
         */
        public void SelectItemByIndex(int i, By locator)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int attempts = 0;
            while (attempts < 10)
            {
                try
                {
                    IList<IWebElement> elements = driver.FindElementsBy(locator);
                    elements[i].Click();
                    break;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
                attempts++;
                Thread.Sleep(SleepTime);
            }
        }

        /**
         * Method to select an item from a dropdown.
         */
        public void SelectItemFromDropdown(string text, By dropdownLocator, By dropdownListLocator)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            ClickElement(dropdownLocator);
            IList<IWebElement> elements = driver.FindElementsBy(dropdownListLocator);
            foreach (IWebElement element in elements)
            {
                Wait.Until(d => element.Displayed);
                if (element.Text.Contains(text))
                {
                    element.Click();
                    break;
                }
            }
        }

        /**
         * Method to select an item from an autocomplete dropdown.
         */
        public void SelectItemFromAutocompleteDropdown(string text, By dropdownLocator, By dropdownListLocator)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            Thread.Sleep(SleepTime/5);
            Wait.Until(d => driver.FindElementBy(ShipmentBuilderWarehouseManagementAddressSearchAutocompleteField).Equals(driver.SwitchTo().ActiveElement()));
            SetFieldValue(dropdownLocator, text);
            int attempts = 0;
            {
                try
                {
                    IList<IWebElement> elements = driver.FindElementsBy(dropdownListLocator);
                    foreach (IWebElement element in elements)
                    {
                        Wait.Until(d => element.Displayed);
                        if (element.Text.Contains(text))
                        {
                            element.Click();
                            attempts = 10;
                            break;
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.StackTrace);
                }
                attempts++;
                Thread.Sleep(SleepTime);
            }
        }

        /**
         * Method to search for and select a warehouse.
         */
        public void SearchAndSelectWarehouse(string warehouseId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            ClickElement(ShipmentBuilderWarehouseSearchButton);
            SwitchToOptimizerWarehouseFrame(driver);
            SelectItemFromAutocompleteDropdown(warehouseId, ShipmentBuilderWarehouseManagementNameSearchAutocompleteField, WarehouseAutocompleteOptions);
            Wait.Until(d => driver.FindElementBy(ShipmentBuilderWarehouseFound).Displayed);
            SetFieldValue(ShipmentBuilderWarehouseNameField, ShipmentBuilderWarehouseNameValue);
            IWebElement warehouseContact = driver.FindElement(ShipmentBuilderHasContacts);

            if (warehouseContact.Text.Contains(NoContacts))
            {
                ClickElement(ShipmentBuilderAddContactButton);
                SetFieldValue(ShipmentBuilderWarehouseAddContactPhoneField, ShipmentBuilderWarehouseAddContactPhoneValue);
                SetFieldValue(ShipmentBuilderWarehouseAddContactFirstNameField, ShipmentBuilderWarehouseAddContactFirstNameValue);
                ClickElement(ShipmentBuilderAddContactSubmitButton);
            }
            ClickElement(ShipmentBuilderUseWarehouseButton);
            Wait.Until(d => driver.FindElementBy(ShipmentBuilderWarehouseSearchAutocompleteField).Equals(driver.SwitchTo().ActiveElement()));
        }

        /**
         * Method to select the mode for a load.
         */
        public void SelectMode(string mode)
        {
            SelectItemFromDropdown(mode, ModeDropdown, ModeDropdownOptions);
        }

        /**
         * Method to set pricing for a load.
         */
        public void SetPricing(string expectedRevenue, string expectedCost)
        {
            SetFieldValue(ExpectedRevenueField, "10");
            SetFieldValue(ExpectedCostField, "10");
        }

        /**
         * Method to search for and select a customer id from search results.
         */
        public void SearchAndSelectAccountById(string customerId)
        {
            SetFieldValue(AccountIdField, customerId);
            ClickSearchAccountButton();
            SelectItemByIndex(0, AccountSearchResults);
            KillWindow(OptimizerLookupPageTitle);
            SwitchToWindow("");
        }
    }
}
