using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Enums;
using DiscordBot.Interfaces;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    /// <summary>
    /// Represents a service for handling Clan Applications.
    /// </summary>
    public class ClanApplicationService : IClanApplication
    {
        private const uint DelayTimerInHours = 24;

        private static readonly Stack<UserModel> _temporaryRuntimeUsers = new(20);
        private readonly IDataService _dataService;
        private readonly INotifier _notifier;
        private readonly ILogger _logger;

        /// <summary>
        /// Initialize an application service, with needed dependencies.
        /// </summary>
        /// <param name="notificationService">A notification service, which implements <see cref="INotifier"/>.</param>
        /// <param name="logService">A log service, which implements <see cref="ILogger"/>.</param>
        /// <param name="dataService">The data service, which gives the required data to process the request. [Cannot be null]</param>
        public ClanApplicationService(INotifier notificationService, ILogger logService, IDataService dataService)
        {
            _notifier = notificationService;
            _logger = logService;
            _dataService = dataService;
        }

        public async Task ProcessClanApplicationAsync(SocketReaction reaction)
        {
            if (reaction.User.IsSpecified)
            {
                await CreateClanApplicationAsync(reaction);
                return;
            }

            _logger.ConsoleLog(new LogMessage(LogSeverity.Info, "Process Clan Application", $"User was not found in downloaded cache <{reaction.UserId}>"));

            await _logger.DatabaseLogAsync(
                LogSeverity.Warning,
                "Process Clan Application",
                $"User was not found in downloaded cache.",
                $"{reaction.UserId}",
                DateTime.UtcNow);
        }

        /// <summary>
        /// Create the clan application.
        /// </summary>
        /// <param name="reaction"></param>
        /// <returns>An asynchronous operation.</returns>
        private async Task CreateClanApplicationAsync(SocketReaction reaction)
        {
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            Clan clanName = _dataService.GetClanName(reaction.Emote);
            IUser userObj = reaction.User.GetValueOrDefault();

            // !_temporaryRuntimeUsers.Any(u => u.DiscordId == reaction.UserId && (currentTime - u.Date).TotalHours <= DelayTimerInHours)
            if (CheckUserForAlreadyExistingClanApplication(currentTime, userObj))
            {
                _temporaryRuntimeUsers.Push(
                    new UserModel(Guid.NewGuid(), userObj.Id, DateTimeOffset.UtcNow, clanName));

                await SendClanApplicationAsync(reaction, clanName);
                return;
            }

            try
            {
                var userModel = _temporaryRuntimeUsers.FirstOrDefault(x => x.DiscordId == userObj.Id);

                await userObj.SendMessageAsync($"Guardian. Wait for your clan application to proceed. You've already signed up for joining {userModel.ClanApplication}.");
            }
            catch (HttpException)
            {
                _logger.ConsoleLog(new LogMessage(LogSeverity.Error, "User Privacy", "Couldn't DM Guardian. [Privacy is on or sender is blocked]"));
                await _logger.DatabaseLogAsync(LogSeverity.Warning, "Create Clan Application", "Couldn't DM Guardian, due to privacy reasons.", "TQC Minion", DateTime.UtcNow);
            }

            await UserAlreadyAppliedToClan(reaction, userObj);
        }

        private async Task UserAlreadyAppliedToClan(SocketReaction reaction, IUser userObj)
        {
            _logger.ConsoleLog(new LogMessage(LogSeverity.Warning, "Clan Application", $"Guardian aka <{reaction.UserId}> tried applying to more than one clan"));
            await _logger.DatabaseLogAsync(LogSeverity.Warning, "Create Clan Application", $"Guardian tried applying to more than one clan.", $"{userObj.Username}#{userObj.Discriminator}", DateTime.UtcNow);
        }

        private static bool CheckUserForAlreadyExistingClanApplication(DateTimeOffset currentTime, IUser userObj)
        {
            return !_temporaryRuntimeUsers.Any(u => u.DiscordId == userObj.Id && (currentTime - u.Date).TotalHours <= DelayTimerInHours);
        }

        /// <summary>
        /// Send the created clan application, to notify leaders.
        /// </summary>
        /// <param name="reaction"></param>
        /// <param name="clanName"></param>
        /// <returns>An asyncronous process containing the Task.</returns>
        private async Task SendClanApplicationAsync(SocketReaction reaction, Clan clanName)
        {
            IUser userObj = reaction.User.GetValueOrDefault();

            await _notifier.NotifyUserAsync(userObj, clanName);

            byte platformId = 0;

            switch (reaction.Channel.Id)
            {
                // Steam / PC | pc-clans
                case 765277945194348544:
                    platformId = 1;
                    break;

                // Playstation | ps4-clans
                case 765277969278042132:
                    platformId = 2;
                    break;

                // Xbox | xbox-clans
                case 765277993454534667:
                    platformId = 3;
                    break;
            }

            await _notifier.NotifyAdminAsync(platformId, userObj, clanName);

            _logger.ConsoleLog(new LogMessage(LogSeverity.Info, "Clan Application", $"Guardian aka <{userObj.Id}> applied to join {clanName}"));

            await _logger.DatabaseLogAsync(
                LogSeverity.Info,
                "Sent Clan Application",
                $"Guardian applied to join {clanName}.",
                $"{userObj.Username}#{userObj.Discriminator}",
                DateTime.UtcNow);
        }
    }
}
