using DatabaseAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories.Interfaces
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<T> GetAsync(uint identifier);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
