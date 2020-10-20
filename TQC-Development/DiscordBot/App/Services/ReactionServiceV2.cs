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
    public class ReactionServiceV2 : IReactable
    {
        private const uint DelayTimerInHours = 24;
        private static Stack<User> _temporaryRuntimeUsers = new Stack<User>(20);
        private readonly INotifiable _notifiable;
        private readonly ILoggable _loggable;

        public ReactionServiceV2(
            INotifiable notificationService,
            ILoggable logService
        )
        {
            _notifiable = notificationService;
            _loggable = logService;
        }

        public async Task ReactionAddedAsync(IUserMessage userMessage, SocketReaction reaction)
        {
            var startTime = DateTimeOffset.UtcNow;
            var clanName = DataService.GetClanName(reaction.Emote);

            if (_temporaryRuntimeUsers.Any(
                u =>
                    u.UserId == reaction.UserId &&
                    (startTime - u.Date).TotalHours <= DelayTimerInHours))
            {
                try
                {
                    await reaction.User.Value.SendMessageAsync($"Guardian. Wait for your clan application to proceed. You've already signed up for joining a delta clan.");
                }
                catch (HttpException)
                {
                    await _loggable.Log(new LogMessage(LogSeverity.Error, "Reaction Added", "Couldn't DM Guardian. [Privacy is on or sender is blocked]"));
                }
                await _loggable.Log(new LogMessage(LogSeverity.Warning, "Clan Application", $"Guardian aka <{reaction.UserId}> tried applying to more than one clan"));
            }
            else
            {
                _temporaryRuntimeUsers.Push(new User() { UserId = reaction.UserId, Date = DateTimeOffset.UtcNow });

                await _notifiable.NotifyUserAsync(reaction.User.Value, clanName);
                await _loggable.Log(new LogMessage(LogSeverity.Info, "Clan Application", $"Guardian aka <{reaction.UserId}> applied to join {clanName}"));

                // Switch on different channels.
                switch (reaction.Channel.Id)
                {
                    // Steam / PC | pc-clans
                    case 765277945194348544:
                        await _notifiable.NotifyAdminAsync(1, reaction.User.Value, clanName);
                        break;

                    // Playstation | ps4-clans
                    case 765277969278042132:
                        await _notifiable.NotifyAdminAsync(2, reaction.User.Value, clanName);
                        break;

                    // Xbox | xbox-clans
                    case 765277993454534667:
                        await _notifiable.NotifyAdminAsync(3, reaction.User.Value, clanName);
                        break;

                    // Debug Default.
                    default:
                        await _notifiable.NotifyAdminAsync(1, reaction.User.Value, clanName);
                        break;
                }
            }

            await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
        }
    }
}
