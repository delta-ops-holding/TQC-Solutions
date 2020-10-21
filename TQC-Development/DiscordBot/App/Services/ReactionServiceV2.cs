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
        private const uint DelayTimerInHours = 24;                                      // Sets a 24 hour delay.

        private static Stack<User> _temporaryRuntimeUsers = new Stack<User>(20);        // A runtime stack, which can only contain 20 users.
        private readonly DataService _dataService;                                      // The data service, to provide data for the request.
        private readonly INotifiable _notifiable;                                       // To provide notifications to the request.
        private readonly ILoggable _loggable;                                           // To provide logging to the request.

        /// <summary>
        /// ReactionService Constructor with requried dependencies.
        /// </summary>
        /// <param name="notificationService">A notification service, which implements <see cref="INotifiable"/>.</param>
        /// <param name="logService">A log service, which implements <see cref="ILoggable"/>.</param>
        /// <param name="dataService">The data service, which gives the required data to process the request. [Cannot be null]</param>
        public ReactionServiceV2(INotifiable notificationService, ILoggable logService, DataService dataService)
        {
            _notifiable = notificationService;
            _loggable = logService;
            _dataService = dataService;
        }

        /// <summary>
        /// Assigns a user to a clan application, a user can only react once per 24 hours.
        /// </summary>
        /// <param name="userMessage">Used to remove the user's reaction, when request is complete.</param>
        /// <param name="reaction">Used to identify the reaction given by the user.</param>
        /// <returns></returns>
        public async Task ClanApplicationAsync(IUserMessage userMessage, SocketReaction reaction)
        {
            var currentDateTime = DateTimeOffset.UtcNow;
            var clanName = _dataService.GetClanName(reaction.Emote);

            // Check if the user has already assigned to a clan within 24 hours.
            if (_temporaryRuntimeUsers.Any(u => u.UserId == reaction.UserId && (currentDateTime - u.Date).TotalHours <= DelayTimerInHours))
            {
                try
                {
                    // Send message to user, if the user already assigned themself to a clan.
                    await reaction.User.Value.SendMessageAsync($"Guardian. Wait for your clan application to proceed. You've already signed up for joining a delta clan.");
                }
                catch (HttpException)
                {
                    // Log Message, if user couldn't be messaged.
                    await _loggable.Log(new LogMessage(LogSeverity.Error, "Reaction Added", "Couldn't DM Guardian. [Privacy is on or sender is blocked]"));
                }

                // Log Message, user tried assigning multiple clans.
                await _loggable.Log(new LogMessage(LogSeverity.Warning, "Clan Application", $"Guardian aka <{reaction.UserId}> tried applying to more than one clan"));
            }
            else
            {
                // Add user to stack, when assigned to clan.
                _temporaryRuntimeUsers.Push(new User() { UserId = reaction.UserId, Date = DateTimeOffset.UtcNow });

                // Notify user with the clan assignment.
                await _notifiable.NotifyUserAsync(reaction.User.Value, clanName);

                // Log Message, user has assigned to a delta clan.
                await _loggable.Log(new LogMessage(LogSeverity.Info, "Clan Application", $"Guardian aka <{reaction.UserId}> applied to join {clanName}"));

                // Switch on different channels, then notify admins.
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
                        //default:
                        //    await _notifiable.NotifyAdminAsync(1, reaction.User.Value, clanName);
                        //    break;
                }
            }

            // Remove User's reaction regardless if they are assigned, or tried to do multiple assignments.
            await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
        }
    }
}
