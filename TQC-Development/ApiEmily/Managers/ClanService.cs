using ApiEmily.Models;
using ApiEmily.Repositories;
using ApiEmily.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace ApiEmily.Managers
{
    public class ClanService
    {
        public async Task<IEnumerable<Clan>> GetClansAsync()
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

        public async Task<Clan> GetClanAsync(uint identifier)
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