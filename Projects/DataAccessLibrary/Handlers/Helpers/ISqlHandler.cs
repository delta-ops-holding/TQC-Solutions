using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAccessLibrary.Handlers.Helpers
{
    public interface ISqlHandler
    {
        Task<int> ExecuteNonQueryAsync(string commandText, CommandType commandType, params SqlParameter[] parameters);

        Task<SqlDataReader> ExecuteReaderAsync(string commandText, CommandType commandType, params SqlParameter[] parameters);
    }
}
