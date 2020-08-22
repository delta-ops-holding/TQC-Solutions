using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Interfaces
{
    /// <summary>
    /// Functionality for Databases.
    /// </summary>
    public interface IDatabase
    {
        public void OpenConnection();

        public void CloseConnection();
    }
}
