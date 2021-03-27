using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Interfaces
{
    /// <summary>
    /// Functionality for Databases.
    /// </summary>
    public interface IDataAccess
    {
        public void OpenConnection();

        public void CloseConnection();
    }
}
