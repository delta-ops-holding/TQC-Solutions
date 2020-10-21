using Discord;
using DiscordBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class LogService : ILoggable
    {
        /// <summary>
        /// Log to Console.
        /// </summary>
        /// <param name="logMessage">The message to log.</param>
        /// <returns>A Completed Task.</returns>
        public Task Log(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.ToString(padSource: 25));

            return Task.CompletedTask;
        }
    }
}
