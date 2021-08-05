using ObjectLibrary.Clan.Interfaces;
using ObjectLibrary.Core;
using System;

namespace ObjectLibrary.Clan
{
    public class ClanApplication : BaseEntity, IClanApplication
    {
        private readonly IClan _clan;
        private readonly Guid _applicationGuid;
        private readonly long _discordUserWhoAppliedSnowflakeId;
        private readonly DateTime _applicationRegistrationDateTime;
        private readonly DateTime _cooldownExpiryDateTime;

        public ClanApplication(int id, IClan clan, Guid applicationGuid, long discordUserWhoAppliedSnowflakeId, DateTime applicationRegistrationDateTime, DateTime cooldownExpiryDateTime) : base(id)
        {
            _clan = clan;
            _applicationGuid = applicationGuid;
            _discordUserWhoAppliedSnowflakeId = discordUserWhoAppliedSnowflakeId;
            _applicationRegistrationDateTime = applicationRegistrationDateTime;
            _cooldownExpiryDateTime = cooldownExpiryDateTime;
        }

        public IClan Clan { get { return _clan; } }
        public Guid ApplicationGuid { get { return _applicationGuid; } }
        public long DiscordUserAppliedSnowflakeId { get { return _discordUserWhoAppliedSnowflakeId; } }
        public DateTime ApplicationRegistrationDateTime { get { return _applicationRegistrationDateTime; } }
        public DateTime CooldownExpiryDateTime { get { return _cooldownExpiryDateTime; } }
    }
}
