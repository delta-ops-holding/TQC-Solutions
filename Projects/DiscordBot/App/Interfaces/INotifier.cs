using DataClassLibrary.Enums;
using Discord;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    /// <summary>
    /// Provides the ability to notifiy.
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// Sends a notification about a new arrival of a clan application.
        /// </summary>
        /// <param name="platformId">Represents the platform of the applcation.</param>
        /// <param name="discordUser">Contains the user from Discord.</param>
        /// <param name="clan">A name of the clan which the application is for.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendApplicationAsync(byte platformId, IUser discordUser, Clan clan);

        /// <summary>
        /// Sends a message directly to the Discord user.
        /// </summary>
        /// <param name="discordUser">The user to direct message.</param>
        /// <param name="message">Contains the message to send.</param>
        Task SendDirectMessageToUserAsync(IUser discordUser, string message);
    }
}
