using ApiEmily.Models;
using ApiEmily.Repositories;
using ApiEmily.Repositories.Interfaces;
using ApiEmily.Repositories.V3;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiEmily.Managers.V3
{
    public class ClanV3Service
    {
        public async Task<IEnumerable<Clan>> GetClansAsync()
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

        public async Task<Clan> GetClanAsync(uint identifier)
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
            var result = await ((IMemberRepository)new ClanMemberV3Repository()).GetAsync(identifier);

            return result;
        }
    }
}