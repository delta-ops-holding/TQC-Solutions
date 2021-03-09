using DatabaseAccess.Database.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace DatabaseAccess.Database
{
    public class SqlDatabase : IDatabase
    {
        private readonly string _connectionString = string.Empty;
        private readonly SqlConnection _sqlConnection = null;

        public SqlDatabase(IConfiguration configuration)
        {
            if (_sqlConnection == null)
            {
                if (configuration != null)
                {
                    _connectionString = configuration.GetConnectionString("ApiDb");

                    if (!string.IsNullOrEmpty(_connectionString))
                        _sqlConnection = new SqlConnection(_connectionString);
                    else
                        throw new Exception($"Server configuration error.");
                }
                else
                    throw new Exception($"Server error.");
            }
        }

        public SqlConnection GetConnection()
        {
            return _sqlConnection;
        }

        public void CloseConnection()
        {
            try
            {

                if (_sqlConnection != null)
                {
                    if (_sqlConnection.State.Equals(ConnectionState.Closed))
                    {
                        return;
                    }

                    _sqlConnection.Close();
                }
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
                {
                    if (_sqlConnection.State.Equals(ConnectionState.Open))
                    {
                        return;
                    }

                    await _sqlConnection.OpenAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void Dispose()
        {
            Debug.WriteLine($"##### Should Dispose DB Object #####");
        }
    }
}