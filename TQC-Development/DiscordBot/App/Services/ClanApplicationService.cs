using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Interfaces;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class ClanApplicationService : Interfaces.IApplication
    {
        private const uint DelayTimerInHours = 24;

        private static Stack<User> _temporaryRuntimeUsers = new Stack<User>(20);
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

        public async Task CreateClanApplicationAsync(IUserMessage userMessage, SocketReaction reaction)
        {
            DateTimeOffset currentTime = DateTimeOffset.UtcNow;
            Enums.ClanNames clanName = _dataService.GetClanName(reaction.Emote);

            if (!_temporaryRuntimeUsers.Any(u => u.UserId == reaction.UserId && (currentTime - u.Date).TotalHours <= DelayTimerInHours))
            {
                await SendClanApplicationAsync(reaction, clanName);

                return;
            }

            try
            {
                await reaction.User.Value.SendMessageAsync($"Guardian. Wait for your clan application to proceed. You've already signed up for joining a delta clan.");
            }
            catch (HttpException)
            {
                await _logger.Log(new LogMessage(LogSeverity.Error, "User Privacy", "Couldn't DM Guardian. [Privacy is on or sender is blocked]"));
            }

            await _logger.Log(new LogMessage(LogSeverity.Warning, "Clan Application", $"Guardian aka <{reaction.UserId}> tried applying to more than one clan"));
            await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
        }

        /// <summary>
        /// Notifies the people about the clan application.
        /// </summary>
        /// <param name="reaction"></param>
        /// <param name="clanName"></param>
        /// <returns>An asyncronous process containing the Task.</returns>
        private async Task SendClanApplicationAsync(SocketReaction reaction, Enums.ClanNames clanName)
        {
            _temporaryRuntimeUsers.Push(new User() { UserId = reaction.UserId, Date = DateTimeOffset.UtcNow });
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

            await _logger.Log(new LogMessage(LogSeverity.Info, "Clan Application", $"Guardian aka <{reaction.UserId}> applied to join {clanName}"));
        }
    }
}
