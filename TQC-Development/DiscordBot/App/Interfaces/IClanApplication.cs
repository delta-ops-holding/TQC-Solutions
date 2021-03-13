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
        /// <param name="socketReaction">Contains the reaction which was added by the user.</param>
        /// <returns>An asynchronous operation.</returns>
        Task ProcessClanApplicationAsync(SocketReaction socketReaction);
    }
}
