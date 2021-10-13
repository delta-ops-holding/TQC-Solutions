using Discord;
using Discord.Net;
using Discord.Rest;
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
        private readonly DiscordRestClient _discordRestClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly IBotConfiguration _botConfiguration;
        private readonly IClanService _clanService;

        public NotificationService(DiscordSocketClient discordClient, DiscordRestClient discordRestClient, ILogger<NotificationService> logger, IBotConfiguration botConfiguration, IClanService clanService)
        {
            _discordClient = discordClient;
            _discordRestClient = discordRestClient;
            _logger = logger;
            _botConfiguration = botConfiguration;
            _clanService = clanService;
        }

        public async Task NotifyStaffsAsync(MessageModel model)
        {
            try
            {
                if (model.DiscordUser is not null)
                {
                    // Get the role to ping.
                    string pingRole = _clanService.GetMentionRoleByClanName(model.Application.AppliedToClan);

                    // Get Channel object by channel id.
                    IMessageChannel messageChannel = _discordClient.GetChannel(_botConfiguration.StaffChannel) as IMessageChannel;

                    // Check if the pinged role is empty.
                    pingRole = string.IsNullOrEmpty(pingRole) ? $"Could not find Role for <{model.Application.AppliedToClan}>" : pingRole;

                    Embed embeddedMessage = CreateEmbed(model);

                    // Send embedded message to admins.
                    await messageChannel.SendMessageAsync(text: $"{pingRole}! Welcome, <@{model.DiscordUser.Id}>", embed: embeddedMessage);
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
                model.Message = $"Hello Guardian. You're successfully signed up for {model.Application.AppliedToClan}. " +
                    $"Please await patiently for an admin to proceed your request. " +
                    $"Applying for more clans will not speed up the process.";

                if (model.DiscordUser is not null)
                {
                    await model.DiscordUser.SendMessageAsync(model.Message);
                    return false;
                }

                var socketUser = _discordClient.GetUser(model.Application.DiscordUserId);
                if (socketUser is not null)
                {
                    await socketUser.SendMessageAsync(model.Message);
                    return false;
                }

                var restUser = await _discordRestClient.GetUserAsync(model.DiscordUser.Id);
                if (restUser is not null)
                {
                    await restUser.SendMessageAsync(model.Message);
                    return false;
                }

                _logger.LogWarning($"User <{model.Application.DiscordUserId}> couldn't be notified. Was not found.");
                return true;
            }
            catch (HttpException httpEx)
            {
                // Check for Privacy Settings Discord Code.
                if (httpEx.DiscordCode == 50007)
                {
                    _logger.LogWarning($"User <{model.Application.DiscordUserId}> couldn't receive DM due to privacy settings.");
                    return true;
                }

                _logger.LogError(httpEx, $"Error occurred while notifying user; Code <{httpEx.DiscordCode}>, detailed message: {httpEx.Message} - reason: {httpEx.Reason}");
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

            var clan = model.Application.AppliedToClan;

            // Switch on provided platform identifier.
            switch (model.Application.ClanAssociatedWithPlatform)
            {
                case ClanPlatform.PC:
                    embedMessage.Color = Color.Red;
                    embedMessage.WithFooter($"{clan}", "https://cdn.discordapp.com/emojis/641432631715561473.png?v=1").WithCurrentTimestamp();
                    break;
                case ClanPlatform.PSN:
                    embedMessage.Color = Color.Blue;
                    embedMessage.WithFooter($"{clan}", "https://cdn.discordapp.com/emojis/551501319177895958.png?v=1").WithCurrentTimestamp();
                    break;
                case ClanPlatform.XBOX:
                    embedMessage.Color = Color.Green;
                    embedMessage.WithFooter($"{clan}", "https://cdn.discordapp.com/emojis/551501460202979328.png?v=1").WithCurrentTimestamp();
                    break;
            }

            return embedMessage.Build();
        }
    }
}
