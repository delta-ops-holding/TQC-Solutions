using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Database.Interfaces
{
    public interface IDatabase : IDisposable
    {
        Task OpenConnectionAsync();

        void CloseConnection();
    }
}
