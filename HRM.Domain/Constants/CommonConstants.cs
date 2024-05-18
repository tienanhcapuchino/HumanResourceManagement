namespace HRM.Domain.Constants
{
    public static class ConfigurationKey
    {
        public const string HRMConnectionString = "HRMConnectStr";
        public const string MySqlConnectionString = "MySqlConnectStr";
    }
    public static class RoutesAPI_HR
    {
        public const string RootHM_APIUrl = "http://localhost:5247/api/";
    }
    public static class RoutesAPI_PR
    {
        public const string RootPR_APIUrl = "http://localhost:5097/api/";
        public const string GetAllNotifications = "notfication/getall";
        public const string GetEmployeesVacations = "openapi/hr/getallvacations";
    }
}
