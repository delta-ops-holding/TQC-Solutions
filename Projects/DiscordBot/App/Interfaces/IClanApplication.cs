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
        /// User applies to a clan application, given by the <see cref="SocketReaction"/> object.
        /// </summary>
        /// <param name="clanReactedTo">Represents the object that contains the necessary information about the reaction of a message.</param>
        /// <param name="userWhoReacted">The user who reacted to the message.</param>
        /// <returns>A Task representing the asynchronous process.</returns>
        Task ApplyToClanAsync(SocketReaction clanReactedTo, IUser userWhoReacted);
    }
}
