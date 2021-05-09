using DatabaseAccess.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Managers.Interfaces
{
    public interface IClanService
    {
        Task<IEnumerable<Guild>> GetClansAsync();

        Task<IEnumerable<ClanPlatform>> GetClanPlatformsAsync();

        Task<IEnumerable<Member>> GetMembersAsync();

        Task<Guild> GetClanAsync(uint identifier);

        Task<ClanPlatform> GetClanPlatformAsync(uint identifier);

        Task<Member> GetMemberAsync(uint identifier);
    }
}
