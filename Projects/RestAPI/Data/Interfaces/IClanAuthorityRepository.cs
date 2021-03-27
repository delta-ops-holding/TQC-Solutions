using RestAPI.Data.Objects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RestAPI.Data.Interfaces
{
    public interface IClanAuthorityRepository : IRepository<ClanAuthority>
    {
        new Task<IEnumerable<ClanAuthority>> GetById(int id);
    }
}
