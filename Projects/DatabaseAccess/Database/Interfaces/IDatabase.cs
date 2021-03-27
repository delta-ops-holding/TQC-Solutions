using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace DatabaseAccess.Database.Interfaces
{
    public interface IDatabase : IDisposable
    {
        SqlConnection GetConnection();

        Task OpenConnectionAsync();

        void CloseConnection();
    }
}
