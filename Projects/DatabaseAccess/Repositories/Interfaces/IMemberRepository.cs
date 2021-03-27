using DatabaseAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccess.Repositories.Interfaces
{
    public interface IMemberRepository : IRepository<Member>
    {
        Task<IEnumerable<Member>> GetByIdAsync(uint identifier);
    }
}
