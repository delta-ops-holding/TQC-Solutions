using LeadershipMinion.Logical.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeadershipMinion.Core.Helpers
{
    /// <summary>
    /// Represents a class to help with Runtime data.
    /// </summary>
    public class RuntimeHelper
    {
        private readonly ConcurrentDictionary<ulong, ApplicationModel> _clanApplications = new();
        private readonly CancellationTokenSource _tokenSource = new();
        private readonly ILogger<RuntimeHelper> _logger;

        public RuntimeHelper(ILogger<RuntimeHelper> logger)
        {
            _logger = logger;

            Task.Run(() => InvokeCleanApplicationDataByInterval(
                TimeSpan.FromHours(1),
                _tokenSource.Token));
        }

        private readonly Stack<ApplicationModel> Applications = new(100);

        public bool AddClanApplication(ApplicationModel model)
        {
            try
            {
                return _clanApplications.TryAdd(model.DiscordUserId, model);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        public ApplicationModel GetExistingClanApplication(ulong applicantKey)
        {
            try
            {
                bool isExisting = _clanApplications.TryGetValue(applicantKey, out var existingModel);

                if (isExisting)
                {
                    return existingModel;
                }

                return null;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        public bool ApplicantHasCooldown(ulong applicantKey, DateTimeOffset currentDate)
        {
            return _clanApplications.Any(x =>
            x.Key == applicantKey &&
            (currentDate - x.Value.RegistrationDate).TotalHours
            <= ConstantHelper.APPLICATION_COOLDOWN_FROM_HOURS);
        }

        private async Task InvokeCleanApplicationDataByInterval(TimeSpan interval, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Started application cleaner with interval of {interval.TotalHours} hours.");
            while (true)
            {
                if (!_clanApplications.IsEmpty)
                {
                    _logger.LogDebug($"Cleaning applications; looking at {_clanApplications.Count} registered applications.");

                    var todaysDate = DateTimeOffset.UtcNow;

                    foreach (var application in _clanApplications)
                    {
                        var value = application.Value;

                        if (!ApplicantHasCooldown(value.DiscordUserId, value.RegistrationDate))
                        {
                            try
                            {
                                bool isRemoved = _clanApplications.TryRemove(value.DiscordUserId, out var removedApplication);

                                if (isRemoved)
                                {
                                    var hoursOld = todaysDate.Subtract(removedApplication.RegistrationDate).TotalHours;
                                    _logger.LogInformation($"Removed old application from memory, was registered <{hoursOld}> ago.");
                                }
                            }
                            catch (ArgumentNullException)
                            {
                                continue;
                            }
                        }
                    }

                    _logger.LogDebug($"Finished cleaning applications; looking at {_clanApplications.Count} registered applications.");
                }

                Task t = Task.Delay(interval, cancellationToken);

                try
                {
                    await t;
                }
                catch (TaskCanceledException taskEx)
                {
                    _logger.LogWarning($"Something went wrong while cleaning the application data by interval => {taskEx.Message}");
                    return;
                }
            }
        }
    }
}
