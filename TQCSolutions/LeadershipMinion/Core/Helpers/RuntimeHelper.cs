using LeadershipMinion.Logical.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
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
        //private readonly CancellationTokenSource _tokenSource = new();
        private readonly ILogger<RuntimeHelper> _logger;

        public RuntimeHelper(ILogger<RuntimeHelper> logger)
        {
            _logger = logger;

            //Task.Run(() => InvokeCleanApplicationDataByInterval(
            //    TimeSpan.FromHours(ConstantHelper.CLEAN_APPLICATIONS_INTERVAL),
            //    _tokenSource.Token));
        }

        /// <summary>
        /// Add a clan application.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>True if the application was added, otherwise false.</returns>
        public bool AddClanApplication(ApplicationModel model)
        {
            try
            {
                return _clanApplications.TryAdd(model.DiscordUserId, model);
            }
            catch (ArgumentNullException e)
            {
                _logger.LogError(e, $"Failed to add App to list, key is null\n{model}");
                return false;
            }
            catch (OverflowException e)
            {
                _logger.LogError(e, "Failed to add App to list, _clanApplications triggered OverflowException");
                return false;
            }
        }

        /// <summary>
        /// Gets an existing clan applications.
        /// </summary>
        /// <param name="applicantKey"></param>
        /// <returns>An existing <see cref="ApplicationModel"/> if it's found, otherwise null.</returns>
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

        /// <summary>
        /// Checks whether the given applicant has an application under cooldown.
        /// </summary>
        /// <param name="applicantKey"></param>
        /// <param name="currentDate"></param>
        /// <returns>True if under cooldown, otherwise false.</returns>
        public bool ApplicantHasCooldown(ulong applicantKey, DateTimeOffset currentDate)
        {
            var hasCooldown = _clanApplications
                .Any(x =>
                    x.Key.Equals(applicantKey) &&
                    (currentDate - x.Value.RegistrationDate).TotalHours <= ConstantHelper.APPLICATION_COOLDOWN
                );

            return hasCooldown;
        }


        public bool RemoveApplication(ulong applicantKey)
        {
            var removed = _clanApplications.TryRemove(applicantKey, out var app);

            if (removed) 
            {
                _logger.LogDebug($"Removed {applicantKey} from  _clanApplications");
            }

            return removed;
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