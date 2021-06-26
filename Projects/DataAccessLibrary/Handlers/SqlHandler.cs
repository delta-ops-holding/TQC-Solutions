using DataAccessLibrary.Handlers.Helpers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLibrary.Handlers
{
    public class SqlHandler : ISqlHandler
    {
        private readonly IConfiguration _configuration;

        public SqlHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            return await SqlHelper.ExecuteNonQueryAsync(_configuration.GetConnectionString("TestDb"), commandText, commandType, parameters);
        }

        public async Task<SqlDataReader> ExecuteReaderAsync(string commandText, CommandType commandType, params SqlParameter[] parameters)
        {
            return await SqlHelper.ExecuteReaderAsync(_configuration.GetConnectionString("TestDb"), commandText, commandType, parameters);
        }
    }
}
