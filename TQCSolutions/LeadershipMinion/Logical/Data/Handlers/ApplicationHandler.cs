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
        private const int CLEAN_INTERVAL = 5;
        private const int TRY_AGAIN_TIMER = 1;

        private readonly IClanConfiguration _clanConfiguration;
        private readonly IClanService _clanService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ApplicationHandler> _logger;
        private readonly RuntimeHelper _runtimeHelper;

        public ApplicationHandler(IClanService clanService, ILogger<ApplicationHandler> logger, INotificationService notificationService, RuntimeHelper runtimeHelper, IClanConfiguration clanConfiguration)
        {
            _clanService = clanService;
            _logger = logger;
            _notificationService = notificationService;
            _runtimeHelper = runtimeHelper;
            _clanConfiguration = clanConfiguration;
        }

        public async Task HandleApplicationAsync(SocketReaction socketReaction, IUser discordUser)
        {
            if (UserIsSpecifiedAndUserWhoReactedIsNotNull(socketReaction, discordUser))
            {
                DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;

                var emote = socketReaction.Emote as Emote;
                var clan = _clanConfiguration.Clans.Find(c => c.EmoteId.Equals(emote.Id));

                if (_runtimeHelper.ApplicantHasCooldown(discordUser.Id, currentDateTimeOffset))
                {
                    await HandleApplicationUnderCooldownAsync(discordUser);
                    return;
                }

                await CreateApplicationAsync(discordUser, currentDateTimeOffset, clan);
                return;
            }

            _logger.LogError($"Reaction {nameof(socketReaction)} or User {nameof(discordUser)} was either null or not defined.");
        }

        private async Task CreateApplicationAsync(IUser discordUser, DateTimeOffset currentDateTimeOffset, ClanDataModel clanData)
        {
            var newApplication = new ApplicationModel(discordUser.Id, currentDateTimeOffset, clanData);

            //_runtimeHelper.Applications.Push(newApplication);
            var applicantApplied = _runtimeHelper.AddClanApplication(newApplication);

            if (applicantApplied)
            {
                var message = $"Hello Guardian. You're successfully signed up for {clanData.Name}. " +
                    $"Please await patiently for an admin to proceed your request. " +
                    $"Applying for more clans will not speed up the process.";

                var messageModel = new MessageModel(message, discordUser, newApplication);

                _logger.LogInformation($"Guardian <{discordUser.Id}> applied to join {clanData.Name}.");
                bool hasPrivacy = await _notificationService.NotifyUserAsync(messageModel);

                messageModel.Message = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {clanData.Name}. Confirmation message has also been sent to the Guardian.";
                if (hasPrivacy)
                {
                    messageModel.Message = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {clanData.Name}. Confirmation message could not be sent to the Guardian, due to privacy settings.";
                }

                await _notificationService.NotifyStaffsAsync(messageModel);
                return;
            }

            _logger.LogWarning($"Couldn't create application <{discordUser.Id}>.");
        }

        private async Task HandleApplicationUnderCooldownAsync(IUser discordUser)
        {
            //var existingApplication = _runtimeHelper.Applications.FirstOrDefault(app => app.DiscordUserId == discordUser.Id);
            var existingApplication = _runtimeHelper.GetExistingClanApplication(discordUser.Id);

            if (existingApplication is not null)
            {
                string message = $"Guardian. Be patient. You've already applied to join {existingApplication.AppliedToClan}. Please wait for it to proceed.";

                var model = new MessageModel(message, discordUser, existingApplication);

                var hasPrivacy = await _notificationService.NotifyUserAsync(model);

                var logMessage = $"Guardian <{discordUser.Id}> attempted to apply to more than one clan, while being on cooldown.";

                if (hasPrivacy)
                {
                    _logger.LogWarning($"{logMessage} User was not notified due to privacy settings.");
                    return;
                }

                _logger.LogInformation($"{logMessage} User was notified.");
                return;
            }

            _logger.LogWarning($"Couldn't retrieve existing application <{discordUser.Id}>, or it simply didn't exist anymore.");
        }

        private bool UserIsSpecifiedAndUserWhoReactedIsNotNull(SocketReaction socketReaction, IUser userWhoReacted)
        {
            return socketReaction.User.IsSpecified && userWhoReacted is not null;
        }
    }
}
