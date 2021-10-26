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
                    // Get the role to ping.
                    //string pingRole = _clanService.GetMentionRoleByClanName(model.Application.AppliedToClan);

                    // Get Channel object by channel id.
                    IMessageChannel messageChannel = _discordClient.GetChannel(_basicConfiguration.StaffChannel) as IMessageChannel;

                    // Check if the pinged role is empty.
                    string pingRole = $"<@&{model.Application.ClanData.MentionRoleId}>";
                    string ping = string.IsNullOrEmpty(pingRole) ? $"Could not find Role for <{model.Application.ClanData.Name}>" : pingRole;

                    // Send embedded message to admins.
                    var sentMsg = await messageChannel.SendMessageAsync(text: $"{ping}! Welcome, <@{model.DiscordUser.Id}>", embed: _embedService.BeautifyMessage(model));

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
                if (httpEx.DiscordCode == 50007)
                {
                    return true;
                }

                return true;
            }
        }
    }
}
