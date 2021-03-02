using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    /// <summary>
    /// Provides access to methods that can be reacted to.
    /// </summary>
    public interface IApplication
    {
        /// <summary>
        /// Create a clan application by a user reaction.
        /// </summary>
        /// <param name="userMessage">The reaction message.</param>
        /// <param name="reaction">Used to identity the reaction data.</param>
        Task CreateClanApplicationAsync(IUserMessage userMessage, SocketReaction reaction);
    }
}
