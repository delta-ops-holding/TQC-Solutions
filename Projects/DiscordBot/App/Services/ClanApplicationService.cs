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
        private const double CLAN_APPLICATION_COOLDOWN = 24;

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

        public async Task ApplyToClanAsync(SocketReaction clanReactedTo, IUser userWhoReacted)
        {
            // Check if the parameters are valid.
            if (UserIsSpecifiedAndNotNull(clanReactedTo, userWhoReacted))
            {
                DateTimeOffset currentDateTime = DateTimeOffset.UtcNow;
                Clan clanName = _dataService.GetClanName(clanReactedTo.Emote);

                if (clanName == Clan.Undefined)
                {
                    string message = "Guardian, the clan no longer supports more application, or was simply not found. Reach out to an admin if you think this was an error.";

                    await _logger.LogAsync(new LogModel(LoggingSeverity.Debug, ToString(), $"Clan does not support any more application, or was not found..", $"{userWhoReacted.Id}", DateTime.UtcNow));
                    await _notifier.SendDirectMessageToUserAsync(userWhoReacted, message);

                    return;
                }

                // Check whether the user has an already processing clan application within 24 hours.
                if (UserHasAnExistingClanApplication(userWhoReacted, currentDateTime))
                {
                    UserModel existingApplication = _temporaryRuntimeUsers.FirstOrDefault(user => user.DiscordId == userWhoReacted.Id);

                    string userMessage = $"Guardian. Wait for your clan application to proceed. You've already signed up for joining {existingApplication.ClanApplication}.";

                    await _logger.LogAsync(new LogModel(LoggingSeverity.Debug, ToString(), $"Guardian tried applying to more than one clan.", $"{userWhoReacted.Id}", DateTime.UtcNow));
                    await _notifier.SendDirectMessageToUserAsync(userWhoReacted, userMessage);
                    return;
                }

                _temporaryRuntimeUsers.Push(new UserModel(Guid.NewGuid(), userWhoReacted.Id, DateTimeOffset.UtcNow, clanName));

                byte platformId = GetPlatformIdByReaction(clanReactedTo);
                MessageModel messageModel = new(string.Empty, platformId, clanName, userWhoReacted);

                await _logger.LogAsync(new LogModel(LoggingSeverity.Info, "Sent Clan Application", $"Guardian applied to join {clanName}.", $"{userWhoReacted.Id}", DateTime.UtcNow));
                await _notifier.SendApplicationAsync(messageModel);
                return;
            }

            await _logger.LogAsync(new LogModel(LoggingSeverity.Warning, ToString(), "User couldn't be found in the downloaded cache.", clanReactedTo.UserId.ToString(), DateTime.UtcNow));
        }

        internal static bool UserIsSpecifiedAndNotNull(SocketReaction clanReactedTo, IUser userWhoReacted)
        {
            return clanReactedTo.User.IsSpecified && userWhoReacted is not null;
        }

        internal static bool UserHasAnExistingClanApplication(IUser userWhoReacted, DateTimeOffset currentDateTime)
        {
            return _temporaryRuntimeUsers.Any(user => user.DiscordId == userWhoReacted.Id && (currentDateTime - user.Date).TotalHours <= CLAN_APPLICATION_COOLDOWN);
        }

        internal static byte GetPlatformIdByReaction(SocketReaction reaction)
        {
            return reaction.Channel.Id switch
            {
                // Steam / PC | pc-clans
                765277945194348544 => 1,
                // Playstation | ps4-clans
                765277969278042132 => 2,
                // Xbox | xbox-clan
                765277993454534667 => 3,
                _ => 0
            };
        }
    }
}
