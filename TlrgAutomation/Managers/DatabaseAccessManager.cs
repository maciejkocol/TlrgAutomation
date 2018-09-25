using NUnit.Framework;
using System;
using System.Data;
using System.Data.SqlClient;

namespace TlrgAutomation.Managers
{
    public class DatabaseAccessManager
    {

        public EnvironmentManager.Environment env;
        private static string DBConnectionString;

        public DatabaseAccessManager()
        {
            env = EnvironmentManager
                .GetEnvironment(TestContext.Parameters["TLRGEnv"] ?? Properties.RunSettings.Default.Run_Environment);
            DBConnectionString = "Data Source=" + env.DBPath + ";" +
            "Initial Catalog=XPlatform;" +
            "Integrated Security=True";
        }

        public static DataTable GetData(string query)
        {
            DataTable dataTable = new DataTable();
            // establish connection
            using (SqlConnection conn = new SqlConnection(DBConnectionString))
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    // create data adapter
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        // queries the database and returns the result to the datatable
                        da.Fill(dataTable);
                        conn.Close();
                        da.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            return dataTable;
        }

        public DataRow GetRandomCarrierRow()
        {
            return GetData(RandomCarrierQuery).Rows[0];
        }

        /**
         * Method to get a random Carrier record that has not been synced with TLRG.
         */
        public DataRow GetUnsyncedCarrierRow()
        {
            return GetData(UnsyncedCarrierQuery).Rows[0];
        }

        /**
         * Method to get a random Customer record that has not been synced with TLRG.
         */
        public DataRow GetUnsyncedCustomerRow()
        {
            return GetData(UnsyncedCustomerQuery).Rows[0];
        }

        /**
         * Method to get a specific Customer record that has been synced with TLRG.
         */
        public DataRow GetSyncedCustomerRow(String customerId)
        {
            string query = string.Format(SyncedCustomerQuery, customerId);
            return GetData(query).Rows[0];
        }

        /**
         * Method to get a random Customer record that has been synced with TLRG.
         */
        public DataRow GetRandomSyncedCustomerRow()
        {
            return GetData(RandomSyncedCustomerQuery).Rows[0];
        }

        /**
         * Method to get a random Carrier record that has been synced with TLRG.
         */
        public DataRow GetRandomSyncedCarrierRow()
        {
            return GetData(RandomSyncedCarrierQuery).Rows[0];
        }

        /**
         * Method to get a random Carrier record with SCAC.
         */
        public DataRow GetRandomCarrierWithScacRow()
        {
            return GetData(RandomCarrierWithScacQuery).Rows[0];

        }

        /**
         * Method to get contacts for a random Carrier.
         */
        public DataTable GetRandomCarrierContactRows()
        {
            return GetData(RandomCarrierContactQuery);
        }

        /**
         * Method to get a random Carrier record with Primary Email.
         */
        public DataRow GetRandomCarrierRowWithPrimaryEmail()
        {
            return GetData(RandomCarrierWithPrimaryEmailQuery).Rows[0];
        }

        public DataRow GetRandomCustomerHasLoadsRow()
        {
            return GetData(RandomCustomerHasLoadsQuery).Rows[0];
        }

        public DataRow GetRandomCustomerWithoutInheritSettings()
        {
            return GetData(RandomSyncedCustomerWithoutInheritSettings).Rows[0];
        }

        /**
         * Method to get all load records for a particular Customer.
         */
        public DataTable GetCustomerLoadsTable(string customerId)
        {
            string query = string.Format(CustomerLoadsQuery, customerId);
            return GetData(query);
        }

        /**
         * Method to get a specific load record for a particular Customer.
         */
        public DataRow GetCustomerLoadRow(string customerId, string loadId, string routeStatus)
        {
            string query = string.Format(LoadForCustomerQuery, loadId, customerId, routeStatus);
            return GetData(query).Rows[0];
        }

        /**
         * Method to get a specific load record with total distance.
         */
        public DataRow GetLoadDistanceRow(string loadId)
        {
            string query = string.Format(LoadDistanceQuery, loadId);
            return GetData(query).Rows[0];
        }

        /**
         * Method to get pending load records for a particular Customer.
         */
        public DataTable GetCustomerLoadsAutoStartTable(string customerId)
        {
            string query = string.Format(CustomerLoadsAutoStartQuery, customerId);
            return GetData(query);
        }

        public DataRow GetLoadGuidFromLoadId(string loadId)
        {
            string query = string.Format(GetLoadGuidFromIdQuery, loadId);
            return GetData(query).Rows[0];
        }

        /**
         * Method to get load detail records for a particular Customer.
         */
        public DataTable GetLoadDetailsTable(string customerId)
        {
            string query = string.Format(LoadDetailsQuery, customerId);
            return GetData(query);
        }

        public string RandomCarrierQuery => "SELECT TOP 1 * " +
                "FROM XPlatform.Carrier.Carrier " +
                "ORDER BY NEWID()";
        public string UnsyncedCarrierQuery => "SELECT TOP 1 t1.CarrierId " +
                "FROM EchoOptimizer.dbo.tblCarrier t1 " +
                "LEFT JOIN XPlatform.Carrier.Carrier t2 ON t2.CarrierId = t1.CarrierId " +
                "WHERE t2.CarrierId IS NULL " +
                "ORDER BY NEWID()";
        public string UnsyncedCustomerQuery => "SELECT TOP 1 t1.CustomerId, t2.IsTLRGEnabled " +
                "FROM EchoOptimizer.dbo.tblCustomer t1 " +
                "LEFT JOIN XPlatform.Customer.Customer t2 ON t2.CustomerId = t1.CustomerId " +
                "WHERE t2.IsTLRGEnabled IS NULL " +
                "ORDER BY NEWID()";
        public string SyncedCustomerQuery => "SELECT TOP 1 t1.CustomerId, t2.IsTLRGEnabled " +
                "FROM EchoOptimizer.dbo.tblCustomer t1 " +
                "LEFT JOIN XPlatform.Customer.Customer t2 ON t2.CustomerId = t1.CustomerId " +
                "WHERE t1.CustomerId = '{0}'";
        public string RandomSyncedCustomerQuery => "SELECT TOP 1 t1.CustomerId, t2.CustomerName " +
                "FROM EchoOptimizer.dbo.tblCustomer t1 " +
                "LEFT JOIN XPlatform.Customer.Customer t2 ON t2.CustomerId = t1.CustomerId " +
                "WHERE t2.IsTLRGEnabled IS NOT NULL " +
                "ORDER BY NEWID()";
        public string RandomSyncedCustomerWithoutInheritSettings => "SELECT TOP 1 t1.CustomerId, t2.CustomerName " +
                "FROM EchoOptimizer.dbo.tblCustomer t1 " +
                "LEFT JOIN XPlatform.Customer.Customer t2 ON t2.CustomerId = t1.CustomerId " +
                "WHERE t2.IsTLRGEnabled IS NOT NULL " +
                "AND OverrideParentSettings = '1'" +
                "ORDER BY NEWID()";
        public string RandomSyncedCarrierQuery => "SELECT TOP 1 t1.CarrierId, t2.CarrierName " +
                "FROM EchoOptimizer.dbo.tblCarrier t1 " +
                "LEFT JOIN XPlatform.Carrier.Carrier t2 ON t2.CarrierId = t1.CarrierId " +
                "WHERE t2.CarrierId IS NOT NULL " +
                "ORDER BY NEWID()";
        public string RandomCarrierWithScacQuery => "SELECT TOP 1 * " +
                "FROM XPlatform.Carrier.Carrier " +
                "WHERE NOT ISNULL(SCAC, '') = '' " +
                "ORDER BY NEWID()";
        public string RandomCarrierContactQuery => "SELECT TOP 1 t1.CarrierId, t2.* " +
                "FROM XPlatform.Carrier.Carrier t1 " +
                "LEFT JOIN XPlatform.Carrier.Contact t2 " +
                "ON t2.AggregateCarrierId = t1.Id " +
                "WHERE ISNULL(t2.title, '') = '' " +
                "ORDER BY NEWID()";
        public string RandomCarrierWithPrimaryEmailQuery => "SELECT TOP 1 t1.* " +
                "FROM XPlatform.Carrier.Carrier t1 " +
                "LEFT JOIN XPlatform.Carrier.Contact t2 " +
                "ON t2.AggregateCarrierId = t1.Id " +
                "WHERE t2.Title = 'PRIMARY' " +
                "ORDER BY NEWID()";
        public string RandomCustomerHasLoadsQuery =>
            "SELECT TOP 1 * " +
            "FROM XPlatform.Customer.Customer t1 " +
            "LEFT JOIN XPlatform.Routing.Load t2 on t2.CustomerId = t1.CustomerId " +
            "WHERE NOT ISNULL(t2.loadId, '') = '' " +
            "ORDER BY NEWID()";
        public string RandomLoadForCustomerQuery =>
            "SELECT TOP 1 * " +
            "FROM XPlatform.Customer.Customer t1 " +
            "LEFT JOIN XPlatform.Routing.Load t2 on t2.CustomerId = t1.CustomerId " +
            "WHERE NOT ISNULL(t2.loadId, '') = '' AND t1.CustomerId = '{0}'" +
            "ORDER BY NEWID()";
        public string LoadForCustomerQuery =>
            "SELECT TOP 1 t2.LoadId, t1.CustomerId, t3.RouteStatus " +
            "FROM XPlatform.Customer.Customer t1 " +
            "LEFT JOIN XPlatform.Routing.Load t2 on t2.CustomerId = t1.CustomerId " +
            "LEFT JOIN XPlatform.Routing.Routing t3 on t2.RoutingKey = t3.RoutingKey " +
            "WHERE t2.loadId = '{0}' AND t1.CustomerId = '{1}' AND t3.RouteStatus = '{2}'";
        public string CustomerLoadsQuery =>
            "SELECT DISTINCT TOP 10 t1.LoadId AS 'LOAD ID', " +
            "CASE " +
                "WHEN t2.CurrentCarrierName IS NULL THEN " +
                    "CASE " +
                        "WHEN t3.RoutingKey IS NULL THEN " +
                        "'No Routes Found' " +
                        "WHEN t2.RouteStatus = 'Not Started' THEN " +
                        "'Routing Not Started' " +
                        "WHEN t2.RouteStatus = 'In Progress' OR t2.RouteStatus = 'Cancelled' THEN " +
                        "'Ready for next carrier' " +
                        "WHEN t2.RouteStatus = 'Fall Through' THEN " +
                        "'' " +
                        "END " +
                "ELSE t2.CurrentCarrierName  + ' (' + t2.CurrentCarrierId + ')' " +
                "END AS 'CARRIER', " +
            "CASE " +
                "WHEN t2.CurrentCarrierRank LIKE '%out of%' " +
                    "THEN REPLACE(t2.CurrentCarrierRank, 'out ', '') " +
                        "WHEN t2.CurrentCarrierRank IS NULL " +
                            "THEN '0 of ' + CONVERT(VARCHAR(10), ( " +
                                "SELECT COALESCE(MAX(Rank), 0) " +
                                "FROM XPlatform.Routing.Route r1 " +
                                "WHERE r1.RoutingKey = t2.RoutingKey " +
                                ")) " +
                "END AS 'CARRIER RANK', " +
            "CASE " +
                "WHEN t2.CurrentCarrierCogs IS NULL " +
                    "THEN '$--' " +
                    "ELSE FORMAT(t2.CurrentCarrierCogs, '$####.0') " +
                "END AS 'COGS', " +
            "t2.RouteStatus AS 'STATUS', " +
            "t1.StartOriginCity AS 'ORIG CITY', " +
            "t1.StartOriginState AS 'ORIG ST', " +
            "t1.EndDestinationCity AS 'DEST CITY', " +
            "t1.EndDestinationState AS 'DEST ST', " +
            "FORMAT(t1.PickUpDate, 'MM/dd/yyyy') AS 'PU DATE' " +
            "FROM XPlatform.Routing.Load t1 " +
            "LEFT JOIN XPlatform.Routing.Routing t2 on t2.RoutingKey = t1.RoutingKey " +
            "LEFT JOIN XPlatform.Routing.Route t3 on t3.RoutingKey = t2.RoutingKey " +
            "WHERE t1.CustomerId = '{0}' AND t2.RouteStatus != 'Archived' " +
            "ORDER BY t1.LoadId DESC ";
        public string CustomerLoadsAutoStartQuery =>
            "SELECT DISTINCT t1.LoadId AS 'LOAD ID', " +
            "t1.StartOriginCity AS 'ORIG CITY', " +
            "t1.StartOriginState AS 'ORIG ST', " +
            "t1.StartOriginPostalCode AS 'ORIG ZIP', " +
            "t1.StartOriginCountry AS 'ORIG COUNTRY', " +
            "t1.EndDestinationCity AS 'DEST CITY', " +
            "t1.EndDestinationState AS 'DEST ST', " +
            "t1.EndDestinationPostalCode AS 'DEST ZIP', " +
            "t1.EndDestinationCountry AS 'DEST COUNTRY', " +
            "t1.Status AS 'STATUS' " +
            "FROM XPlatform.Load.Load t1 " +
            "WHERE t1.CustomerId = '{0}' AND (t1.Status = 'PendingUpdated' OR t1.Status = 'Pending') AND t1.SystemIsDeleted != '1' " +
            "ORDER BY t1.LoadId DESC ";
        public string LoadDetailsQuery =>
            "SELECT DISTINCT t3.Id, t3.CarrierName AS 'CARRIER NAME', FORMAT(t3.TotalCOGs, '$####.##') AS 'COGS', t3.CarrierId AS 'CARRIER ID' " +
            "FROM XPlatform.Routing.Route t1 " +
            "LEFT JOIN XPlatform.Routing.Routing t2 on t1.RoutingKey = t2.RoutingKey " +
            "LEFT JOIN XPlatform.Routing.Route t3 on t3.RoutingKey = t2.RoutingKey " +
            "WHERE t1.LoadId = '{0}' AND t2.RouteStatus != 'Archived' AND t3.BookedByRepGuid IS NOT NULL " +
            "ORDER BY t3.Id DESC ";
        public string LoadDistanceQuery =>
            "SELECT LoadId, Distance " +
            "FROM EchoOptimizer.dbo.tblloads " +
            "WHERE LoadId = '{0}' ";

        public string GetLoadGuidFromIdQuery =>
            "SELECT LoadGuId FROM EchoOptimizer.dbo.tblcustomerloads" +
            " WHERE LoadID = '{0}'";
    }
}
