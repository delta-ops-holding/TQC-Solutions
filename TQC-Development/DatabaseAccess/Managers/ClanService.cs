using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Managers.Interfaces;
using DatabaseAccess.Models;
using DatabaseAccess.Repositories;
using DatabaseAccess.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccess.Managers
{
    public class ClanService : IClanService
    {
        private IDatabase Database { get; }

        public ClanService(IDatabase database)
        {
            Database = database;
        }

        public async Task<IEnumerable<BaseEntity>> GetClansAsync()
        {
            var result = await ((IClanRepository)new ClanRepository(Database)).GetAllAsync();

            return result;
        }

        public async Task<IEnumerable<BaseEntity>> GetClanPlatformsAsync()
        {
            var result = await ((IClanPlatformRepository)new ClanPlatformRepository(Database)).GetAllAsync();

            return result;
        }

        public async Task<IEnumerable<BaseEntity>> GetMembersAsync()
        {
            var result = await ((IMemberRepository)new ClanMemberRepository(Database)).GetAllAsync();

            return result;
        }

        public async Task<BaseEntity> GetClanAsync(uint identifier)
        {
            var result = await ((IClanRepository)new ClanRepository(Database)).GetAsync(identifier);

            return result;
        }

        public async Task<BaseEntity> GetClanPlatformAsync(uint identifier)
        {
            var result = await ((IClanPlatformRepository)new ClanPlatformRepository(Database)).GetAsync(identifier);

            return result;
        }

        public async Task<IEnumerable<BaseEntity>> GetMembersAsync(uint identifier)
        {
            var result = await ((IMemberRepository)new ClanMemberRepository(Database)).GetByIdAsync(identifier);

            return result;
        }

        public Task<BaseEntity> GetMemberAsync(uint identifier)
        {
            throw new System.NotImplementedException();
        }
    }
}