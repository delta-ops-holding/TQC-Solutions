﻿using Discord;
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
        private readonly IBasicConfiguration _basicConfiguration;
        private readonly IClanService _clanService;
        private readonly IEmbedService _embedService;

        public NotificationService(DiscordSocketClient discordClient, DiscordRestClient discordRestClient, ILogger<NotificationService> logger, IBasicConfiguration basicConfiguration, IClanService clanService, IEmbedService embedService)
        {
            _discordClient = discordClient;
            _discordRestClient = discordRestClient;
            _logger = logger;
            _basicConfiguration = basicConfiguration;
            _clanService = clanService;
            _embedService = embedService;
        }

        /// <summary>
        /// Sends an embedded message to a specific channel.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A task representing the asynchronous process, representing a notifcation being sent to the staff.</returns>
        public async Task NotifyStaffsAsync(MessageModel model)
        {
            try
            {
                if (model.DiscordUser is not null)
                {
                    _logger.LogDebug($"Notifying Staff with clan application for {model.Application.AppliedToClan}.");

                    // Get the role to ping.
                    string pingRole = _clanService.GetMentionRoleByClanName(model.Application.AppliedToClan);

                    // Get Channel object by channel id.
                    IMessageChannel messageChannel = _discordClient.GetChannel(_basicConfiguration.StaffChannel) as IMessageChannel;

                    // Check if the pinged role is empty.
                    pingRole = string.IsNullOrEmpty(pingRole) ? $"Could not find Role for <{model.Application.AppliedToClan}>" : pingRole;

                    // Send embedded message to admins.
                    var sentMsg = await messageChannel.SendMessageAsync(text: $"{pingRole}! Welcome, <@{model.DiscordUser.Id}>", embed: _embedService.BeautifyMessage(model));

                    return;
                }

                _logger.LogWarning("Staff were not notified with message, due to user being null.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        /// <summary>
        /// Sends a direct message to a user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A task representing the asynchronous process, representing a notifcation being sent to the user.</returns>
        public async Task<bool> NotifyUserAsync(MessageModel model)
        {
            try
            {
                if (model.DiscordUser is not null)
                {
                    _logger.LogDebug($"Notifying User <{model.DiscordUser.Id}> in DM's");

                    await model.DiscordUser.SendMessageAsync(model.Message);
                    return false;
                }

                var socketUser = _discordClient.GetUser(model.Application.DiscordUserId);
                if (socketUser is not null)
                {
                    _logger.LogDebug($"Notifying user <{socketUser.Id}> in DM's");
                    await socketUser.SendMessageAsync(model.Message);
                    return false;
                }

                var restUser = await _discordRestClient.GetUserAsync(model.DiscordUser.Id);
                if (restUser is not null)
                {
                    _logger.LogDebug($"Notifying user <{restUser.Id}> in DM's");
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
    }
}
