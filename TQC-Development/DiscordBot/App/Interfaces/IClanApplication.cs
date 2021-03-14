using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    /// <summary>
    /// Provides access to methods that can be reacted to.
    /// </summary>
    public interface IClanApplication
    {
        /// <summary>
        /// Starts the process of a clan application.
        /// </summary>
        /// <param name="currentReaction">Contains the reaction which was added by the user.</param>
        /// <param name="currentUser">Current user, of which reacted to the message.</param>
        /// <returns>An asynchronous operation.</returns>
        Task ProcessClanApplicationAsync(SocketReaction currentReaction, IUser currentUser);
    }
}
