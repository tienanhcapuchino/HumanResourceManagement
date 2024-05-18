﻿namespace HRM.Domain.Constants
{
    public static class ConfigurationKey
    {
        public const string HRMConnectionString = "HRMConnectStr";
        public const string MySqlConnectionString = "MySqlConnectStr";
    }
    public static class RoutesAPI_HR
    {
        public const string RootHM_APIUrl = "http://localhost:5247/";
    }
    public static class RoutesAPI_PR
    {
        public const string RootPR_APIUrl = "Http://localhost:5097/";
        public const string GetAllNotifications = "notfication/getall";
        public const string GetEmployeesVacations = "openapi/hr/getallvacations";
    }
}
