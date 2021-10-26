using Discord;

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

        public string Message { get; set; }
        public IUser DiscordUser { get; set; }
        public ApplicationModel Application { get; set; }
    }
}
