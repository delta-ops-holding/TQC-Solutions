using DataClassLibrary.Models;
using Discord;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    /// <summary>
    /// Able to Log messages to the console.
    /// </summary>
    public interface ILogger
    {
        /// <summary>
        /// Logs a message to the console.
        /// </summary>
        /// <param name="logMessage">Used to provide the message to log.</param>
        void ConsoleLog(LogMessage logMessage);

        /// <summary>
        /// Saves a log in the database.
        /// </summary>
        /// <param name="logMessage">Information of the log.</param>
        /// <returns>A task representing the asynchronous process.</returns>
        Task DatabaseLogAsync(LogSeverity severity, string source, string message, string createdBy, DateTime createdDate);

        Task DatabaseLogAsync(LogModel logModel);
    }
}
