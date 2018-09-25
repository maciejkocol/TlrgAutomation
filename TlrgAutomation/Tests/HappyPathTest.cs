using TlrgAutomation.Pages.Optimizer;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using TlrgAutomation.Pages.Tlrg;
using System.Threading;
using System.Data;
using System.Text;
using System.IO;
using TlrgAutomation.Managers;

namespace TlrgAutomation.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class HappyPathTest : BaseTest
    {
        public LoadManager.Load load;

        [ThreadStatic]
        protected static AddEditLoadPage addEditLoadPage;
        protected static LoadDetailsPage loadDetailsPage;

        [SetUp]
        protected virtual void _Setup()
        {
            load = LoadManager.GetLoad();
        }

        //[Test, Retry(2), Category("CreateLoad")]
        //[NonParallelizable]
        public void CreateLoadShipmentBuilder()
        {
            addEditLoadPage = new AddEditLoadPage();
            addEditLoadPage.SearchAndSelectAccountById(load.OptiCustomerId)
                .SelectMode(load.OptiMode)
                .SetPricing(load.OptiExpectedRevenue, load.OptiExpectedCost)
                .SaveLoad()
                .UpdateCargoValue(load.OptiCargoValue)
                .UpdateTenderType(load.OptiTenderType)
                .AddPickStop(load.OptiPickWarehouseG, addEditLoadPage.GetLoadDate(load.OptiPickStartDate, 1, 11), addEditLoadPage.GetLoadDate(load.OptiPickEndDate, 1, 13))
                .AddDropStop(load.OptiDropWarehouseGList, addEditLoadPage.GetLoadDate(load.OptiDropStartDateList, 2, 11), addEditLoadPage.GetLoadDate(load.OptiDropEndDateList, 2, 13))
                .EnterEquipment(load.OptiEquipment)
                .AddItem(load.OptiItemDescription, load.OptiItemMinWeight, load.OptiItemMaxWeight)
                .AddMoney(load.OptiMaxBuy)
                .EnterNotes(load.OptiIntNotes, load.OptiExtNotes)
                .AssignToCarrierSales(load.OptiAssignToCarrier)
                .SubmitShipment();
            Assert.IsTrue(addEditLoadPage.LoadCreated());
            string loadIdCreated = addEditLoadPage.GetLoadIdCreated();
            addEditLoadPage.ViewShipment();
            loadDetailsPage = new LoadDetailsPage();
            Assert.IsTrue(loadDetailsPage.LoadDisplayed());
            string loadIdDisplayed = loadDetailsPage.GetLoadIdDisplayed();
            Assert.AreEqual(loadIdCreated, loadIdDisplayed);
        }

        //[Test, Retry(2), Category("CreateLoad")]
        //[NonParallelizable]
        public void CreateMultiStopLoadShipmentBuilder()
        {
            addEditLoadPage = new AddEditLoadPage();
            addEditLoadPage.SearchAndSelectAccountById(load.OptiCustomerId)
                .SelectMode(load.OptiMode)
                .SetPricing(load.OptiExpectedRevenue, load.OptiExpectedCost)
                .SaveLoad()
                .UpdateCargoValue(load.OptiCargoValue)
                .UpdateTenderType(load.OptiTenderType)
                .AddPickStop(load.OptiPickWarehouseG, addEditLoadPage.GetLoadDate(load.OptiPickStartDate, 1, 11), addEditLoadPage.GetLoadDate(load.OptiPickEndDate, 1, 13))
                .AddMultiDropStop(load.OptiDropWarehouseGList, addEditLoadPage.GetLoadDate(load.OptiDropStartDateList, 2, 11), addEditLoadPage.GetLoadDate(load.OptiDropEndDateList, 2, 13))
                .EnterEquipment(load.OptiEquipment)
                .AddItem(load.OptiItemDescription, load.OptiItemMinWeight, load.OptiItemMaxWeight)
                .AddMoney(load.OptiMaxBuy)
                .EnterNotes(load.OptiIntNotes, load.OptiExtNotes)
                .AssignToCarrierSales(load.OptiAssignToCarrier)
                .SubmitShipment();
            Assert.IsTrue(addEditLoadPage.LoadCreated());
            string loadIdCreated = addEditLoadPage.GetLoadIdCreated();
            addEditLoadPage.ViewShipment();
            loadDetailsPage = new LoadDetailsPage();
            Assert.IsTrue(loadDetailsPage.LoadDisplayed());
            string loadIdDisplayed = loadDetailsPage.GetLoadIdDisplayed();
            Assert.AreEqual(loadIdCreated, loadIdDisplayed);
        }

        [Test, Retry(2), Category("Smoke")]
        [NonParallelizable]
        public void StartWaterfallProcessInProgress()
        {
            string customerId = load.OptiCustomerId;
            string carrierId = load.OptiCarrierId;
            string uniqueName = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            AccountsPage optimizerAccountsPageElements = new AccountsPage()
                .SyncCustomerWithTlrg(customerId);
            SettingsPage settingsPage = new SettingsPage()
                .ClickCustomerSettingsGeneralLink()
                .SearchAndSelectCustomer(customerId)
                .SetCustomerGeneralFields("2", "125", "1", uniqueName + "@echo.com", "(GMT -6:00) Central Time (US & Canada), Mexico City")
                .ClickSaveButton()
                .WaitForSaveConfirmation()
                .SyncNewCarrier(carrierId);
            PricingUploadPage pricingUploadPage = new PricingUploadPage()
                .UploadLineHaulRates(customerId, carrierId, uniqueName);
            CreateLoadShipmentBuilder();
            string expectedLoadId = loadDetailsPage.GetLoadIdDisplayed();
            loadDetailsPage.ClickElement(loadDetailsPage.StartTlrgButton);
            string actualWarning = driver.FindElementBy(loadDetailsPage.LoadWarning).Text;
            string expectedWarning = "You are currently viewing a READ ONLY version of this page. No changes are allowed.";
            string expectedStatus = "In Progress";
            Assert.AreEqual(expectedWarning, actualWarning);
            HomePage homePage = new HomePage();
            homePage.ClickAllTab()
                .WaitForStartTlrgLoad(expectedLoadId, customerId, expectedStatus)
                .SearchAndSelectCustomer(customerId)
                .FilterLoads(homePage.LoadIdField, expectedLoadId);
            IList<string> actualLoad = homePage.GetLoadAtIndex(0);
            HomePageTable table = new HomePageTable();
            HomePageRow row = table.GetRowByLoadId(expectedLoadId);
            Assert.AreEqual(expectedLoadId, row.LoadId);
            Assert.AreEqual(expectedStatus, row.Status);
        }

        [Test, Retry(2), Category("Smoke")]
        [NonParallelizable]
        public void LoadCogsCalculation()
        {
            string customerId = load.OptiCustomerId;
            string carrierId = load.OptiCarrierId;
            string uniqueName = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            AccountsPage optimizerAccountsPageElements = new AccountsPage()
                .SyncCustomerWithTlrg(customerId);
            SettingsPage settingsPage = new SettingsPage()
                .ClickCustomerSettingsGeneralLink()
                //.SearchAndSelectCustomer(customerId)
                //.SetCustomerGeneralFields("2", "125", "1", uniqueName + "@echo.com", "(GMT -6:00) Central Time (US & Canada), Mexico City")
                //.ClickSaveButton()
                //.WaitForSaveConfirmation()
                .SyncNewCarrier(carrierId);
            PricingUploadPage pricingUploadPage = new PricingUploadPage()
                .UploadLineHaulMultiRates(customerId, carrierId, uniqueName)
                .UploadFuelSchedule(customerId, carrierId)
                .UploadSpecialServices(customerId, carrierId);
            CreateMultiStopLoadShipmentBuilder();
            string expectedLoadId = loadDetailsPage.GetLoadIdDisplayed();
            loadDetailsPage.ClickElement(loadDetailsPage.StartTlrgButton);
            string actualWarning = driver.FindElementBy(loadDetailsPage.LoadWarning).Text;
            string expectedWarning = "You are currently viewing a READ ONLY version of this page. No changes are allowed.";
            string expectedStatus = "In Progress";
            Assert.AreEqual(expectedWarning, actualWarning);
            HomePage homePage = new HomePage();
            string expectedCogs = homePage.GetLoadCogs(expectedLoadId, 0.01, 100.00, 215.00, 0.3, 1000.00);
            homePage.ClickAllTab()
                .WaitForStartTlrgLoad(expectedLoadId, customerId, expectedStatus)
                .SearchAndSelectCustomer(customerId)
                .FilterLoads(homePage.LoadIdField, expectedLoadId);
            IList<string> actualLoad = homePage.GetLoadAtIndex(0);
            string actualLoadId = actualLoad[0];
            string actualCogs = actualLoad[3];
            string actualStatus = actualLoad[4];
            Assert.AreEqual(expectedLoadId, actualLoadId);
            Assert.AreEqual(expectedCogs, actualCogs);
            Assert.AreEqual(expectedStatus, actualStatus);
        }

        [Test, Retry(2), Category("Smoke")]
        [NonParallelizable]
        public void TimeoutWaterfallProcess()
        {
            string customerId = load.OptiCustomerId;
            string carrier1Id = "";
            string carrier2Id = "";
            string timeOut = "2";
            while (carrier1Id == carrier2Id)
            {
                carrier1Id = dbAccess.GetUnsyncedCarrierRow()["CarrierId"].ToString();
                carrier2Id = dbAccess.GetUnsyncedCarrierRow()["CarrierId"].ToString();
            }
            string uniqueName = "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff");
            AccountsPage optimizerAccountsPageElements = new AccountsPage()
                .SyncCustomerWithTlrg(customerId);
            SettingsPage settingsPage = new SettingsPage()
                .ClickCustomerSettingsGeneralLink()
                .SearchAndSelectCustomer(customerId)
                .SetCustomerGeneralFields(timeOut, "125", "1", uniqueName + "@echo.com", "(GMT -6:00) Central Time (US & Canada), Mexico City")
                .ClickSaveButton()
                .WaitForSaveConfirmation()
                .SyncNewCarrier(carrier1Id)
                .SyncNewCarrier(carrier2Id);
            PricingUploadPage pricingUploadPage = new PricingUploadPage()
                .UploadLineHaulMultiRates(customerId, carrier1Id, "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff"))
                .UploadFuelSchedule(customerId, carrier1Id)
                .UploadSpecialServices(customerId, carrier1Id)
                .UploadLineHaulMultiRates(customerId, carrier2Id, "qa" + DateTime.Now.ToString("yyyyMMddHHmmssfff"))
                .UploadFuelSchedule(customerId, carrier2Id)
                .UploadSpecialServices(customerId, carrier2Id);
            CreateMultiStopLoadShipmentBuilder();
            string expectedLoadId = loadDetailsPage.GetLoadIdDisplayed();
            loadDetailsPage.ClickElement(loadDetailsPage.StartTlrgButton);
            string actualWarning = driver.FindElementBy(loadDetailsPage.LoadWarning).Text;
            string expectedWarning = "You are currently viewing a READ ONLY version of this page. No changes are allowed.";
            string expectedStatus = "In Progress";
            Assert.AreEqual(expectedWarning, actualWarning);
            HomePage homePage = new HomePage();
            homePage.ClickAllTab()
                .WaitForStartTlrgLoad(expectedLoadId, customerId, expectedStatus)
                .SearchAndSelectCustomer(customerId)
                .FilterLoads(homePage.LoadIdField, expectedLoadId);




            IList<string> actualLoad = homePage.GetLoadAtIndex(0);
            string actualLoadId = actualLoad[0];
            string actualCogs = actualLoad[3];
            string actualStatus = actualLoad[4];
            Assert.AreEqual(expectedLoadId, actualLoadId);
            //Assert.AreEqual(expectedCogs, actualCogs);
            Assert.AreEqual(expectedStatus, actualStatus);
        }

        //[Test, Category("EchoDrive")]
        //[NonParallelizable]
        public void CreateRandomLoadTest()
        {
            Random rndGen = new Random();
            LoadManager.Load load = LoadManager.GetLoad();
            AddEditLoadPage addEditLoadPage = new AddEditLoadPage();

            List<string> customerIds = new List<string>(new string[] { "E49035", "E90771", "E61470", "E36516", "E101257", "E124164" });
            List<string> dollarAmounts = new List<string>(new string[] { "0", "1", ".5", "1000000", "-3", "1000", "100" });
            List<string> equipmentTypes = new List<string>(new string[] { "van", "flatbed", "stepdeck", "reefer" });
            List<string> warehouseIds = new List<string>(new string[] { "G12000", "G2928940", "G3317302", "G900009", "G810013",
                                                                        "G530010", "G500006", "G990007"});
            List<int> randNumDaysForward = new List<int>(new int[] { rndGen.Next(0, 15), rndGen.Next(0, 15) });
            List<int> randTimeHours = new List<int>(new int[] { rndGen.Next(0, 23), rndGen.Next(0, 23) });
            List<int> randWeight = new List<int>(new int[] { rndGen.Next(10000, 20000), rndGen.Next(10000, 20000) });
            randNumDaysForward.Sort();
            randTimeHours.Sort();
            randWeight.Sort();

            string randomCustomer = customerIds[rndGen.Next(customerIds.Count)];
            string today = DateTime.Today.ToLongDateString();

            addEditLoadPage.SearchAndSelectAccountById(randomCustomer)
                .SelectMode(load.OptiMode)
                .SelectWeOwnOrCanGet(rndGen.Next(0, 5) == 1 ? "canget" : "weown")
                .SetPricing(dollarAmounts[rndGen.Next(dollarAmounts.Count)], dollarAmounts[rndGen.Next(dollarAmounts.Count)])
                .SaveLoad()
                .UpdateCargoValue(load.OptiCargoValue)
                .UpdateTenderType(load.OptiTenderType)
                .AddPickStop(warehouseIds[rndGen.Next(warehouseIds.Count)], addEditLoadPage.GetLoadDate(today, randNumDaysForward[0], randTimeHours[0]), addEditLoadPage.GetLoadDate(today, randNumDaysForward[1], randTimeHours[1]))
                .AddDropStop(warehouseIds[rndGen.Next(warehouseIds.Count)], addEditLoadPage.GetLoadDate(today, randNumDaysForward[0], randTimeHours[0]), addEditLoadPage.GetLoadDate(today, randNumDaysForward[1], randTimeHours[1]))
                .ClickElement(addEditLoadPage.ShipmentBuilderParentSubmitButton)
                .AddEquipmentWithRandomSpecialServices(equipmentTypes[rndGen.Next(0, equipmentTypes.Count)])
                .AddItem(load.OptiItemDescription, randWeight[0].ToString(), randWeight[1].ToString(), rndGen.Next(1, 3))
                .AddMoney(load.OptiMaxBuy)
                .EnterNotes(load.OptiIntNotes, load.OptiExtNotes)
                .AssignToCarrierSales(rndGen.Next(0, 5) == 1 ? "yes" : "no")
                .SubmitShipment();
            string loadIdCreated = addEditLoadPage.GetLoadIdCreated();
            addEditLoadPage.ViewShipment();
            DataRow loadGuidRow = dbAccess.GetLoadGuidFromLoadId(loadIdCreated);
            string loadGuid = loadGuidRow["LoadGuid"].ToString();

            StringBuilder logText = new StringBuilder();
            string filename = "C:\\users\\cwebb\\desktop\\loads3.csv";
            if (!File.Exists(filename))
            {
                logText.Append(loadIdCreated)
                    .Append(",")
                    .Append(loadGuid);
            }
            else
            {
                logText
                    .Append(",")
                    .Append(Environment.NewLine)
                    .Append(loadIdCreated)
                    .Append(",")
                    .Append(loadGuid);
            }
            File.AppendAllText(filename, logText.ToString());
            RequestManager rm = Fixtures.GetRequestManager();
            rm.InitializeLoadByGuid(loadGuid);
        }
    }
}
