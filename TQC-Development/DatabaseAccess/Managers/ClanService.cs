using DatabaseAccess.Models;
using DatabaseAccess.Repositories;
using DatabaseAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccess.Managers
{
    public class ClanService
    {
        public async Task<IEnumerable<Guild>> GetClansAsync()
        {
            var result = await ((IClanRepository)new ClanRepository()).GetAllAsync();

            return result;
        }

        public async Task<IEnumerable<ClanPlatform>> GetClanPlatformsAsync()
        {
            var result = await ((IClanPlatformRepository)new ClanPlatformRepository()).GetAllAsync();

            return result;
        }

        public async Task<IEnumerable<Member>> GetMembersAsync()
        {
            var result = await ((IMemberRepository)new ClanMemberRepository()).GetAllAsync();

            return result;
        }

        public async Task<Guild> GetClanAsync(uint identifier)
        {
            var result = await ((IClanRepository)new ClanRepository()).GetAsync(identifier);

            return result;
        }

        public async Task<ClanPlatform> GetClanPlatformAsync(uint identifier)
        {
            var result = await ((IClanPlatformRepository)new ClanPlatformRepository()).GetAsync(identifier);

            return result;
        }

        public async Task<IEnumerable<Member>> GetMembersAsync(uint identifier)
        {
            var result = await ((IMemberRepository)new ClanMemberRepository()).GetByIdAsync(identifier);

            return result;
        }
    }
}