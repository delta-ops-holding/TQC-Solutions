using Discord;
using Discord.Net;
using Discord.WebSocket;
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

        private static Stack<UserModel> _temporaryRuntimeUsers = new Stack<UserModel>(20);
        private readonly DataService _dataService;
        private readonly INotifier _notifier;
        private readonly ILogger _logger;

        /// <summary>
        /// Initialize an application service, with needed dependencies.
        /// </summary>
        /// <param name="notificationService">A notification service, which implements <see cref="INotifier"/>.</param>
        /// <param name="logService">A log service, which implements <see cref="ILogger"/>.</param>
        /// <param name="dataService">The data service, which gives the required data to process the request. [Cannot be null]</param>
        public ClanApplicationService(INotifier notificationService, ILogger logService, DataService dataService)
        {
            _notifier = notificationService;
            _logger = logService;
            _dataService = dataService;
        }

        public async Task ProcessClanApplicationAsync(IUserMessage userMessage, SocketReaction socketReaction)
        {
            if (socketReaction.User.IsSpecified)
            {
                await CreateClanApplicationAsync(userMessage, socketReaction);
            }

            await _logger.ConsoleLog(new LogMessage(LogSeverity.Info, "Process Clan Application", $"User was not found in downloaded cache <{socketReaction.UserId}>"));
        }

        /// <summary>
        /// Create the clan application.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="reaction"></param>
        /// <returns>An asynchronous operation.</returns>
        private async Task CreateClanApplicationAsync(IUserMessage userMessage, SocketReaction reaction)
        {
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            Enums.Clan clanName = _dataService.GetClanName(reaction.Emote);

            if (!_temporaryRuntimeUsers.Any(u => u.DiscordId == reaction.UserId && (currentTime - u.Date).TotalHours <= DelayTimerInHours))
            {
                _temporaryRuntimeUsers.Push(new UserModel() { Id = Guid.NewGuid(), DiscordId = reaction.UserId, Date = DateTimeOffset.UtcNow });
                await SendClanApplicationAsync(reaction, clanName);

                return;
            }

            try
            {
                await reaction.User.Value.SendMessageAsync($"Guardian. Wait for your clan application to proceed. You've already signed up for joining a delta clan.");
            }
            catch (HttpException)
            {
                await _logger.ConsoleLog(new LogMessage(LogSeverity.Error, "User Privacy", "Couldn't DM Guardian. [Privacy is on or sender is blocked]"));
            }

            await _logger.ConsoleLog(new LogMessage(LogSeverity.Warning, "Clan Application", $"Guardian aka <{reaction.UserId}> tried applying to more than one clan"));
            await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
        }        

        /// <summary>
        /// Send the created clan application, to notify leaders.
        /// </summary>
        /// <param name="reaction"></param>
        /// <param name="clanName"></param>
        /// <returns>An asyncronous process containing the Task.</returns>
        private async Task SendClanApplicationAsync(SocketReaction reaction, Enums.Clan clanName)
        {            
            await _notifier.NotifyUserAsync(reaction.User.Value, clanName);

            switch (reaction.Channel.Id)
            {
                // Steam / PC | pc-clans
                case 765277945194348544:
                    await _notifier.NotifyAdminAsync(1, reaction.User.Value, clanName);
                    break;

                // Playstation | ps4-clans
                case 765277969278042132:
                    await _notifier.NotifyAdminAsync(2, reaction.User.Value, clanName);
                    break;

                // Xbox | xbox-clans
                case 765277993454534667:
                    await _notifier.NotifyAdminAsync(3, reaction.User.Value, clanName);
                    break;
            }

            await _logger.ConsoleLog(new LogMessage(LogSeverity.Info, "Clan Application", $"Guardian aka <{reaction.UserId}> applied to join {clanName}"));
        }
    }
}
