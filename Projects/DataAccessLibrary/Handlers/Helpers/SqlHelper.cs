using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Handlers.Helpers
{
    public static class SqlHelper
    {
        private static readonly CommandBehavior _commandBehavior = CommandBehavior.CloseConnection;

        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            using SqlConnection conn = new(connectionString);

            using SqlCommand cmd = new()
            {
                CommandText = commandText,
                Connection = conn,
                CommandType = commandType
            };
            cmd.Parameters.AddRange(parameters);
            await conn.OpenAsync();

            return await cmd.ExecuteNonQueryAsync();
        }

        public static async Task<SqlDataReader> ExecuteReaderAsync(string connectionString, string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            SqlConnection conn = new(connectionString);
            SqlCommand cmd = new()
            {
                CommandText = commandText,
                Connection = conn,
                CommandType = commandType
            };

            cmd.Parameters.AddRange(parameters);
            await conn.OpenAsync();

            return await cmd.ExecuteReaderAsync(_commandBehavior);
        }
    }
}
