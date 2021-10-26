using Discord;

namespace LeadershipMinion.Logical.Models
{
    /// <summary>
    /// Reprents a model for message objects.
    /// </summary>
    public class MessageModel
    {
        public MessageModel(string message, IUser discordUser, ApplicationModel application)
        {
            Message = message;
            DiscordUser = discordUser;
            Application = application;
        }

        public MessageModel(string message, IUser discordUser)
        {
            Message = message;
            DiscordUser = discordUser;
        }

        public string Message { get; set; }
        public IUser DiscordUser { get; set; }
        public ApplicationModel Application { get; set; }
    }
}
