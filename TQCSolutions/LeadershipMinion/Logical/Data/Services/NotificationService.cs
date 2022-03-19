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
    /// <summary>
    /// A class that notifies different types of clients.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly DiscordSocketClient _discordClient;
        private readonly DiscordRestClient _discordRestClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly IBasicConfiguration _basicConfiguration;
        private readonly IEmbedService _embedService;

        public NotificationService(DiscordSocketClient discordClient, DiscordRestClient discordRestClient, ILogger<NotificationService> logger, IBasicConfiguration basicConfiguration, IEmbedService embedService)
        {
            _discordClient = discordClient;
            _discordRestClient = discordRestClient;
            _logger = logger;
            _basicConfiguration = basicConfiguration;
            _embedService = embedService;
        }

        public async Task NotifyStaffsAsync(MessageModel model)
        {
            try
            {
                if (model.DiscordUser is not null)
                {
                    IMessageChannel channel = (_basicConfiguration.Environment == SystemEnvironment.Debug)
                        ? _discordClient.GetChannel(_basicConfiguration.DebugChannel) as IMessageChannel
                        : _discordClient.GetChannel(_basicConfiguration.StaffChannel) as IMessageChannel;

                    string pingRole = $"<@&{model.Application.ClanData.MentionRoleId}>";
                    string ping = string.IsNullOrEmpty(pingRole) ? $"Could not find Role for <{model.Application.ClanData.Name}>" : pingRole;

                    string msg = $"{ping}! Welcome, <@{model.DiscordUser.Id}>";

                    if (_basicConfiguration.Environment == SystemEnvironment.Debug)
                    {
                        msg = $"This is a demo, but Hello, <@{model.DiscordUser.Id}>";
                    }

                    var sentMsg = await channel.SendMessageAsync(text: msg, embed: _embedService.BeautifyMessage(model));

                    return;
                }

                _logger.LogWarning("Staff were not notified with message, due to user being null.");
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

                return true;
            }
            catch (HttpException httpEx)
            {
                // Check for Privacy Settings Discord Code.
                if (httpEx.DiscordCode == DiscordErrorCode.CannotSendMessageToUser)
                {
                    return true;
                }

                return true;
            }
        }
    }
}
