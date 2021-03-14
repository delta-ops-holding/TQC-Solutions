using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task CreateLog(LogMessage logMessage);

        Task<IEnumerable<LogMessage>> GetLatest();
    }
}
