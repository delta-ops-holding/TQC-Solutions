using RestAPI.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.DataAccess
{
    /// <summary>
    /// Handles connection to the database, implements <see cref="IDatabase"/>
    /// </summary>
    public class SqlDataAccess : IDataAccess, IDisposable

    {
        private static SqlDataAccess _instance = null;
        private static SqlConnection _sqlConnection = null;
        private static readonly object _lock = new object();
        private static string _connectionString = string.Empty;

        /// <summary>
        /// Contructor for the Database, initializes new SqlConnection uppon creation, and applies connection string.
        /// </summary>
        private SqlDataAccess()
        {
            lock (_lock)
            {
                if (string.IsNullOrEmpty(_connectionString)) _connectionString = @"Server=VIOLURREOT\DEVELOPMENT; Database=tqcdb; Integrated Security=true;";

                if (_sqlConnection == null) _sqlConnection = new SqlConnection(_connectionString);
            }
        }

        /// <summary>
        /// Instance of the database.
        /// </summary>
        public static SqlDataAccess Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null) _instance = new SqlDataAccess();

                    return _instance;
                }
            }
        }

        /// <summary>
        /// Get Connection.
        /// </summary>
        public SqlConnection GetSqlConnection
        {
            get
            {
                return _sqlConnection;
            }
        }

        /// <summary>
        /// Close sql connection.
        /// </summary>
        public void CloseConnection()
        {
            try
            {
                if (_sqlConnection.State == ConnectionState.Closed) return;

                if (_sqlConnection.State != ConnectionState.Broken) _sqlConnection.Close();

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Open sql connection.
        /// </summary>
        public void OpenConnection()
        {
            try
            {
                if (_sqlConnection.State == ConnectionState.Open) return;

                if (_sqlConnection.State != ConnectionState.Connecting) _sqlConnection.Open();

                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            ((IDisposable)_instance).Dispose();
        }
    }
}
