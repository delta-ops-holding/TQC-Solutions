using DatabaseAccess.Models;
using DataClassLibrary.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories.Interfaces
{
    public interface ILogRepository
    {
        Task CreateLog(LogMessage logMessage);

        Task<IEnumerable<LogModel>> GetLatest();

        Task CreateLog(LogModel logModel);
    }
}
