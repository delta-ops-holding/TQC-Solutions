using LeadershipMinion.Logical.Enums;
using System;

namespace LeadershipMinion.Logical.Models
{
    //public record ApplicationModel(ulong DiscordUserId, DateTimeOffset RegistrationDate, Clan AppliedToClan, ClanPlatform OnPlatorm);

    public class ApplicationModel
    {
        public ApplicationModel(ulong discordUserId, DateTimeOffset registrationDate, Clan appliedToClan, ClanPlatform clanAssociatedWithPlatform)
        {
            DiscordUserId = discordUserId;
            RegistrationDate = registrationDate;
            AppliedToClan = appliedToClan;
            ClanAssociatedWithPlatform = clanAssociatedWithPlatform;
        }

        public ulong DiscordUserId { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public Clan AppliedToClan { get; set; }
        public ClanPlatform ClanAssociatedWithPlatform { get; set; }
    }
}
