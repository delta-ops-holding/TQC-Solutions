using DatabaseAccess.Repositories.Interfaces;
using DataClassLibrary.Models;
using Discord;
using DiscordBot.Interfaces;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    /// <summary>
    /// Represents a logging service.
    /// </summary>
    public class LogService : ILogger
    {
        private readonly ILogRepository _logRepo;

        public LogService(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        public void ConsoleLog(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.ToString());
        }

        public async Task LogAsync(LogModel logModel)
        {
            await _logRepo.CreateLog(logModel);
        }
    }
}