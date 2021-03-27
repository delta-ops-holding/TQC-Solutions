using DataClassLibrary.Enums;
using DataClassLibrary.Models;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Interfaces;
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

        public async Task ProcessClanApplicationAsync(SocketReaction currentReaction, IUser currentUser)
        {
            try
            {
                if (currentReaction.User.IsSpecified || currentUser != null)
                {
                    await CreateClanApplicationAsync(currentReaction, currentUser);
                    return;
                }

                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Warning, "Process Clan Application", "User was not found in downloaded cache.", currentReaction.UserId.ToString(), DateTime.UtcNow));

                _logger.ConsoleLog(new LogMessage(LogSeverity.Info, "Process Clan Application", $"User was not found in downloaded cache <{currentReaction.UserId}>"));
            }
            catch (Exception ex)
            {
                await _logger.DatabaseLogAsync(new LogModel(
                    LoggingSeverity.Error,
                    "Proccess Clan Application",
                    $"Error while creating clan application : {ex.Message}",
                    "TQC Minion",
                    DateTime.UtcNow));
            }
        }

        /// <summary>
        /// Create the clan application.
        /// </summary>
        /// <param name="currentReaction"></param>
        /// <returns>An asynchronous operation.</returns>
        private async Task CreateClanApplicationAsync(SocketReaction currentReaction, IUser currentUser)
        {
            try
            {
                var currentTime = DateTimeOffset.UtcNow;
                var clanName = _dataService.GetClanName(currentReaction.Emote);

                // !_temporaryRuntimeUsers.Any(u => u.DiscordId == reaction.UserId && (currentTime - u.Date).TotalHours <= DelayTimerInHours)
                if (CheckUserForAlreadyExistingClanApplication(currentTime, currentUser))
                {
                    _temporaryRuntimeUsers.Push(
                        new UserModel(Guid.NewGuid(), currentUser.Id, DateTimeOffset.UtcNow, clanName));

                    await SendClanApplicationAsync(currentReaction, currentUser, clanName);
                    return;
                }

                try
                {
                    var userModel = _temporaryRuntimeUsers.FirstOrDefault(x => x.DiscordId == currentUser.Id);

                    await currentUser.SendMessageAsync($"Guardian. Wait for your clan application to proceed. You've already signed up for joining {userModel.ClanApplication}.");
                }
                catch (HttpException)
                {
                    await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Warning, "Create Clan Application", "Couldn't DM Guardian, due to privacy reasons.", "TQC Minion", DateTime.UtcNow));
                    _logger.ConsoleLog(new LogMessage(LogSeverity.Error, "User Privacy", "Couldn't DM Guardian. [Privacy is on or sender is blocked]"));
                }

                await UserAlreadyAppliedToClan(currentUser);
            }
            catch (Exception ex)
            {
                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Error, "Create Clan Application", $"Error in creating- or sending clan application : {ex.Message}", "TQC Minion", DateTime.UtcNow));
            }
        }

        private async Task UserAlreadyAppliedToClan(IUser currentUser)
        {
            await _logger.DatabaseLogAsync(LogSeverity.Warning, "Create Clan Application", $"Guardian tried applying to more than one clan.", $"{currentUser.Id}", DateTime.UtcNow);

            _logger.ConsoleLog(new LogMessage(LogSeverity.Warning, "Clan Application", $"Guardian aka <{currentUser.Id}> tried applying to more than one clan"));
        }

        private static bool CheckUserForAlreadyExistingClanApplication(DateTimeOffset currentTime, IUser currentUser)
        {
            return !_temporaryRuntimeUsers.Any(u => u.DiscordId == currentUser.Id && (currentTime - u.Date).TotalHours <= DelayTimerInHours);
        }

        /// <summary>
        /// Send the created clan application, to notify leaders.
        /// </summary>
        /// <param name="reaction"></param>
        /// <param name="clanName"></param>
        /// <returns>An asyncronous process containing the Task.</returns>
        private async Task SendClanApplicationAsync(SocketReaction reaction, IUser currentUser, Clan clanName)
        {
            try
            {
                await _notifier.NotifyUserAsync(currentUser, clanName);

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

                await _notifier.NotifyAdminAsync(platformId, currentUser, clanName);

                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Info, "Sent Clan Application", $"Guardian applied to join {clanName}.", $"{currentUser.Id}", DateTime.UtcNow));

                _logger.ConsoleLog(new LogMessage(LogSeverity.Info, "Clan Application", $"Guardian aka <{currentUser.Id}> applied to join {clanName}"));
            }
            catch (Exception ex)
            {
                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Error, "Send Clan Application", $"Error while notifying users with clan application : {ex.Message}", "TQC Minion", DateTime.UtcNow));
            }
        }
    }
}
