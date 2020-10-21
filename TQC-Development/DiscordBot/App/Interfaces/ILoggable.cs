using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    /// <summary>
    /// Able to Log messages to the console.
    /// </summary>
    public interface ILoggable
    {
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="logMessage">Used to provide the message to log.</param>
        /// <returns>A Completed Task.</returns>
        Task Log(LogMessage logMessage);
    }
}
