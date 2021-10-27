using System;

namespace LeadershipMinion.Logical.Models
{
    /// <summary>
    /// Represents a model for applications.
    /// </summary>
    public class ApplicationModel
    {
        public ApplicationModel(ulong discordUserId, DateTimeOffset registrationDate, ClanDataModel clanData)
        {
            DiscordUserId = discordUserId;
            RegistrationDate = registrationDate;
            ClanData = clanData;
        }

        public ulong DiscordUserId { get; set; }
        public DateTimeOffset RegistrationDate { get; set; }
        public ClanDataModel ClanData { get; set; }
    }
}
