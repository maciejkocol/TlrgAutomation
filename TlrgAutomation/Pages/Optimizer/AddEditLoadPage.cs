using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TlrgAutomation.Pages.Optimizer
{
    public class AddEditLoadPage : BasePage
    {
        public AddEditLoadPage() : base()
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
        public By ShipmentBuilderItemDropField = By.Id("Drop");
        public By ShipmentBuilderProductDescriptionField = By.Id("ProductDescription");
        public By ShipmentBuilderMinWeightField = By.Id("EstMinWeight");
        public By ShipmentBuilderMaxWeightField = By.Id("EstMaxWeight");
        public By ShipmentBuilderMaxBuyField = By.Id("MaxBuy");
        public By ShipmentBuilderSpecialInstructionsIntField = By.Id("spInstrInt");
        public By ShipmentBuilderSpecialInstructionsExtField = By.Id("spInstrExt");

        // labels and text
        public By ShipmentBuilderSavedLabel = By.XPath("//label[text()='Shipment Saved']");
        public By ShipmentBuilderSavedConfirmation = By.XPath("//b[contains(.,'Shipment') and contains(.,'has been created')]");
        public By ShipmentBuilderLoadIdCreated = By.XPath("//b[contains(.,'Shipment') and contains(.,'has been created')]/span[@class='heading']");
        public By ShipmentBuilderWarehouseFound = By.XPath("//div[@id='warehouse-found-display' and @style='']");
        public By ShipmentBuilderHasContacts = By.XPath("(//p[text()='No Contacts']) | (//button[text()='Remove contact'])");
        public By ShipmentBuilderAssociatedItem = By.Id("crossItemErrorDiv");

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
        public By ShipmentBuilderEquipmentTeamServicesButton = By.Id("id1");
        public By ShipmentBuilderEquipmentStepdeckButton = By.Id("EquipmentId27");
        public By ShipmentBuilderEquipmentReeferButton = By.Id("EquipmentId42");
        public By ShipmentBuilderEquipmentSubmitButton = By.XPath("//button/span[text()='Submit']");
        public By ShipmentBuilderAddItemButton = By.Id("additem");
        public By ShipmentBuilderCarrierSalesYesButton = By.Id("btnCarrierSalesYes");
        public By ShipmentBuilderCarrierSalesNoButton = By.Id("btnCarrierSalesNo");
        public By ShipmentBuilderSubmitButton = By.XPath("//div[@id='shipment']//input[@id='submit']");
        public By ShipmentBuilderViewInOptimizerButton = By.Id("btnViewInOptimizer");
        public By ShipmentBuilderActiveDatePickerDay = By.XPath("//td[contains(@class,'datepicker-days-cell-over')]//a[contains(@class,'ui-state-active')]");

        //Special Services
        public By TeamServicesCheckbox = By.Id("id1");
        public By BlanketWrappedCheckbox = By.Id("id2");
        public By StrapsCheckbox = By.Id("id3");
        public By PalletExchangeCheckbox = By.Id("id4");
        public By TWICCardCheckbox = By.Id("id5");
        public By DropTrailerShipperCheckbox = By.Id("id6");
        public By ODCheckbox = By.Id("id8");
        public By LoadBarCheckbox = By.Id("id11");
        public By DriverAssistCheckbox = By.Id("id12");
        public By PalletJackCheckbox = By.Id("id13");
        public By DropTrailerCheckbox = By.Id("id14");
        public By PapsFastApprovedCheckbox = By.Id("id16");
        public By TempRequirementsMin = By.Id("minid17");
        public By TempRequirementsMax = By.Id("maxid17");
        public By TemperatureRequirementsCheckbox = By.Id("id17");

        //RadioButtons
        public By WeOwnItRadioButton = By.Id("rbWeWonIt");
        public By CanGetRadioButton = By.Id("rbCanGet");

        // dropdowns
        public By ModeDropdown = By.Id("ucPickMode_ddlMode");
        public By TenderTypeDropdown = By.Id("updateTenderTypeSelectId");
        public By TarpsDropdown = By.Id("id9");

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
        public AddEditLoadPage WaitForAddEditLoadPageToLoad()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            IWebElement accountNameFieldElement = driver.FindElementBy(AccountNameField);
            IWebElement accountIdFieldElement = driver.FindElementBy(AccountIdField);
            Wait.Until(d => accountNameFieldElement.Displayed);
            Wait.Until(d => accountIdFieldElement.Displayed);
            return this;
        }

        /**
         * Method to click the Search account button.
         */
        public AddEditLoadPage ClickSearchAccountButton()
        {
            ClickElement(SearchAccountButton);
            return this;
        }

        /**
         * Method to save created load. 
         */
        public AddEditLoadPage SaveLoad()
        {
            ClickElement(ShipmentBuilderSaveButton);
            return this;
        }

        /**
         * Method to update the cargo value of a load.
         */
        public AddEditLoadPage UpdateCargoValue(string value)
        {
            ClickElement(ShipmentBuilderAccountButton);
            ClickElement(ShipmentBuilderUpdateCargoValueButton);
            EnterIntoAlert(value);
            AcceptAlert();
            return this;
        }

        /**
         * Method to update the tender type of a load.
         */
        public AddEditLoadPage UpdateTenderType(string type)
        {
            ClickElement(ShipmentBuilderUpdateTenderTypeButton);
            SelectItemFromDropdown(type, TenderTypeDropdown, TenderTypeDropdownOptions);
            return this;
        }

        /**
         * Method to add pick-up stop for a load.
         */
        public AddEditLoadPage AddPickStop(string warehouseG, string pickStart, string pickEnd)
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
            return this;
        }

        /**
         * Method to add a drop stop to a load.
         */
        public AddEditLoadPage AddDropStop(string warehouseG, string dropStart, string dropEnd)
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
            return this;
        }

        /**
         * Overload method to add a drop stop to a load.
         */
        public AddEditLoadPage AddDropStop(List<string> warehouseGList, List<string> dropStart, List<string> dropEnd)
        {
            try
            {
                AddDropStop(warehouseGList[0], dropStart[0], dropEnd[0]);
            }
            catch (Exception e)
            {
                throw new IndexOutOfRangeException("Unable to find first drop stop in list: \"" + e.ToString() + "\".");
            }
            ClickElement(ShipmentBuilderParentSubmitButton);
            return this;
        }

        /**
         * Method to add multiple drop stops to a load.
         */
        public AddEditLoadPage AddMultiDropStop(List<string> warehouseGList, List<string> dropStart, List<string> dropEnd)
        {
            try
            {
                for (int i=0; i < warehouseGList.Count; i++)
                {
                    AddDropStop(warehouseGList[i], dropStart[i], dropEnd[i]);
                    Thread.Sleep(SleepTime/2);
                }
            }
            catch (Exception e)
            {
                throw new IndexOutOfRangeException("Drop stop lists are not equal in size: \"" + e.ToString() + "\".");
            }
            ClickElement(ShipmentBuilderParentSubmitButton);
            return this;
        }

        /**
         * Method to select equipment type for a load.
         */
        public AddEditLoadPage EnterEquipment(List<string> equipmentList)
        {
            ClickElement(ShipmentBuilderEquipmentButton);
            foreach (string equipment in equipmentList)
            {
                switch (equipment.ToLower().Replace(" ", ""))
                {
                    case "flatbed":
                        ClickElement(ShipmentBuilderEquipmentFlatbedButton);
                        break;
                    case "teamservices":
                        ClickElement(ShipmentBuilderEquipmentTeamServicesButton);
                        break;
                    case "tarps":
                        SelectTarps(load.OptiTarps);
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
            }
            ClickElement(ShipmentBuilderEquipmentSubmitButton);
            return this;
        }

        public AddEditLoadPage AddEquipmentWithRandomSpecialServices(string equipmentType)
        {
            Random rand = new Random();
            List<By> specialServicesCheckboxes = new List<By>(new By[] { TeamServicesCheckbox, BlanketWrappedCheckbox,
                StrapsCheckbox, PalletExchangeCheckbox,TWICCardCheckbox, DropTrailerShipperCheckbox,
            ODCheckbox, LoadBarCheckbox, DriverAssistCheckbox, PalletJackCheckbox,
            DropTrailerCheckbox, PapsFastApprovedCheckbox});

            ClickElement(ShipmentBuilderEquipmentButton);
            switch (equipmentType.ToLower().Replace(" ", ""))
            {
                case "flatbed":
                    ClickElement(ShipmentBuilderEquipmentFlatbedButton);
                    SelectElement selector = new SelectElement(driver.FindElementBy(TarpsDropdown));
                    selector.SelectByText("4 ft");
                    for (int i = 0; i < 4; i++)
                    {
                        ClickElement(specialServicesCheckboxes[rand.Next(0, specialServicesCheckboxes.Count)]);
                    }
                    break;
                case "stepdeck":
                    ClickElement(ShipmentBuilderEquipmentStepdeckButton);
                    SelectElement selector2 = new SelectElement(driver.FindElementBy(TarpsDropdown));
                    selector2.SelectByText("4 ft");
                    for (int i = 0; i < 4; i++)
                    {
                        ClickElement(specialServicesCheckboxes[rand.Next(0, specialServicesCheckboxes.Count)]);
                    }
                    break;
                case "reefer":
                    ClickElement(ShipmentBuilderEquipmentReeferButton);
                    ClickElement(TemperatureRequirementsCheckbox);
                    SetFieldValue(TempRequirementsMin, "50");
                    SetFieldValue(TempRequirementsMax, "100");
                    for (int i = 0; i < rand.Next(0, specialServicesCheckboxes.Count); i++)
                    {
                        ClickElement(specialServicesCheckboxes[rand.Next(0, specialServicesCheckboxes.Count)]);
                    }
                    break;
                default:
                    ClickElement(ShipmentBuilderEquipmentVanButton);
                    for (int i = 0; i < 4; i++)
                    {
                        ClickElement(specialServicesCheckboxes[rand.Next(0, specialServicesCheckboxes.Count)]);
                    }
                    break;
            }
            ClickElement(ShipmentBuilderEquipmentSubmitButton);
            return this;
        }

        /**
         * Method to add items to a load.
         */
        public AddEditLoadPage AddItem(string description, string minWeight, string maxWeight)
        {
            ClickElement(ShipmentBuilderItemsButton);
            for(int i=1; i<=AssociatedDropItemCount(); i++)
            {
                Thread.Sleep(SleepTime / 2);
                ClickElement(ShipmentBuilderAddItemButton);
                SelectDrop(i.ToString());
                SetFieldValue(ShipmentBuilderProductDescriptionField, description);
                SetFieldValue(ShipmentBuilderMinWeightField, minWeight);
                SetFieldValue(ShipmentBuilderMaxWeightField, maxWeight);
                ClickElement(ShipmentBuilderChildSubmitButton);
                Thread.Sleep(SleepTime / 2);
            }
            ClickElement(ShipmentBuilderParentSubmitButton);
            return this;
        }

        /**
        * Method to add an item to a load.
        */
        public AddEditLoadPage AddItem(string description, string minWeight, string maxWeight, int itemsToAdd = 1)
        {
            ClickElement(ShipmentBuilderItemsButton);
            for (int i = 0; i < itemsToAdd; i++)
            {
                ClickElement(ShipmentBuilderAddItemButton);
                SetFieldValue(ShipmentBuilderProductDescriptionField, description);
                SetFieldValue(ShipmentBuilderMinWeightField, minWeight);
                SetFieldValue(ShipmentBuilderMaxWeightField, maxWeight);
                ClickElement(ShipmentBuilderChildSubmitButton);
            }
            ClickElement(ShipmentBuilderParentSubmitButton);
            return this;
        }

        /**
         * Method to get associated drop item count.
         */
        public int AssociatedDropItemCount()
        {
            return Regex.Matches(driver.FindElementBy(ShipmentBuilderAssociatedItem).Text.ToLower(), "drop").Count;
        }

        /**
         * Method to add money to a load.
         */
        public AddEditLoadPage AddMoney(string maxBuy)
        {
            ClickElement(ShipmentBuilderMoneyButton);
            SetFieldValue(ShipmentBuilderMaxBuyField, maxBuy);
            ClickElement(ShipmentBuilderParentSubmitButton);
            return this;
        }

        /**
         * Method to enter notes for a load.
         */
        public AddEditLoadPage EnterNotes(string intNotes, string extNotes)
        {
            ClickElement(ShipmentBuilderNotesButton);
            SetFieldValue(ShipmentBuilderSpecialInstructionsIntField, intNotes);
            SetFieldValue(ShipmentBuilderSpecialInstructionsExtField, extNotes);
            ClickElement(ShipmentBuilderParentSubmitButton);
            return this;
        }

        /**
         * Method to assign a load to carrier sales.
         */
        public AddEditLoadPage AssignToCarrierSales(string assign)
        {
            ClickElement(ShipmentBuilderShipmentAssignmentButton);
            if (assign.ToLower().Equals("no"))
            {
                ClickElement(ShipmentBuilderCarrierSalesNoButton);
            }
            else
            {
                ClickElement(ShipmentBuilderCarrierSalesYesButton);
            }
            ClickElement(ShipmentBuilderParentSubmitButton);
            return this;
        }

        /**
         * Method to submit load as shipment.
         */
        public AddEditLoadPage SubmitShipment()
        {
            ClickElement(ShipmentBuilderSubmitButton);
            return this;
        }

        /**
         * Method to view shipment in Optimizer.
         */
        public AddEditLoadPage ViewShipment()
        {
            ClickElement(ShipmentBuilderViewInOptimizerButton);
            return this;
        }
        
        public new AddEditLoadPage ClickElement(By element)
        {
            return (AddEditLoadPage)(base.ClickElement(element));
        }

        /**
         * Method to validate whether load was created successfully.
         */
        public Boolean LoadCreated()
        {
            int attempts = 0;
            while (attempts < 30)
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
         * Method to get load Id that was allegedly created.
         */
        public string GetLoadIdCreated()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int attempts = 0;
            while (attempts < 10)
            {
                Thread.Sleep(SleepTime / 4);
                try
                {
                    IWebElement element = driver.FindElementBy(ShipmentBuilderLoadIdCreated);
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
            return driver.FindElementBy(ShipmentBuilderLoadIdCreated).Text;
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
        public AddEditLoadPage SelectActiveDayFromDatePicker()
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            int attempts = 0;
            while (attempts < 10)
            {
                Thread.Sleep(SleepTime / 4);
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
            return this;
        }

        public AddEditLoadPage SelectWeOwnOrCanGet(string value)
        {
            switch (value.ToLower())
            {
                case "weown":
                    ClickElement(WeOwnItRadioButton);
                    break;
                case "canget":
                    ClickElement(CanGetRadioButton);
                    break;
            }
            return this;
        }

        /**
         * Method to click on an item by text it displays.
         */
        public AddEditLoadPage SelectItemByText(string text, By locator)
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
            return this;
        }

        /**
         * Method to click on an item by its relative index.
         */
        public AddEditLoadPage SelectItemByIndex(int i, By locator)
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
            return this;
        }

        /**
         * Method to select an item from a dropdown.
         */
        public AddEditLoadPage SelectItemFromDropdown(string text, By dropdownLocator, By dropdownListLocator)
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
            return this;
        }

        /**
         * Method to select an item from an autocomplete dropdown.
         */
        public AddEditLoadPage SelectItemFromAutocompleteDropdown(string text, By dropdownLocator, By dropdownListLocator)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            Thread.Sleep(SleepTime / 5);
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
            return this;
        }

        /**
         * Method to search for and select a warehouse.
         */
        public AddEditLoadPage SearchAndSelectWarehouse(string warehouseId)
        {
            WebDriverWait Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
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
            return this;
        }

        /**
         * Method to select the mode for a load.
         */
        public AddEditLoadPage SelectMode(string mode)
        {
            SelectItemFromDropdown(mode, ModeDropdown, ModeDropdownOptions);
            return this;
        }

        /**
         * Method to select the tarps for a load.
         */
        public AddEditLoadPage SelectTarps(string tarp)
        {
            driver.FindElementBy(TarpsDropdown).SendKeys(tarp);
            return this;
        }

        /**
         * Method to select the drop for an item.
         */
        public AddEditLoadPage SelectDrop(string drop)
        {
            driver.FindElementBy(ShipmentBuilderItemDropField).SendKeys(drop);
            return this;
        }

        /**
         * Method to set pricing for a load.
         */
        public AddEditLoadPage SetPricing(string expectedRevenue, string expectedCost)
        {
            SetFieldValue(ExpectedRevenueField, "10");
            SetFieldValue(ExpectedCostField, "10");
            return this;
        }

        /**
         * Method to search for and select a customer id from search results.
         */
        public AddEditLoadPage SearchAndSelectAccountById(string customerId)
        {
            SetFieldValue(AccountIdField, customerId);
            ClickSearchAccountButton();
            SelectItemByIndex(0, AccountSearchResults);
            KillWindow(OptimizerLookupPageTitle);
            SwitchToWindow("");
            return this;
        }

        /**
         * Method to get current date advanced by given number of days and hours.
         */
        public string GetLoadDate(string date, int days, int hours)
        {
            string expectedDateFormat = "MM/dd/yyyy H:mm";
            DateTime dt;
            if (DateTime.TryParseExact(date, expectedDateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                return date;
            }
            return DateTime.Today.AddDays(days).AddHours(hours).ToString(expectedDateFormat);
        }

        /**
         * Overload method to get current date advanced by given number of days and hours.
         */
        public List<string> GetLoadDate(List<string> dates, int days, int hours)
        {
            List<string> loadDates = new List<string>(new string[] {});
            foreach (string date in dates)
            {
                string loadDate = GetLoadDate(date, days, hours);
                loadDates.Add(loadDate);
                days++;
            }
            return loadDates;
        }
    }
}
