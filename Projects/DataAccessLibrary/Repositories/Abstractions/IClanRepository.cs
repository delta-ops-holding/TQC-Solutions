using ObjectLibrary.Clan;
using ObjectLibrary.Clan.Abstractions;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Repositories.Abstractions
{
    public interface IClanRepository : IRepository<IClan>
    {
        Task<int> AssignClanEmoteAsync(IClan clan, ClanEmote clanEmote, bool overrideExistingData = true);
        Task<int> RemoveClanEmoteAsync(IClan clan);
        Task<int> AssignClanMentionRoleAsync(IClan clan, MentionRole mentionRole, bool overrideExistingData = true);
        Task<int> RemoveClanMentionRoleAsync(IClan clan);
        Task<int> AssignClanAuthority(IClan clan, IMember member);
        Task<int> RemoveClanAuthority(IClan clan, IMember member);
    }
}
