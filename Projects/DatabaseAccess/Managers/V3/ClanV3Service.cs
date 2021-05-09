using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Managers.Interfaces;
using DatabaseAccess.Models;
using DatabaseAccess.Repositories;
using DatabaseAccess.Repositories.Interfaces;
using DatabaseAccess.Repositories.V3;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DatabaseAccess.Managers.V3
{
    public class ClanV3Service : IClanService
    {
        private IClanRepository ClanRepository { get; }
        private IClanPlatformRepository ClanPlatformRepository { get; }
        private IMemberRepository MemberRepository { get; }

        public ClanV3Service(IClanRepository clanRepository, IClanPlatformRepository clanPlatformRepository, IMemberRepository memberRepository)
        {
            ClanRepository = clanRepository;
            ClanPlatformRepository = clanPlatformRepository;
            MemberRepository = memberRepository;
        }

        public async Task<IEnumerable<Guild>> GetClansAsync()
        {
            var result = await ClanRepository.GetAllAsync();

            return result;
        }

        public async Task<IEnumerable<ClanPlatform>> GetClanPlatformsAsync()
        {
            var result = await ClanPlatformRepository.GetAllAsync();

            return result;
        }

        public async Task<IEnumerable<Member>> GetMembersAsync()
        {
            var result = await MemberRepository.GetAllAsync();

            return result;
        }

        public async Task<Guild> GetClanAsync(uint identifier)
        {
            var result = await ClanRepository.GetAsync(identifier);

            return result;
        }

        public async Task<ClanPlatform> GetClanPlatformAsync(uint identifier)
        {
            var result = await ClanPlatformRepository.GetAsync(identifier);

            return result;
        }

        public async Task<Member> GetMemberAsync(uint identifier)
        {
            var result = await MemberRepository.GetAsync(identifier);

            return result;
        }
    }
}