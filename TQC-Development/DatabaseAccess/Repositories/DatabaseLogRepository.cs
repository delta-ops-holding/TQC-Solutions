using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Enums;
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

            c.Parameters.AddWithValue("@severity", (int)logMessage.LogSeverity);
            c.Parameters.AddWithValue("@source", logMessage.Source);
            c.Parameters.AddWithValue("@message", logMessage.Message);
            c.Parameters.AddWithValue("@createdBy", logMessage.CreatedBy);
            c.Parameters.AddWithValue("@createdDate", logMessage.CreatedDate).DbType = DbType.DateTime;

            try
            {
                await _databaseInstance.OpenConnectionAsync();
                await c.ExecuteNonQueryAsync();
            }
            catch (Exception) { throw; }
            finally { _databaseInstance.CloseConnection(); }
        }

        public async Task<IEnumerable<LogMessage>> GetLatest()
        {
            using SqlCommand c = new SqlCommand()
            {
                CommandText = "proc_get_latest_logs",
                CommandType = CommandType.StoredProcedure,
                Connection = _databaseInstance.GetConnection()
            };

            try
            {
                await _databaseInstance.OpenConnectionAsync();

                var dataReader = await c.ExecuteReaderAsync();

                List<LogMessage> logs = new List<LogMessage>();

                if (dataReader.HasRows)
                {
                    while (await dataReader.ReadAsync())
                    {
                        logs.Add(new LogMessage(
                             logSeverity: (LogSeverity)dataReader.GetInt32(1),
                             source: dataReader.GetString(2),
                             message: dataReader.GetString(3),
                             createdBy: dataReader.GetString(4),
                             createdDate: dataReader.GetDateTime(5)));
                    }
                }

                return logs;
            }
            finally
            {
                _databaseInstance.CloseConnection();
            }
        }
    }
}
