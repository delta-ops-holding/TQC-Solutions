using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Models;
using DatabaseAccess.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories
{
    public class DatabaseLogRepository : ILogRepository
    {
        private readonly IDatabase _databaseInstance;

        public DatabaseLogRepository(IDatabase database)
        {
            _databaseInstance = database;
        }

        public async Task CreateLog(LogMessage logMessage)
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_create_log",
                CommandType = CommandType.StoredProcedure,
                Connection = _databaseInstance.GetConnection()
            };

            c.Parameters.AddWithValue("@severity", logMessage.LogSeverity);
            c.Parameters.AddWithValue("@source", logMessage.Source);
            c.Parameters.AddWithValue("@message", logMessage.Message);
            c.Parameters.AddWithValue("@createdBy", logMessage.CreatedBy);
            c.Parameters.AddWithValue("@createdDate", logMessage.CreatedDate);

            try
            {
                await _databaseInstance.OpenConnectionAsync();
                await c.ExecuteNonQueryAsync();
            }
            finally { _databaseInstance.CloseConnection(); }
        }
    }
}
