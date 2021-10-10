using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace LeadershipMinion.Logical.Data.Abstractions
{
    public interface IApplicationHandler
    {
        /// <summary>
        /// Creates a new clan application asynchronously.
        /// </summary>
        /// <param name="reactionAdded">A Websocket-based object from Discord.</param>
        /// <param name="userWhoReacted">A generic user, of who reacted to the message.</param>
        /// <returns>A Task representing the process asynchronously.</returns>
        Task HandleApplicationAsync(SocketReaction reactionAdded, IUser userWhoReacted);
    }
}