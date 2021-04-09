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

        public async Task SendApplicationAsync(MessageModel message)
        {
            try
            {
                if (message.DiscordUser is IUser discordUser)
                {
                    message.Message = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {message.Clan}. Confirmation message has also been sent to the Guardian."; ;

                    bool userHasPrivacySettingsOn = await NotifyUserAsync(discordUser, message.Clan);

                    if (userHasPrivacySettingsOn)
                    {
                        message.Message = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {message.Clan}. Confirmation message could not be sent to the Guardian, due to privacy settings.";
                    }

                    await NotifyAdminAsync(message);
                }
            }
            catch (Exception)
            {
                await _logger.LogAsync(
                    new LogModel(
                        LoggingSeverity.Critical,
                        "Send Clan Application",
                        "Error, could not send clan application.",
                        "TQC Minion",
                        DateTime.UtcNow));
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
                await _logger.LogAsync(
                    new LogModel(
                        LoggingSeverity.Warning,
                        "User DM",
                        "Couldn't DM Guardian, due to privacy reasons.",
                        discordUser.Id.ToString(),
                        DateTime.UtcNow));
            }
        }

        /// <summary>
        /// Notify Admin Channel.
        /// </summary>
        /// <param name="message">A model representing the message to send.</param>
        private async Task NotifyAdminAsync(MessageModel message)
        {
            try
            {
                if (message.DiscordUser is IUser discordUser)
                {
                    // Channel to post to.
                    //ulong channelId = 0; // To give a default value to catch on.
                    //ulong debugChannelId = 768014874289766402; // Dev Server, to test bot.
                    ulong mainChannelId = 767474913308835880; // Channel on the admin server to post in.

                    // Get the role to ping.
                    string pingRole = _dataService.GetRoleByClan(message.Clan);

                    // Get Channel object by channel id.
                    IMessageChannel messageChannel = _client.GetChannel(mainChannelId) as IMessageChannel;

                    // Check if the pinged role is empty.
                    pingRole = string.IsNullOrEmpty(pingRole) ? $"Could not find Role for <{message.Clan}>" : pingRole;

                    Embed embeddedMessage = CreateEmbed(message);

                    // Send embedded message to admins.
                    await messageChannel.SendMessageAsync(text: $"{pingRole}! Welcome, <@{discordUser.Id}>", embed: embeddedMessage);
                }
            }
            catch (Exception)
            {
                await _logger.LogAsync(
                    new LogModel(
                        LoggingSeverity.Critical,
                        "Notify Admin",
                        "Error, clan application could not be sent in specific admin channel.",
                        "TQC Minion",
                        DateTime.UtcNow));
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
                await discordUser.SendMessageAsync(
                    $"Hello Guardian. You're successfully signed up for {clanName}. " +
                    $"Please await patiently for an admin to proceed your request. " +
                    $"Applying for more clans will not speed up the process.");

                return false;
            }
            catch (HttpException)
            {
                await _logger.LogAsync(
                    new LogModel(
                        LoggingSeverity.Error,
                        "Notify User",
                        "Couldn't DM Guardian due to privacry reasons.",
                        discordUser.Id.ToString(),
                        DateTime.UtcNow));

                return true;
            }
            catch (Exception)
            {
                await _logger.LogAsync(
                    new LogModel(
                        LoggingSeverity.Critical,
                        "Notify User",
                        "Error, could not DM the Guardian.",
                        "TQC Minion",
                        DateTime.UtcNow));

                return true;
            }
        }

        /// <summary>
        /// Creates an embedded message.
        /// </summary>
        /// <param name="message">A model representing the message to send.</param>
        /// <returns></returns>
        private static Embed CreateEmbed(MessageModel message)
        {
            // Create new Embed Builder.
            var embedMessage = new EmbedBuilder()
            {
                Title = "New Clan Application Arrived!",
                Description = message.Message
            };

            // Switch on provided platform identifier.
            switch (message.PlatformId)
            {
                case 1:
                    embedMessage.Color = Color.Red;
                    embedMessage.WithFooter($"{message.Clan}", "https://cdn.discordapp.com/emojis/641432631715561473.png?v=1").WithCurrentTimestamp();
                    break;
                case 2:
                    embedMessage.Color = Color.Blue;
                    embedMessage.WithFooter($"{message.Clan}", "https://cdn.discordapp.com/emojis/551501319177895958.png?v=1").WithCurrentTimestamp();
                    break;
                case 3:
                    embedMessage.Color = Color.Green;
                    embedMessage.WithFooter($"{message.Clan}", "https://cdn.discordapp.com/emojis/551501460202979328.png?v=1").WithCurrentTimestamp();
                    break;
            }

            return embedMessage.Build();
        }

        private static string CreateStylishMessage(string message, Clan clan, byte platformId)
        {
            string footerMessage = platformId switch
            {
                1 => $"{clan} https://cdn.discordapp.com/emojis/641432631715561473.png?v=1",
                2 => $"{clan} https://cdn.discordapp.com/emojis/551501319177895958.png?v=1",
                3 => $"{clan} https://cdn.discordapp.com/emojis/551501460202979328.png?v=1",
                _ => $"Couldn't get information."
            };

            var stylishMessage =
                $"New Clan Application Arrived!\n" +
                $"{message}\n" +
                $"{footerMessage}";

            return stylishMessage;
        }
    }
}
