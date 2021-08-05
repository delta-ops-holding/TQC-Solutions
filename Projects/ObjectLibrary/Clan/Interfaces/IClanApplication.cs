using ObjectLibrary.Clan.Interfaces;
using System;

namespace ObjectLibrary.Clan.Interfaces
{
    public interface IClanApplication
    {
        Guid ApplicationGuid { get; }
        DateTime ApplicationRegistrationDateTime { get; }
        IClan Clan { get; }
        DateTime CooldownExpiryDateTime { get; }
        long DiscordUserAppliedSnowflakeId { get; }
    }
}