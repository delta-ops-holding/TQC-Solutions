using LeadershipMinion.Logical.Enums;
using System;

namespace LeadershipMinion.Logical.Models
{
    public class ApplicationModel
    {
        [Obsolete]
        public ApplicationModel(ulong discordUserId, DateTimeOffset registrationDate, Clan appliedToClan, ClanPlatform clanAssociatedWithPlatform)
        {
            DiscordUserId = discordUserId;
            RegistrationDate = registrationDate;
            AppliedToClan = appliedToClan;
            ClanAssociatedWithPlatform = clanAssociatedWithPlatform;
        }

        public ApplicationModel(ulong discordUserId, DateTimeOffset registrationDate, ClanDataModel clanData)
        {
            DiscordUserId = discordUserId;
            RegistrationDate = registrationDate;
            ClanData = clanData;
        }

        public ulong DiscordUserId { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public ClanDataModel ClanData { get; set; }

        [Obsolete]
        public Clan AppliedToClan { get; set; }
        [Obsolete]
        public ClanPlatform ClanAssociatedWithPlatform { get; set; }
    }
}
