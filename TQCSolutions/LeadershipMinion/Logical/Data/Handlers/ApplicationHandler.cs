﻿using Discord;
using Discord.WebSocket;
using LeadershipMinion.Core.Helpers;
using LeadershipMinion.Logical.Data.Abstractions;
using LeadershipMinion.Logical.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LeadershipMinion.Logical.Data.Handlers
{
    public class ApplicationHandler : IApplicationHandler
    {
        private readonly IClanService _clanService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ApplicationHandler> _logger;

        private static readonly Stack<ApplicationModel> _applicationsUnderCooldown = new(100);

        public ApplicationHandler(IClanService clanService, ILogger<ApplicationHandler> logger, INotificationService notificationService)
        {
            _clanService = clanService;
            _logger = logger;
            _notificationService = notificationService;
        }

        public async Task CreateApplicationAsync(SocketReaction appliedBy, IUser userWhoReacted)
        {
            if (UserIsSpecifiedAndUserWhoReactedIsNotNull(appliedBy, userWhoReacted))
            {
                DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;

                var emote = appliedBy.Emote as Emote;
                var clanName = _clanService.GetClanNameByEmoteId(emote.Id);
                var platform = _clanService.GetClanPlatformByChannelId(appliedBy.Channel.Id);

                if (HasApplicationCooldown(userWhoReacted, currentDateTimeOffset))
                {
                    var existingApplication = _applicationsUnderCooldown.FirstOrDefault(u => u.DiscordUserId == userWhoReacted.Id);
                    string message = $"Guardian. Be patient. You've already applied to join {existingApplication.AppliedToClan}. Please wait for it to proceed.";

                    var model = new MessageModel(message, platform, existingApplication.AppliedToClan, userWhoReacted);
                    await _notificationService.SendDirectMessageToUserAsync(model);

                    _logger.LogInformation($"Guardian <{userWhoReacted.Id}> attempted to apply to more than one clan, while being on cooldown.");

                    return;
                }

                var newApplication = new ApplicationModel(userWhoReacted.Id, currentDateTimeOffset, clanName, platform);
                _applicationsUnderCooldown.Push(newApplication);

                var messageModel = new MessageModel("", platform, clanName, userWhoReacted);

                _logger.LogInformation($"Guardian <{userWhoReacted.Id}> applied to join {clanName}.");
                await _notificationService.SendApplicationAsync(messageModel);
                return;
            }
        }

        private static bool UserIsSpecifiedAndUserWhoReactedIsNotNull(SocketReaction appliedBy, IUser userWhoReacted)
        {
            return appliedBy.User.IsSpecified && userWhoReacted is not null;
        }

        private static bool HasApplicationCooldown(IUser userWhoReacted, DateTimeOffset currentDateTimeOffset)
        {
            return _applicationsUnderCooldown.Any(
                                u => u.DiscordUserId == userWhoReacted.Id &&
                                (currentDateTimeOffset - u.RegistrationDate)
                                .TotalHours <= ConstantHelper.APPLICATION_COOLDOWN_FROM_HOURS);
        }
    }
}
