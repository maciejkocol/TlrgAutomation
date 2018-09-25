using System;
using System.Collections.Generic;

namespace TlrgAutomation
{
    public class LoadManager
    {
        public class Load
        {
            public string OptiCustomerId;
            public string OptiCarrierId;
            public string OptiMode;
            public string OptiExpectedRevenue;
            public string OptiExpectedCost;
            public string OptiCargoValue;
            public string OptiTenderType;
            public string OptiPickWarehouseG;
            public string OptiPickStartDate;
            public string OptiPickEndDate;
            public List<string> OptiDropWarehouseGList;
            public List<string> OptiDropStartDateList;
            public List<string> OptiDropEndDateList;
            public List<string> OptiEquipment;
            public string OptiTarps;
            public string OptiItemDescription;
            public string OptiItemMinWeight;
            public string OptiItemMaxWeight;
            public string OptiMaxBuy;
            public string OptiIntNotes;
            public string OptiExtNotes;
            public string OptiAssignToCarrier;
        }

        public static Load GetLoad()
        {
            return new Load()
            {
                OptiCustomerId = "E62106",
                OptiCarrierId = "L10671",
                OptiMode = "TL",
                OptiExpectedRevenue = "10",
                OptiExpectedCost = "10",
                OptiCargoValue = "1",
                OptiTenderType = "Routing Guide",
                OptiPickWarehouseG = "G12000",
                //OptiPickStartDate = "12/18/2018 11:00",
                //OptiPickEndDate = "12/18/2018 13:00",
                OptiPickStartDate = "",
                OptiPickEndDate = "",
                OptiDropWarehouseGList = new List<string>(new string[] { "G120004", "G120005", "G120010" }),
                //OptiDropStartDateList = new List<string>(new string[] { "12/19/2018 11:00", "12/20/2018 11:00" }),
                //OptiDropEndDateList = new List<string>(new string[] { "12/19/2018 13:00", "12/20/2018 13:00" }),
                OptiDropStartDateList = new List<string>(new string[] { "", "", "" }),
                OptiDropEndDateList = new List<string>(new string[] { "", "", "" }),
                OptiEquipment = new List<string>(new string[] { "flatbed", "team services", "tarps" }),
                OptiTarps = "6 ft",
                OptiItemDescription = "test",
                OptiItemMinWeight = "100",
                OptiItemMaxWeight = "1000",
                OptiMaxBuy = "5",
                OptiIntNotes = "!",
                OptiExtNotes = "=",
                OptiAssignToCarrier = "no"
            };
        }
    }
}
