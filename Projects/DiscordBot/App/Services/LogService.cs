using DatabaseAccess.Repositories.Interfaces;
using DataClassLibrary.Enums;
using DataClassLibrary.Models;
using Discord;
using DiscordBot.Interfaces;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    /// <summary>
    /// Represents a service, for handling Logging.
    /// </summary>
    public class LogService : ILogger
    {
        private readonly ILogRepository _logRepo;

        public LogService(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        /// <summary>
        /// Log to Console.
        /// </summary>
        /// <param name="logMessage">The message to log.</param>
        /// <returns>A Completed Task.</returns>
        public void ConsoleLog(LogMessage logMessage)
        {
            Console.WriteLine(logMessage.ToString());
        }

        public async Task DatabaseLogAsync(LogSeverity severity, string source, string message, string createdBy, DateTime createdDate)
        {
            var s = severity switch
            {
                LogSeverity.Critical => 1,
                LogSeverity.Error => 2,
                LogSeverity.Warning => 3,
                LogSeverity.Info => 4,
                LogSeverity.Verbose => 5,
                LogSeverity.Debug => 6,
                _ => throw new NotSupportedException("Log Severity wasn't supported.")
            };

            var log = new DatabaseAccess.Models.LogMessage(
                logSeverity: (LoggingSeverity)s,
                source: source,
                message: message,
                createdBy: createdBy,
                createdDate: createdDate);

            await _logRepo.CreateLog(log);
        }

        public async Task DatabaseLogAsync(LogModel logModel)
        {
            await _logRepo.CreateLog(logModel);
        }
    }
}