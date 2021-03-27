using DataClassLibrary.Enums;
using DataClassLibrary.Models;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Services;
using System;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public class NotificationService : INotifier
    {
        private readonly DiscordSocketClient _client;
        private readonly IDataService _dataService;
        private readonly ILogger _logger;

        /// <summary>
        /// Notification Service with required dependencies.
        /// </summary>
        /// <param name="client">Used to identify the client to use.</param>
        /// <param name="loggable">Used to log data.</param>
        /// <param name="dataService">To proceed the request, with the right data.</param>
        public NotificationService(DiscordSocketClient client, ILogger loggable, IDataService dataService)
        {
            _client = client;
            _logger = loggable;
            _dataService = dataService;
        }

        /// <summary>
        /// Notify Admin Channel.
        /// </summary>
        /// <param name="platformId">The platform identifier for which the notification should appear in.</param>
        /// <param name="discordUser">The user who is assigned to the notification.</param>
        /// <param name="clanName">Used to identify the assigned clan.</param>
        public async Task NotifyAdminAsync(byte platformId, IUser discordUser, Clan clanName)
        {
            try
            {
                // Channel to post to.
                //ulong channelId = 0; // To give a default value to catch on.
                ulong mainChannelId = 767474913308835880; // Channel on the admin server to post in.
                                                          //ulong debugChannelId = 768014874289766402; // Dev Server, to test bot.

                var pingRole = _dataService.GetRoleByClan(clanName);                            // Get the role to ping.
                var messageChannel = _client.GetChannel(mainChannelId) as IMessageChannel;      // Get Channel object by channel id.

                var embedMessage = new EmbedBuilder()                                           // Create new Embed Builder.
                {
                    Title = "New Clan Application Arrived!",
                    Description = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {clanName}. Confirmation message has also been sent to the Guardian."
                };

                // Switch on provided platform identifier.
                switch (platformId)
                {
                    case 1:
                        embedMessage.Color = Color.LighterGrey;
                        embedMessage.WithFooter($"{clanName}", "https://cdn.discordapp.com/emojis/641432631715561473.png?v=1").WithCurrentTimestamp();
                        break;
                    case 2:
                        embedMessage.Color = Color.Blue;
                        embedMessage.WithFooter($"{clanName}", "https://cdn.discordapp.com/emojis/551501319177895958.png?v=1").WithCurrentTimestamp();
                        break;
                    case 3:
                        embedMessage.Color = Color.Green;
                        embedMessage.WithFooter($"{clanName}", "https://cdn.discordapp.com/emojis/551501460202979328.png?v=1").WithCurrentTimestamp();
                        break;
                }

                // Check if the pinged role is empty.
                pingRole = string.IsNullOrEmpty(pingRole) ? $"Could not find Role for <{clanName}>" : pingRole;

                // Send embedded message to admins.
                await messageChannel.SendMessageAsync(
                    text: $"{pingRole}!",
                    embed: embedMessage.Build());
            }
            catch (Exception)
            {
                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Critical, "Notify Admins", "Error, while trying to notify admins.", "TQC Minion", DateTime.UtcNow));
            }
        }

        /// <summary>
        /// Notify User as Direct Message.
        /// </summary>
        /// <param name="discordUser">The User to direct message.</param>
        /// <param name="clanName">The clan name the user assigned to.</param>
        public async Task NotifyUserAsync(IUser discordUser, Clan clanName)
        {
            try
            {
                // Try Direct Message the user with a notification about the assignment.
                await discordUser.SendMessageAsync($"Hello Guardian. You're successfully signed up for the clan, {clanName}. Please await patiently for an admin to proceed your request.");
            }
            catch (HttpException)
            {
                // Log Error if could not message the user.
                _logger.ConsoleLog(new LogMessage(LogSeverity.Error, "Reaction Added", "Couldn't DM Guardian. [Privacy is on or sender is blocked]"));

                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Error, "Notify User", "Couldn't DM Guardian due to privacry reasons.", discordUser.Id.ToString(), DateTime.UtcNow));
            }
            catch (Exception)
            {
                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Critical, "Notify User", "Error, while trying to notify user.", "TQC Minion", DateTime.UtcNow));
            }
        }
    }
}
