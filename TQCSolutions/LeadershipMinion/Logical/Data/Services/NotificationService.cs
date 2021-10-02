using Discord;
using Discord.Net;
using Discord.WebSocket;
using LeadershipMinion.Core.Abstractions;
using LeadershipMinion.Logical.Data.Abstractions;
using LeadershipMinion.Logical.Enums;
using LeadershipMinion.Logical.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LeadershipMinion.Logical.Data.Services
{
    public class NotificationService : INotificationService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly IBotConfiguration _botConfiguration;
        private readonly IClanService _clanService;

        public NotificationService(ILogger<NotificationService> logger, IBotConfiguration botConfiguration, IClanService clanService, DiscordSocketClient discordClient)
        {
            _logger = logger;
            _botConfiguration = botConfiguration;
            _clanService = clanService;
            _discordClient = discordClient;
        }

        public async Task SendApplicationAsync(MessageModel model)
        {
            try
            {
                if (model.DiscordUser is IUser discordUser)
                {
                    model.Message = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {model.Clan}. Confirmation message has also been sent to the Guardian."; ;

                    bool userHasPrivacySettingsOn = await NotifyUserAsync(discordUser, model.Clan);

                    if (userHasPrivacySettingsOn)
                    {
                        model.Message = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {model.Clan}. Confirmation message could not be sent to the Guardian, due to privacy settings.";
                    }

                    await NotifyAdminAsync(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task SendDirectMessageToUserAsync(MessageModel model)
        {
            try
            {
                if (model.DiscordUser is IUser user)
                {
                    await user.SendMessageAsync(model.Message);
                }
            }
            catch (HttpException httpEx)
            {
                _logger.LogError(httpEx, httpEx.Message);
            }
        }

        private async Task NotifyAdminAsync(MessageModel message)
        {
            try
            {
                if (message.DiscordUser is IUser discordUser)
                {
                    // Get the role to ping.
                    string pingRole = _clanService.GetMentionRoleByClanName(message.Clan);

                    // Get Channel object by channel id.
                    IMessageChannel messageChannel = _discordClient.GetChannel(_botConfiguration.StaffChannel) as IMessageChannel;

                    // Check if the pinged role is empty.
                    pingRole = string.IsNullOrEmpty(pingRole) ? $"Could not find Role for <{message.Clan}>" : pingRole;

                    Embed embeddedMessage = CreateEmbed(message);

                    // Send embedded message to admins.
                    await messageChannel.SendMessageAsync(text: $"{pingRole}! Welcome, <@{discordUser.Id}>", embed: embeddedMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

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
            catch (HttpException httpEx)
            {
                _logger.LogError(httpEx, httpEx.Message);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                return true;
            }
        }

        private static Embed CreateEmbed(MessageModel model)
        {
            // Create new Embed Builder.
            var embedMessage = new EmbedBuilder()
            {
                Title = "New Clan Application Arrived!",
                Description = model.Message
            };

            // Switch on provided platform identifier.
            switch (model.ClanPlatform)
            {
                case ClanPlatform.PC:
                    embedMessage.Color = Color.Red;
                    embedMessage.WithFooter($"{model.Clan}", "https://cdn.discordapp.com/emojis/641432631715561473.png?v=1").WithCurrentTimestamp();
                    break;
                case ClanPlatform.PSN:
                    embedMessage.Color = Color.Blue;
                    embedMessage.WithFooter($"{model.Clan}", "https://cdn.discordapp.com/emojis/551501319177895958.png?v=1").WithCurrentTimestamp();
                    break;
                case ClanPlatform.XBOX:
                    embedMessage.Color = Color.Green;
                    embedMessage.WithFooter($"{model.Clan}", "https://cdn.discordapp.com/emojis/551501460202979328.png?v=1").WithCurrentTimestamp();
                    break;
            }

            return embedMessage.Build();
        }


    }
}
