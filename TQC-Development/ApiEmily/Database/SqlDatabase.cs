using ApiEmily.Database.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiEmily.Database
{
    public class SqlDatabase : IDatabase
    {
        #region Fields
        private static SqlDatabase _sqlInstance = null;
        private static readonly object _sqlDataLock = new object();
        private readonly string _connectionString = string.Empty;
        private readonly SqlConnection _sqlConnection = null;
        #endregion

        #region Properties
        public static SqlDatabase SqlInstance
        {
            get
            {
                lock (_sqlDataLock)
                {
                    if (_sqlInstance == null) _sqlInstance = new SqlDatabase();

                    return _sqlInstance;
                }
            }
        }
        #endregion

        #region Constructors
        private SqlDatabase()
        {
            if (_sqlConnection == null)
            {
                ConnectionStringSettings settings = ConfigurationManager.ConnectionStrings["ApiDb"];

                if (settings != null)
                {
                    _connectionString = settings.ConnectionString;

                    if (!string.IsNullOrEmpty(_connectionString))
                        _sqlConnection = new SqlConnection(_connectionString);
                    else
                        throw new Exception($"Server configuration error.");
                }
                else
                    throw new Exception($"Server error.");
            }
        }
        #endregion

        public SqlConnection GetConnection()
        {
            return _sqlConnection;
        }

        public void CloseConnection()
        {
            try
            {
                if (_sqlConnection != null)
                    _sqlConnection.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task OpenConnectionAsync()
        {

            try
            {
                if (_sqlConnection != null)
                    await _sqlConnection.OpenAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
        }
    }
}