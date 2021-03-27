using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEmily.Database.Interfaces
{
    public interface IDatabase : IDisposable
    {
        Task OpenConnectionAsync();

        void CloseConnection();
    }
}
