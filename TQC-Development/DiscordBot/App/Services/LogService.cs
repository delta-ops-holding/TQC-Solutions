using Discord;
using Discord.WebSocket;
using DiscordBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    /// <summary>
    /// Represents a service, for handling Logging.
    /// </summary>
    public class LogService : ILogger
    {
        private readonly DiscordSocketClient _client;

        public LogService(DiscordSocketClient client)
        {
            _client = client;
        }

        /// <summary>
        /// Log to Console.
        /// </summary>
        /// <param name="logMessage">The message to log.</param>
        /// <returns>A Completed Task.</returns>
        public Task ConsoleLog(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.ToString());

            return Task.CompletedTask;
        }
    }
}
