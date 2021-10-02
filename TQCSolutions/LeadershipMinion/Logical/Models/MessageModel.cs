using LeadershipMinion.Logical.Enums;

namespace LeadershipMinion.Logical.Models
{
    public class MessageModel
    {
        public MessageModel(string message, ClanPlatform clanPlatform, Clan clan, object discordUser)
        {
            Message = message;
            ClanPlatform = clanPlatform;
            Clan = clan;
            DiscordUser = discordUser;
        }

        public string Message { get; set; }
        public ClanPlatform ClanPlatform { get; set; }
        public Clan Clan { get; set; }
        public object DiscordUser { get; set; }
    }
}
