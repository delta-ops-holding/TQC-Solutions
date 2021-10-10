using Discord;
using LeadershipMinion.Logical.Enums;

namespace LeadershipMinion.Logical.Models
{
    public class MessageModel
    {
        public MessageModel(string message, IUser discordUser, ApplicationModel application)
        {
            Message = message;
            DiscordUser = discordUser;
            Application = application;
        }

        //public MessageModel(string message, ClanPlatform clanPlatform, Clan clan, object discordUser, ulong discordUserId)
        //{
        //    Message = message;
        //    ClanPlatform = clanPlatform;
        //    Clan = clan;
        //    DiscordUser = discordUser;
        //    DiscordUserId = discordUserId;
        //}

        public string Message { get; set; }
        public IUser DiscordUser { get; set; }
        public ApplicationModel Application { get; set; }

        //public ClanPlatform ClanPlatform { get; set; }
        //public Clan Clan { get; set; }
        //public object DiscordUser { get; set; }
        //public ulong DiscordUserId { get; }
    }
}
