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
        public Task Log(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.ToString(padSource: 25));

            return Task.CompletedTask;
        }
    }
}
