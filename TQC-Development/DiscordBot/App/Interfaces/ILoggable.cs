using Discord;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public interface ILoggable
    {
        Task Log(LogMessage logMessage);
    }
}
