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

        public async Task NotifyStaffsAsync(MessageModel model)
        {
            try
            {
                if (model.DiscordUser is IUser discordUser)
                {
                    // Get the role to ping.
                    string pingRole = _clanService.GetMentionRoleByClanName(model.Clan);

                    // Get Channel object by channel id.
                    IMessageChannel messageChannel = _discordClient.GetChannel(_botConfiguration.StaffChannel) as IMessageChannel;

                    // Check if the pinged role is empty.
                    pingRole = string.IsNullOrEmpty(pingRole) ? $"Could not find Role for <{model.Clan}>" : pingRole;

                    Embed embeddedMessage = CreateEmbed(model);

                    // Send embedded message to admins.
                    await messageChannel.SendMessageAsync(text: $"{pingRole}! Welcome, <@{discordUser.Id}>", embed: embeddedMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task<bool> NotifyUserAsync(MessageModel model)
        {
            try
            {
                try
                {
                    if (model.DiscordUser is not null && model.DiscordUser is IUser genericUser)
                    {
                        await genericUser.SendMessageAsync(model.Message);

                        return false;
                    }

                    var socketUser = _discordClient.GetUser(model.DiscordUserId);

                    if (socketUser is not null)
                    {
                        await socketUser.SendMessageAsync(model.Message);

                        return false;
                    }

                    _logger.LogWarning($"User <{model.DiscordUserId}> couldn't be notified. Was not found.");
                    return true;
                }
                catch (HttpException httpEx)
                {
                    // Check for Privacy Settings Discord Code.
                    if (httpEx.DiscordCode == 50007)
                    {
                        return true;
                    }

                    _logger.LogError(httpEx, httpEx.Message);

                    return true;
                }
            }
            catch (Exception)
            {
                _logger.LogError($"Unknown Error occurred while notifying user.");

                return true;
            }
        }

        internal Embed CreateEmbed(MessageModel model)
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
