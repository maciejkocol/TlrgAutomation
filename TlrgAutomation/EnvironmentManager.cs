using System;

namespace Tlrg
{
    public class EnvironmentManager
    {
        public class Environment
        {
            // Shared
            public bool Headless;
            public string DBPath;

            // TLRG specific
            public string TlrgURL;
            public string TlrgCustomerId;

            // Optimizer specific
            public string OptiURL;
            public string OptiUser;
            public string OptiPassword;
            public string OptiCustomerId;
            public string OptiMode;
            public string OptiExpectedRevenue;
            public string OptiExpectedCost;
            public string OptiCargoValue;
            public string OptiTenderType;
            public string OptiPickWarehouseG;
            public string OptiPickStartDate;
            public string OptiPickEndDate;
            public string OptiDropWarehouseG;
            public string OptiDropStartDate;
            public string OptiDropEndDate;
            public string OptiEquipment;
            public string OptiItemDescription;
            public string OptiItemMinWeight;
            public string OptiItemMaxWeight;
            public string OptiMaxBuy;
            public string OptiIntNotes;
            public string OptiExtNotes;
            public string OptiAssignToCarrier;
        }

        public static Environment GetEnvironment(string aEnvironmentName)
        {
            Environment result = Dev1();
            switch (aEnvironmentName.ToUpper())
            {
                case "DEV1":
                    result = Dev1();
                    break;
                case "DEV2":
                    result = Dev2();
                    break;
                case "QA1":
                    result = Qa1();
                    break;
                case "QA2":
                    result = Qa2();
                    break;
                case "QA3":
                    result = Qa3();
                    break;
                default:
                    throw new Exception("Invalid String, unable to select environment");
            }
            return result;
        }
        
        public static Environment Dev1()
        {
            return new Environment()
            {
                Headless = true,
                DBPath = "tlrg-db01.dev.echogl.net\\db01",
                TlrgURL = "http://tlrg-pltweb01.dev.echogl.net/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://tlrg-www.dev.echogl.net/Optimizer/AddEditLoad.aspx",
                OptiUser = "Automation",
                OptiPassword = "Test1234",
                OptiCustomerId = "E129651",
                OptiMode = "TL",
                OptiExpectedRevenue = "10",
                OptiExpectedCost = "10",
                OptiCargoValue = "1",
                OptiTenderType = "Routing Guide",
                OptiPickWarehouseG = "G12000",
                OptiPickStartDate = "08/18/2018 9:00",
                OptiPickEndDate = "08/18/2018 17:00",
                OptiDropWarehouseG = "G120004",
                OptiDropStartDate = "08/19/2018 9:00",
                OptiDropEndDate = "08/19/2018 17:00",
                OptiEquipment = "van",
                OptiItemDescription = "test",
                OptiItemMinWeight = "1",
                OptiItemMaxWeight = "1",
                OptiMaxBuy = "5",
                OptiIntNotes = "!",
                OptiExtNotes = "=",
                OptiAssignToCarrier = "no"
            };
        }

        public static Environment Dev2()
        {
            return new Environment()
            {
                Headless = true,
                DBPath = "cdsdev2-db01.dev.echogl.net\\db01",
                TlrgURL = "http://cdsdev2-pltweb01.dev.echogl.net/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://cdsdev2-www01.dev.echogl.net/Optimizer/",
                OptiUser = "Automation",
                OptiPassword = "Test1234",
                OptiCustomerId = "E129651",
                OptiMode = "TL",
                OptiExpectedRevenue = "10",
                OptiExpectedCost = "10",
                OptiCargoValue = "1",
                OptiTenderType = "Routing Guide",
                OptiPickWarehouseG = "G12000",
                OptiPickStartDate = "08/18/2018 9:00",
                OptiPickEndDate = "08/18/2018 17:00",
                OptiDropWarehouseG = "G120004",
                OptiDropStartDate = "08/19/2018 9:00",
                OptiDropEndDate = "08/19/2018 17:00",
                OptiEquipment = "van",
                OptiItemDescription = "test",
                OptiItemMinWeight = "1",
                OptiItemMaxWeight = "1",
                OptiMaxBuy = "5",
                OptiIntNotes = "!",
                OptiExtNotes = "=",
                OptiAssignToCarrier = "no"
            };
        }

        public static Environment Qa1()
        {
            return new Environment()
            {
                Headless = true,
                DBPath = "qa1-db01.qa.echogl.net\\db01",
                TlrgURL = "http://qa1_tlrg.echo.com/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://qa1.echo.com/Optimizer/",
                OptiUser = "Automation",
                OptiPassword = "Test1234",
                OptiCustomerId = "E129651",
                OptiMode = "TL",
                OptiExpectedRevenue = "10",
                OptiExpectedCost = "10",
                OptiCargoValue = "1",
                OptiTenderType = "Routing Guide",
                OptiPickWarehouseG = "G12000",
                OptiPickStartDate = "08/18/2018 9:00",
                OptiPickEndDate = "08/18/2018 17:00",
                OptiDropWarehouseG = "G120004",
                OptiDropStartDate = "08/19/2018 9:00",
                OptiDropEndDate = "08/19/2018 17:00",
                OptiEquipment = "van",
                OptiItemDescription = "test",
                OptiItemMinWeight = "1",
                OptiItemMaxWeight = "1",
                OptiMaxBuy = "5",
                OptiIntNotes = "!",
                OptiExtNotes = "=",
                OptiAssignToCarrier = "no"
            };
        }

        public static Environment Qa2()
        {
            return new Environment()
            {
                Headless = true,
                DBPath = "qa2-db01.qa.echogl.net\\db01",
                TlrgURL = "http://qa2_tlrg.echo.com/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://qa2.echo.com/Optimizer/",
                OptiUser = "Automation",
                OptiPassword = "Test1234",
                OptiCustomerId = "E129651",
                OptiMode = "TL",
                OptiExpectedRevenue = "10",
                OptiExpectedCost = "10",
                OptiCargoValue = "1",
                OptiTenderType = "Routing Guide",
                OptiPickWarehouseG = "G12000",
                OptiPickStartDate = "08/18/2018 9:00",
                OptiPickEndDate = "08/18/2018 17:00",
                OptiDropWarehouseG = "G120004",
                OptiDropStartDate = "08/19/2018 9:00",
                OptiDropEndDate = "08/19/2018 17:00",
                OptiEquipment = "van",
                OptiItemDescription = "test",
                OptiItemMinWeight = "1",
                OptiItemMaxWeight = "1",
                OptiMaxBuy = "5",
                OptiIntNotes = "!",
                OptiExtNotes = "=",
                OptiAssignToCarrier = "no"
            };
        }

        public static Environment Qa3()
        {
            return new Environment()
            {
                Headless = true,
                DBPath = "qa3-db01.qa.echogl.net\\db01",
                TlrgURL = "http://qa3_tlrg.echo.com/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://qa3.echo.com/Optimizer/",
                OptiUser = "Automation",
                OptiPassword = "Test1234",
                OptiCustomerId = "E129651",
                OptiMode = "TL",
                OptiExpectedRevenue = "10",
                OptiExpectedCost = "10",
                OptiCargoValue = "1",
                OptiTenderType = "Routing Guide",
                OptiPickWarehouseG = "G12000",
                OptiPickStartDate = "08/18/2018 9:00",
                OptiPickEndDate = "08/18/2018 17:00",
                OptiDropWarehouseG = "G120004",
                OptiDropStartDate = "08/19/2018 9:00",
                OptiDropEndDate = "08/19/2018 17:00",
                OptiEquipment = "van",
                OptiItemDescription = "test",
                OptiItemMinWeight = "1",
                OptiItemMaxWeight = "1",
                OptiMaxBuy = "5",
                OptiIntNotes = "!",
                OptiExtNotes = "=",
                OptiAssignToCarrier = "no"
            };
        }
    }
}
