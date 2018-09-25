using System;

namespace TlrgAutomation
{
    public class EnvironmentManager
    {
        public class Environment
        {
            public bool Headless;
            public string DBPath;
            public string TlrgURL;
            public string TlrgCustomerId;
            public string OptiURL;
            public string OptiUser;
            public string OptiPassword;
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
                Headless = Properties.RunSettings.Default.Run_Headless,
                DBPath = "tlrg-db01.dev.echogl.net\\db01",
                TlrgURL = "http://tlrg-pltweb01.dev.echogl.net/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://tlrg-www.dev.echogl.net/Optimizer/",
                OptiUser = "Automation",
                OptiPassword = "Test1234"
            };
        }

        public static Environment Dev2()
        {
            return new Environment()
            {
                Headless = Properties.RunSettings.Default.Run_Headless,
                DBPath = "cdsdev2-db01.dev.echogl.net\\db01",
                TlrgURL = "http://cdsdev2-pltweb01.dev.echogl.net/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://cdsdev2-www01.dev.echogl.net/Optimizer/",
                OptiUser = "Automation",
                OptiPassword = "Test1234"
            };
        }

        public static Environment Qa1()
        {
            return new Environment()
            {
                Headless = Properties.RunSettings.Default.Run_Headless,
                DBPath = "qa1-db01.qa.echogl.net",
                TlrgURL = "http://qa1_tlrg.echo.com/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://qa1.echo.com/Optimizer/",
                OptiUser = "Automation",
                OptiPassword = "Test1234"
            };
        }

        public static Environment Qa2()
        {
            return new Environment()
            {
                Headless = Properties.RunSettings.Default.Run_Headless,
                DBPath = "qa2-db01.qa.echogl.net",
                TlrgURL = "http://qa2_tlrg.echo.com/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://qa2.echo.com/Optimizer/",
                OptiUser = "Automation",
                OptiPassword = "Test1234"
            };
        }

        public static Environment Qa3()
        {
            return new Environment()
            {
                Headless = Properties.RunSettings.Default.Run_Headless,
                DBPath = "qa3-db01.qa.echogl.net",
                TlrgURL = "http://qa3_tlrg.echo.com/",
                TlrgCustomerId = "E9704",
                OptiURL = "http://qa3.echo.com/Optimizer/",
                OptiUser = "Automation",
                OptiPassword = "Test1234"
            };
        }
    }
}
