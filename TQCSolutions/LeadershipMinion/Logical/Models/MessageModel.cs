using LeadershipMinion.Logical.Enums;

namespace LeadershipMinion.Logical.Models
{
    public class MessageModel
    {
        public MessageModel(string message, ClanPlatform clanPlatform, Clan clan, object discordUser, ulong discordUserId)
        {
            Message = message;
            ClanPlatform = clanPlatform;
            Clan = clan;
            DiscordUser = discordUser;
            DiscordUserId = discordUserId;
        }

        public string Message { get; set; }
        public ClanPlatform ClanPlatform { get; set; }
        public Clan Clan { get; set; }
        public object DiscordUser { get; set; }
        public ulong DiscordUserId { get; }
    }
}
