using ObjectLibrary.Clan.Abstractions;
using ObjectLibrary.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Clan
{
    public class ClanApplication : BaseEntity, IClanApplication
    {
        private readonly int _clanId;
        private readonly Guid _applicationGuid;
        private readonly long _discordUserId;
        private readonly DateTime _applicationRegistered;
        private readonly DateTime _coolddownExpiry;

        public ClanApplication(int id, int clanId, Guid applicationGuid, long discordUserId, DateTime applicationRegistered, DateTime coolddownExpiry) : base(id)
        {
            _clanId = clanId;
            _applicationGuid = applicationGuid;
            _discordUserId = discordUserId;
            _applicationRegistered = applicationRegistered;
            _coolddownExpiry = coolddownExpiry;
        }

        public int ClanId => _clanId;

        public Guid ApplicationGuid => _applicationGuid;

        public long DiscordUserId => _discordUserId;

        public DateTime ApplicationRegistered => _applicationRegistered;

        public DateTime CoolddownExpiry => _coolddownExpiry;
    }
}
