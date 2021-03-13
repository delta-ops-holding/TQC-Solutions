using DatabaseAccess.Repositories.Interfaces;
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

        public Task DatabaseLogAsync(LogSeverity severity, string source, string message, string createdBy, DateTime createdDate)
        {
            _ = Task.Run(async () =>
            {
                var s = severity switch
                {
                    LogSeverity.Critical => 1,
                    LogSeverity.Error => 2,
                    LogSeverity.Warning => 3,
                    LogSeverity.Info => 4,
                    LogSeverity.Verbose => 5,
                    _ => 6
                };

                var log = new DatabaseAccess.Models.LogMessage(
                    logSeverity: (DatabaseAccess.Enums.LogSeverity)s,
                    source: source,
                    message: message,
                    createdBy: createdBy,
                    createdDate: createdDate);

                await _logRepo.CreateLog(log);
            }); 

            return Task.CompletedTask;
        }
    }
}
