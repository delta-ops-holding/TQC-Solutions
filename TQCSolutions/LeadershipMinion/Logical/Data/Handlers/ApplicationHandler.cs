using Discord;
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

        private readonly IClanService _clanService;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ApplicationHandler> _logger;
        private readonly RuntimeHelper<ApplicationModel> _runtimeHelper;

        public ApplicationHandler(IClanService clanService, ILogger<ApplicationHandler> logger, INotificationService notificationService, RuntimeHelper<ApplicationModel> runtimeHelper)
        {
            _clanService = clanService;
            _logger = logger;
            _notificationService = notificationService;
            _runtimeHelper = runtimeHelper;
        }

        public async Task HandleApplicationAsync(SocketReaction messageReaction, IUser discordUser)
        {
            if (UserIsSpecifiedAndUserWhoReactedIsNotNull(messageReaction, discordUser))
            {
                DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;

                var emote = messageReaction.Emote as Emote;
                var clanName = _clanService.GetClanNameByEmoteId(emote.Id);
                var platform = _clanService.GetClanPlatformByChannelId(messageReaction.Channel.Id);

                if (ApplicantHasCooldown(discordUser, currentDateTimeOffset))
                {
                    await HandleApplicationUnderCooldownAsync(discordUser);

                    await Task.CompletedTask;
                }

                await CreateApplicationAsync(discordUser, currentDateTimeOffset, clanName, platform);

                await Task.CompletedTask;
            }

            _logger.LogWarning($"User {messageReaction.User.Value.Id} was either not specified or the reaction object was null.");
        }

        private async Task CreateApplicationAsync(IUser discordUser, DateTimeOffset currentDateTimeOffset, Enums.Clan clanName, Enums.ClanPlatform platform)
        {
            var newApplication = new ApplicationModel(discordUser.Id, currentDateTimeOffset, clanName, platform);

            _runtimeHelper.Applications.Push(newApplication);

            var messageModel = new MessageModel("", discordUser, newApplication);

            _logger.LogInformation($"Guardian <{discordUser.Id}> applied to join {clanName}.");

            bool hasPrivacy = await _notificationService.NotifyUserAsync(messageModel);

            messageModel.Message = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {clanName}. Confirmation message has also been sent to the Guardian.";
            if (hasPrivacy)
            {
                messageModel.Message = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {clanName}. Confirmation message could not be sent to the Guardian, due to privacy settings.";
            }

            await _notificationService.NotifyStaffsAsync(messageModel);
        }

        private async Task HandleApplicationUnderCooldownAsync(IUser discordUser)
        {
            var existingApplication = _runtimeHelper.Applications.FirstOrDefault(app => app.DiscordUserId == discordUser.Id);

            string message = $"Guardian. Be patient. You've already applied to join {existingApplication.AppliedToClan}. Please wait for it to proceed.";

            var model = new MessageModel(message, discordUser, existingApplication);

            var hasPrivacy = await _notificationService.NotifyUserAsync(model);

            var logMessage = $"Guardian <{discordUser.Id}> attempted to apply to more than one clan, while being on cooldown.";

            if (hasPrivacy)
            {
                _logger.LogWarning($"{logMessage} User was not notified due to privacy settings.");
                
                await Task.CompletedTask;
            }

            _logger.LogInformation($"{logMessage} User was notified.");
            await Task.CompletedTask;
        }

        private bool ApplicantHasCooldown(IUser userWhoReacted, DateTimeOffset currentDateTimeOffset)
        {
            return _runtimeHelper.Applications.Any(
                                u => u.DiscordUserId == userWhoReacted.Id &&
                                (currentDateTimeOffset - u.RegistrationDate)
                                .TotalHours <= ConstantHelper.APPLICATION_COOLDOWN_FROM_HOURS);
        }

        private bool UserIsSpecifiedAndUserWhoReactedIsNotNull(SocketReaction appliedBy, IUser userWhoReacted)
        {
            return appliedBy.User.IsSpecified && userWhoReacted is not null;
        }

        private void CleanApplicationsByInternal()
        {
            //_logger.LogDebug("Cleaning applications..");

            //_ = Task.Run(() =>
            //{
            //    while (!_applicationsUnderCooldown.Any())
            //    {
            //        _logger.LogDebug($"No applications found.");
            //        Task.Delay(TimeSpan.FromMinutes(TRY_AGAIN_TIMER));
            //    }

            //    do
            //    {
            //        _logger.LogDebug($"Applications under CD: <{_applicationsUnderCooldown.Count}>.");

            //        for(int i = 0; i < _applicationsUnderCooldown.Count; i++)
            //        {
            //            if (_applicationsUnderCooldown[i].RegistrationDate < DateTimeOffset.UtcNow)
            //            {
            //                _applicationsUnderCooldown.Remove(_applicationsUnderCooldown[i]);
            //            }
            //        }

            //        _logger.LogDebug($"Cleaned Applications under CD: <{_applicationsUnderCooldown.Count}>.");

            //    } while (_applicationsUnderCooldown is not null && _applicationsUnderCooldown.Any());

            //    Task.Delay(TimeSpan.FromMinutes(CLEAN_INTERVAL));
            //    CleanApplicationsByInternal();
            //});
        }
    }
}
