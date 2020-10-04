using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace ApiEmily.Managers.Data
{
    public abstract class DataAccessManager
    {
        private readonly static string _connString = ConfigurationManager.ConnectionStrings["TqcDb"].ConnectionString;

        private readonly static string ServerName = Environment.GetEnvironmentVariable("ServerName");
        private readonly static string DatabaseScheme = Environment.GetEnvironmentVariable("DatabaseScheme");
        private readonly static string DbUserId = Environment.GetEnvironmentVariable("DbUserId");
        private readonly static string DbPassword = Environment.GetEnvironmentVariable("DbPassword");

        public static string GetConnectionString()
        {
            return 
                $"Server={ServerName};" +
                $"Database={DatabaseScheme};" +
                $"User Id={DbUserId};" +
                $"Password={DbPassword}";
        }

        public static string GetDatabaseConfigurationString(string dbName)
        {
            return ConfigurationManager.ConnectionStrings[dbName].ConnectionString;
        }
    }
}