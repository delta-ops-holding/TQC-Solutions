using LeadershipMinion.Logical.Enums;
using System;

namespace LeadershipMinion.Logical.Models
{
    public record ApplicationModel(ulong DiscordUserId, DateTimeOffset RegistrationDate, Clan AppliedToClan, ClanPlatform OnPlatorm);
}
