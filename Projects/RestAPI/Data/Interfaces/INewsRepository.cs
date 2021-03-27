using RestAPI.Data.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestAPI.Data.Interfaces
{
    /// <summary>
    /// Extends the <see cref="IRepository{T}"/>.
    /// </summary>
    public interface INewsRepository : IRepository<News>
    {
    }
}
