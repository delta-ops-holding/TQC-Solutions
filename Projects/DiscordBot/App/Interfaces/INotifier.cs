using DataClassLibrary.Enums;
using DataClassLibrary.Models;
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
        /// <param name="message">A struct representing the message.</param>
        /// <param name="discordUser">A generic representing the discord user.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SendApplicationAsync(MessageModel message);

        /// <summary>
        /// Sends a message directly to the Discord user.
        /// </summary>
        /// <param name="discordUser">The user to direct message.</param>
        /// <param name="message">Contains the message to send.</param>
        Task SendDirectMessageToUserAsync(IUser discordUser, string message);
    }
}
