using Discord;
using Discord.WebSocket;
using LeadershipMinion.Core.Helpers;
using LeadershipMinion.Logical.Data.Abstractions;
using LeadershipMinion.Logical.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace LeadershipMinion.Logical.Data.Handlers
{
    /// <summary>
    /// A class that handles the way of processing applications.
    /// </summary>
    public class ApplicationHandler : IApplicationHandler
    {
        private readonly IClanConfiguration _clanConfiguration;
        private readonly INotificationService _notificationService;
        private readonly ILogger<ApplicationHandler> _logger;
        private readonly RuntimeHelper _runtimeHelper;

        public ApplicationHandler(ILogger<ApplicationHandler> logger, INotificationService notificationService, RuntimeHelper runtimeHelper, IClanConfiguration clanConfiguration)
        {
            _logger = logger;
            _notificationService = notificationService;
            _runtimeHelper = runtimeHelper;
            _clanConfiguration = clanConfiguration;
        }
        public async Task HandleCalBotMsgAsync(SocketMessage message, string PingStr)
        {
            await _notificationService.CalBotGhostPingAsync(message, PingStr);
        }

        public async Task HandleApplicationAsync(SocketReaction socketReaction, IUser discordUser)
        {
            if (UserIsSpecifiedAndUserWhoReactedIsNotNull(socketReaction, discordUser))
            {
                DateTimeOffset currentDateTimeOffset = DateTimeOffset.UtcNow;

                var emote = socketReaction.Emote as Emote;
                var clan = _clanConfiguration.Clans.Find(c => c.EmoteId.Equals(emote.Id));

                if (clan.Disabled)
                {
                    string message = $"Sorry, Guardian. {clan.Name} is currently disabled for registrations. If you think this was a mistake, please contact a staff memeber.";
                    var messageModel = new MessageModel(message, discordUser);

                    await _notificationService.NotifyUserAsync(messageModel);

                    return;
                }

                if (_runtimeHelper.ApplicantHasCooldown(discordUser.Id, currentDateTimeOffset))
                {
                    await HandleApplicationUnderCooldownAsync(discordUser);

                    return;
                }

                _runtimeHelper.RemoveApplication(discordUser.Id);
                await CreateApplicationAsync(discordUser, currentDateTimeOffset, clan);
                return;
            }

            _logger.LogError($"Reaction {nameof(socketReaction)} or User {nameof(discordUser)} was either null or not defined.");
        }

        private async Task CreateApplicationAsync(IUser discordUser, DateTimeOffset currentDateTimeOffset, ClanDataModel clanData)
        {
            var newApplication = new ApplicationModel(discordUser.Id, currentDateTimeOffset, clanData);
            var applicantAdded = _runtimeHelper.AddClanApplication(newApplication);

            if (applicantAdded)
            {
                var message = $"Hello Guardian. You're successfully signed up for {clanData.Name}. " +
                    $"Please await patiently for an admin to proceed your request. " +
                    $"Applying for more clans will not speed up the process.";

                var messageModel = new MessageModel(message, discordUser, newApplication);

                _logger.LogInformation($"Guardian <{discordUser.Id}> applied to join {clanData.Name}.");
                bool hasPrivacy = await _notificationService.NotifyUserAsync(messageModel);

                messageModel.Message = $"{discordUser.Username}, registered themself for joining {clanData.Name}. Confirmation message has also been sent to the Guardian.";
                if (hasPrivacy)
                {
                    messageModel.Message = $"{discordUser.Username}, registered themself for joining {clanData.Name}. Confirmation message could not be sent to the Guardian, due to privacy settings.";
                }

                await _notificationService.NotifyStaffsAsync(messageModel);
                return;
            }

            _logger.LogWarning($"Couldn't create application <{discordUser.Id}>.");
        }

        private async Task HandleApplicationUnderCooldownAsync(IUser discordUser)
        {
            var existingApplication = _runtimeHelper.GetExistingClanApplication(discordUser.Id);

            if (existingApplication is not null)
            {
                string message = $"Guardian. Be patient. You've already applied to join {existingApplication.ClanData.Name}. Please wait for it to proceed.";

                var model = new MessageModel(message, discordUser, existingApplication);

                var hasPrivacy = await _notificationService.NotifyUserAsync(model);

                if (hasPrivacy)
                {
                    return;
                }

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
