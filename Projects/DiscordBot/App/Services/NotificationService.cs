using DataClassLibrary.Enums;
using DataClassLibrary.Models;
using Discord;
using Discord.Net;
using Discord.WebSocket;
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
        /// Send application.
        /// </summary>
        /// <param name="platformId">Describes the platform of which the application is from.</param>
        /// <param name="discordUser">Represents a user from Discord.</param>
        /// <param name="clan">A clan which has been applied to.</param>
        /// <returns></returns>
        public async Task SendApplicationAsync(byte platformId, IUser discordUser, Clan clan)
        {
            try
            {
                
                string clanApplicationArrivalMessage = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {clan}. Confirmation message has also been sent to the Guardian."; ;

                bool userHasPrivacySettingsOn = await NotifyUserAsync(discordUser, clan);

                if (userHasPrivacySettingsOn)
                {
                    clanApplicationArrivalMessage = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {clan}. Confirmation message could not be sent to the Guardian, due to privacy settings.";
                }

                await NotifyAdminAsync(platformId, clan, clanApplicationArrivalMessage);
            }
            catch (Exception)
            {
                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Critical, "Send Clan Application", "Error, while notfifying clan application arrival.", "TQC Minion", DateTime.UtcNow));
            }
        }

        /// <summary>
        /// Sends a direct message to the user.
        /// </summary>
        /// <param name="discordUser">Represents a user from Discord.</param>
        /// <param name="message">Contains the message for the user.</param>
        /// <returns></returns>
        public async Task SendDirectMessageToUserAsync(IUser discordUser, string message)
        {
            try
            {
                await discordUser.SendMessageAsync(message);
            }
            catch (HttpException)
            {
                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Warning, "User DM", "Couldn't DM Guardian, due to privacy reasons.", "TQC Minion", DateTime.UtcNow));
            }
        }

        /// <summary>
        /// Notify Admin Channel.
        /// </summary>
        /// <param name="platformId">The platform identifier for which the notification should appear in.</param>
        /// <param name="clanName">Used to identify the assigned clan.</param>
        /// <param name="message">A message to notify admins with.</param>
        private async Task NotifyAdminAsync(byte platformId, Clan clanName, string message)
        {
            try
            {
                // Channel to post to.
                //ulong channelId = 0; // To give a default value to catch on.
                //ulong debugChannelId = 768014874289766402; // Dev Server, to test bot.
                ulong mainChannelId = 767474913308835880; // Channel on the admin server to post in.

                // Get the role to ping.
                string pingRole = _dataService.GetRoleByClan(clanName);

                // Get Channel object by channel id.
                IMessageChannel messageChannel = _client.GetChannel(mainChannelId) as IMessageChannel;

                // Check if the pinged role is empty.
                pingRole = string.IsNullOrEmpty(pingRole) ? $"Could not find Role for <{clanName}>" : pingRole;

                Embed embeddedMessage = CreateEmbed(platformId, clanName, message);

                // Send embedded message to admins.
                await messageChannel.SendMessageAsync(text: $"{pingRole}!", embed: embeddedMessage);
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
        /// <returns>True if the user has privacy settings on, else false.</returns>
        private async Task<bool> NotifyUserAsync(IUser discordUser, Clan clanName)
        {
            try
            {
                // Try Direct Message the user with a notification about the assignment.
                await discordUser.SendMessageAsync($"Hello Guardian. You're successfully signed up for the clan, {clanName}. Please await patiently for an admin to proceed your request.");

                return false;
            }
            catch (HttpException)
            {
                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Error, "Notify User", "Couldn't DM Guardian due to privacry reasons.", discordUser.Id.ToString(), DateTime.UtcNow));
                return true;
            }
            catch (Exception)
            {
                await _logger.DatabaseLogAsync(new LogModel(LoggingSeverity.Critical, "Notify User", "Error, while trying to notify user.", "TQC Minion", DateTime.UtcNow));
                return true;
            }
        }

        /// <summary>
        /// Creates an embedded message.
        /// </summary>
        /// <param name="platformId">A platform id, represents the color of the embed.</param>
        /// <param name="clanName">Which clan the embed is for.</param>
        /// <param name="description">A message desription for the embed.</param>
        /// <returns></returns>
        private static Embed CreateEmbed(byte platformId, Clan clanName, string description)
        {
            // Create new Embed Builder.
            var embedMessage = new EmbedBuilder()
            {
                Title = "New Clan Application Arrived!",
                Description = description
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

            return embedMessage.Build();
        }
    }
}
