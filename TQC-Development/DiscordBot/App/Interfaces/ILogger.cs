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
    public interface ILogger
    {
        /// <summary>
        /// Logs a message.
        /// </summary>
        /// <param name="logMessage">Used to provide the message to log.</param>
        Task ConsoleLog(LogMessage logMessage);
    }
}
