using DataClassLibrary.Models;
using Discord;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    /// <summary>
    /// Provides access to log.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message to the console.
        /// </summary>
        /// <param name="logMessage">Used to provide the message to log.</param>
        void ConsoleLog(LogMessage logMessage);

        /// <summary>
        /// Logs a process into the database.
        /// </summary>
        /// <param name="logModel">Describes the information to be logged.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task LogAsync(LogModel logModel);
    }
}
