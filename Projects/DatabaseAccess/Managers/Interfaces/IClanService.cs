using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Managers.Interfaces
{
    public interface IClanService
    {
        Task<IEnumerable<BaseEntity>> GetClansAsync();
        Task<IEnumerable<BaseEntity>> GetClanPlatformsAsync();
        Task<IEnumerable<BaseEntity>> GetMembersAsync();
        Task<BaseEntity> GetClanAsync(uint identifier);
        Task<BaseEntity> GetClanPlatformAsync(uint identifier);
        Task<BaseEntity> GetMemberAsync(uint identifier);
    }
}
