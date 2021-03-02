﻿using Discord;
using DiscordBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class LogService : ILogger
    {
        /// <summary>
        /// Log to Console.
        /// </summary>
        /// <param name="logMessage">The message to log.</param>
        /// <returns>A Completed Task.</returns>
        public Task Log(LogMessage logMessage)
        {
            Console.WriteLine(logMessage);

            return Task.CompletedTask;
        }
    }
}
