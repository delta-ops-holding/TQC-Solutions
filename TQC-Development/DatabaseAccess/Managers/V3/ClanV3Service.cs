using DatabaseAccess.Models;
using DatabaseAccess.Repositories;
using DatabaseAccess.Repositories.Interfaces;
using DatabaseAccess.Repositories.V3;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccess.Managers.V3
{
    public class ClanV3Service
    {
        public async Task<IEnumerable<Guild>> GetClansAsync()
        {
            var result = await ((IClanRepository)new ClanV3Repository()).GetAllAsync();

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
            var result = await ((IClanRepository)new ClanV3Repository()).GetAsync(identifier);

            return result;
        }

        public async Task<ClanPlatform> GetClanPlatformAsync(uint identifier)
        {
            var result = await ((IClanPlatformRepository)new ClanPlatformRepository()).GetAsync(identifier);

            return result;
        }

        public async Task<Member> GetMemberAsync(uint identifier)
        {
            var result = await ((IMemberRepository)new ClanFounderV3Repository()).GetAsync(identifier);

            return result;
        }
    }
}