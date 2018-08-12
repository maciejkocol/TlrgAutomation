using Tlrg.Pages.Optimizer;
using NUnit.Framework;
using System;

namespace Tlrg.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.Children)]
    public class CreateLoadTest: BaseTest
    {
        [ThreadStatic]
        protected static AddEditLoadPage addEditLoadPage;
        
        [Test, Retry(2)]
        public void CreateLoadShipmentBuilder()
        {
            addEditLoadPage = new AddEditLoadPage(driver);
            addEditLoadPage.SearchAndSelectAccountById(env.OptiCustomerId);
            addEditLoadPage.SelectMode(env.OptiMode);
            addEditLoadPage.SetPricing(env.OptiExpectedRevenue, env.OptiExpectedCost);
            addEditLoadPage.SaveLoad();
            addEditLoadPage.UpdateCargoValue(env.OptiCargoValue);
            addEditLoadPage.UpdateTenderType(env.OptiTenderType);
            addEditLoadPage.AddPickStop(env.OptiPickWarehouseG, env.OptiPickStartDate, env.OptiPickEndDate);
            addEditLoadPage.AddDropStop(env.OptiDropWarehouseG, env.OptiDropStartDate, env.OptiDropEndDate);
            addEditLoadPage.EnterEquipment(env.OptiEquipment);
            addEditLoadPage.AddItem(env.OptiItemDescription, env.OptiItemMinWeight, env.OptiItemMaxWeight);
            addEditLoadPage.AddMoney(env.OptiMaxBuy);
            addEditLoadPage.EnterNotes(env.OptiIntNotes, env.OptiExtNotes);
            addEditLoadPage.AssignToCarrierSales(env.OptiAssignToCarrier);
            addEditLoadPage.SubmitShipment();
            Assert.IsTrue(addEditLoadPage.LoadCreated());
            addEditLoadPage.ViewShipment();
        }
    }
}
