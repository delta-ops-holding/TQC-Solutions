using ApiEmily.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEmily.Repositories.Interfaces
{
    public interface IMemberRepository : IRepository<Member>
    {
        Task<IEnumerable<Member>> GetByIdAsync(uint identifier);
    }
}
