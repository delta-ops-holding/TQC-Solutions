using ObjectLibrary.Common.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Repositories.Abstractions
{
    public interface IRepository<TBaseEntity> where TBaseEntity : IBaseEntity
    {
        Task<int> CreateAsync(TBaseEntity entity);
        Task<int> UpdateAsync(TBaseEntity entity);
        Task<int> RemoveAsync(TBaseEntity entity);
        Task<TBaseEntity> GetByIdAsync(int id);
        Task<IEnumerable<TBaseEntity>> GetEntities();
    }
}
