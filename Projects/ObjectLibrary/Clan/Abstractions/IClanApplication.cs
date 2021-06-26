using ObjectLibrary.Common.Abstractions;
using System;

namespace ObjectLibrary.Clan.Abstractions
{
    public interface IClanApplication : IBaseEntity
    {
        Guid ApplicationGuid { get; }
        DateTime ApplicationRegistered { get; }
        int ClanId { get; }
        DateTime CoolddownExpiry { get; }
        long DiscordUserId { get; }
    }
}