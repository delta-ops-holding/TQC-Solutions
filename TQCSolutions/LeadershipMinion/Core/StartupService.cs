using Discord;
using Discord.Net;
using Discord.WebSocket;
using LeadershipMinion.Core.Abstractions;
using LeadershipMinion.Logical.Data.Abstractions;
using LeadershipMinion.Logical.Enums;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LeadershipMinion.Core
{
    public class StartupService : IStartupService
    {
        private readonly string _botStatusVersion;

        private readonly IBotConfiguration _botConfiguration;
        private readonly IBasicConfiguration _basicConfiguration;
        private readonly ILogger<StartupService> _logger;
        private readonly IApplicationHandler _applicationHandler;
        private readonly DiscordSocketClient _discordClient;

        public StartupService(IBotConfiguration botConfiguration, ILogger<StartupService> logger, DiscordSocketClient discordClient, IApplicationHandler applicationHandler, IBasicConfiguration basicConfiguration)
        {
            _botConfiguration = botConfiguration;
            _basicConfiguration = basicConfiguration;
            _logger = logger;
            _discordClient = discordClient;
            _applicationHandler = applicationHandler;

            _botStatusVersion = $"On {_botConfiguration.Version}-{_botConfiguration.Status}";
        }

        public async Task InitializeBotAsync()
        {
            _logger.LogInformation("Starting Services...");
            SubscribeToEvents();
            await StartConnectionWithDiscordAsync();

            await Task.Delay(-1);
        }

        /// <summary>
        /// Fired when guild data has finished downloading.
        /// <br>Ready Event Handler for <see cref="DiscordSocketClient"/>.</br>
        /// </summary>
        /// <returns>A Task representing the asynchronous process.</returns>
        private Task Ready()
        {
            _ = Task.Run(
                async () =>
                {
                    await _discordClient.SetGameAsync(_botStatusVersion, type: ActivityType.Playing);

                    await Task.WhenAll(_discordClient.Guilds.Select(g => g.DownloadUsersAsync()));
                });

            return Task.CompletedTask;
        }

        
        private Task MessageReceived(SocketMessage message)
        {
            _ = Task.Run(async () =>
            {
                if (message.Author.Id == _basicConfiguration.CalBotId)
                {
                    string PingRegexPattern = @"\b<@\d+>\b";
                    Match m = Regex.Match(message.Embeds.First().Description, PingRegexPattern);
                    await _applicationHandler.HandleCalBotMsgAsync(message, m.Value);
                    if (m.Success)
                    {
                        /// await _applicationHandler.HandleCalBotMsgAsync(message, m.Value);
                    }
                }
            });
            return Task.CompletedTask;
        }
        /// <summary>
        /// Fired when a reaction is added to a message.
        /// <br>ReactionAdded Event Handler for <see cref="DiscordSocketClient"/>.</br>
        /// </summary>
        /// <param name="cacheUserMessage"></param>
        /// <param name="messageChannel"></param>
        /// <param name="socketReaction"></param>
        /// <returns>A Task representing the asynchronous process.</returns>
        private Task ReactionAdded(Cacheable<IUserMessage, ulong> cacheUserMessage, Cacheable<IMessageChannel, ulong> messageChannel, SocketReaction socketReaction)
        {
            _ = Task.Run(async () =>
            {
                if (socketReaction.User.GetValueOrDefault() is not SocketGuildUser currentUser)
                {
                    currentUser = _discordClient.GetUser(socketReaction.UserId) as SocketGuildUser;
                }

                if (ContainsFilteredChannel(socketReaction.Channel.Id) || IsDebugEnvironment())
                {
                    if (UserHasFilteredRole(currentUser.Roles) || currentUser.IsBot)
                    {
                        return;
                    }

                    await _applicationHandler.HandleApplicationAsync(socketReaction, currentUser);

                    try
                    {
                        var userMessage = await cacheUserMessage.GetOrDownloadAsync();

                        if (userMessage is not null)
                        {
                            await userMessage.RemoveReactionAsync(socketReaction.Emote, socketReaction.UserId);
                            return;
                        }
                    }
                    catch (Exception)
                    {
                        _logger.LogError("Error while attempting to remove the reaction after process.");
                    }
                }
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// Log Event Handler for <see cref="DiscordSocketClient"/>.
        /// </summary>
        /// <param name="logMessage">A message object used for logging purposes.</param>
        /// <returns>A Task representing the asynchronous process.</returns>
        private Task ClientLog(LogMessage logMessage)
        {
            // Use Task to run background thread.
            _ = Task.Run(
                async () =>
                {
                    if (ContainsFilteredMessages(logMessage.Message, "Unknown Dispatch", "Unknown Channel"))
                    {
                        return;
                    }

                    try
                    {
                        switch (logMessage.Exception)
                        {
                            case GatewayReconnectException reconnectException:
                                _logger.LogInformation($"Discord requested a server reconnect; reason: {reconnectException.Message}");
                                await RestartConnectionAsync();
                                break;
                            case WebSocketClosedException closedException:
                                _logger.LogInformation($"Discord closed my connection, attempting to restart system. Reason: {closedException.Reason}");
                                await RestartConnectionAsync();
                                break;
                            default:
                                switch (logMessage.Severity)
                                {
                                    case LogSeverity.Critical:
                                        _logger.LogCritical(logMessage.Message);
                                        break;
                                    case LogSeverity.Error:
                                        _logger.LogError(logMessage.Message);
                                        break;
                                    case LogSeverity.Warning:
                                        _logger.LogWarning(logMessage.Message);
                                        break;
                                    case LogSeverity.Info:
                                        _logger.LogInformation(logMessage.Message);
                                        break;
                                    case LogSeverity.Verbose:
                                        _logger.LogTrace(logMessage.Message);
                                        break;
                                    case LogSeverity.Debug:
                                        _logger.LogDebug(logMessage.Message);
                                        break;
                                }
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(exception: ex, "Something went wrong while attempting to log");
                    }
                });

            return Task.CompletedTask;
        }

        private async Task StartConnectionWithDiscordAsync()
        {
            await _discordClient.LoginAsync(TokenType.Bot, _botConfiguration.Token);
            await _discordClient.StartAsync();
        }

        private async Task RestartConnectionAsync()
        {
            _logger.LogInformation("Restarting Connection...");
            await _discordClient.LogoutAsync();
            _logger.LogInformation("Logout Successfull.");

            await StartConnectionWithDiscordAsync();
        }

        private bool ContainsFilteredMessages(string message, params string[] filters)
        {
            if (filters.Length is 0)
            {
                return false;
            }

            for (int i = 0; i < filters.Length; i++)
            {
                if (message.Contains(filters[i]))
                {
                    return true;
                }
            }

            return false;
        }

        private bool ContainsFilteredChannel(ulong channelId)
        {
            return _basicConfiguration.ApplicationChannels.Contains(channelId);
        }

        private bool UserHasFilteredRole(IReadOnlyCollection<SocketRole> userRoles)
        {
            return userRoles.Any(r => r.Id.Equals(_basicConfiguration.StaffRole));
        }

        private bool IsDebugEnvironment()
        {
            return _basicConfiguration.Environment == SystemEnvironment.Debug;
        }

        private void RunFunFactsRoulette()
        {
            //_ = Task.Run(
            //    async () =>
            //    {
            //        _logger.LogInformation("Running Fun Facts Roulette.");
            //        var loopFunFacts = true;

            //        do
            //        {
            //            var funFacts = _botConfiguration.FunFacts;
            //            var rnd = new Random();
            //            var selectedFact = funFacts.ElementAt<string>(rnd.Next(0, funFacts.Count));
            //            await _discordClient.SetGameAsync($"{_botStatusVersion} - {selectedFact}", type: ActivityType.Playing);

            //            await Task.Delay(TimeSpan.FromSeconds(ConstantHelper.GAME_ACTIVITY_COOLDOWN));
            //        } while (loopFunFacts);
            //    });
        }

        private void SubscribeToEvents()
        {
            _discordClient.Log += ClientLog;
            _discordClient.ReactionAdded += ReactionAdded;
            _discordClient.MessageReceived += MessageReceived;
            _discordClient.Ready += Ready;

            _logger.LogInformation("Events successfully subscribed.");
        }
    }
}
