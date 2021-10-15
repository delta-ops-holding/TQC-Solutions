using Discord;
using Discord.Net;
using Discord.WebSocket;
using LeadershipMinion.Core.Abstractions;
using LeadershipMinion.Core.Helpers;
using LeadershipMinion.Logical.Data.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LeadershipMinion.Core
{
    public class StartupService : IStartupService
    {
        private readonly string _botStatusVersion;

        private readonly IBotConfiguration _botConfiguration;
        private readonly ILogger<StartupService> _logger;
        private readonly IApplicationHandler _applicationHandler;
        private readonly DiscordSocketClient _discordClient;

        public StartupService(IBotConfiguration botConfiguration, ILogger<StartupService> logger, DiscordSocketClient discordClient, IApplicationHandler applicationHandler)
        {
            _botConfiguration = botConfiguration;
            _logger = logger;
            _discordClient = discordClient;

            _botStatusVersion = $"On {_botConfiguration.Version}-{_botConfiguration.Status}";
            _applicationHandler = applicationHandler;
        }

        public async Task InitializeBotAsync()
        {
            _logger.LogInformation("Starting Services...");

            // Hook events before starting communication.
            SubscribeToEvents();

            await StartConnectionWithDiscordAsync();

            await Task.Delay(-1);
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

                // Debug Mode:
                if (socketReaction.Channel.Id.Equals(_botConfiguration.DebugChannel))
                {
                    _logger.LogDebug("Working as intentional.");
                    return;
                }

                // If the socket reaction, is from any of the filtered channels.
                if (_botConfiguration.Channels.Contains(socketReaction.Channel.Id))
                {
                    // If the user has any roles from the filter.
                    if (currentUser.Roles.Any(r => r.Id.Equals(_botConfiguration.StaffRole)))
                    {
                        string message = $"Leadership assigned reaction <{socketReaction.Emote.Name}> to message.";
                        _logger.LogDebug($"{message}");

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
        /// Fired when a guild becomes available.
        /// <br>GuildAvailable Event Handler for <see cref="DiscordSocketClient"/>.</br>
        /// </summary>
        /// <param name="guild"></param>
        /// <returns>A Task representing the asynchronous process.</returns>
        private Task GuildAvailable(SocketGuild guild)
        {
            _logger.LogDebug($"{guild.Name} is now available!");

            return Task.CompletedTask;
        }

        /// <summary>
        /// Fired when offline guild members are downloaded.
        /// <br>GuildMembersDownloaded Event Handler for <see cref="DiscordSocketClient"/>.</br>
        /// </summary>
        /// <param name="guild"></param>
        /// <returns>A Task representing the asynchronous process.</returns>
        private Task GuildMembersDownloaded(SocketGuild guild)
        {
            _logger.LogDebug($"Cached Offline Users from {guild.Name}!");

            return Task.CompletedTask;
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
                    // Set Game Status on Ready.
                    await _discordClient.SetGameAsync(_botStatusVersion, type: ActivityType.Playing);

                    // Run Funny Facts to be displayed as status.
                    //RunFunFactsRoulette();

                    // Download all Guild users on Ready.
                    _logger.LogDebug("Downloading Guild Members..");

                    await Task.WhenAll(_discordClient.Guilds.Select(g => g.DownloadUsersAsync()));
                    int count = _discordClient.Guilds.Sum(g => g.Users.Count);

                    _logger.LogDebug($"Finished Download. Cached => {count} users.");
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
                    if (logMessage.Message.Contains("Unknown Dispatch"))
                    {
                        return;
                    }

                    try
                    {
                        switch (logMessage.Exception)
                        {
                            case GatewayReconnectException reconnectException:
                                _logger.LogDebug($"Discord requested a server reconnect; reason: {reconnectException.Message}");
                                await RestartConnectionAsync();
                                break;
                            case WebSocketClosedException closedException:
                                _logger.LogWarning($"Discord closed my connection, attempting to restart system. Reason: {closedException.Reason}");
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

        private void RunFunFactsRoulette()
        {
            _ = Task.Run(
                async () =>
                {
                    _logger.LogInformation("Running Fun Facts Roulette.");
                    var loopFunFacts = true;

                    do
                    {
                        var funFacts = _botConfiguration.FunFacts;
                        var rnd = new Random();
                        var selectedFact = funFacts.ElementAt<string>(rnd.Next(0, funFacts.Count));
                        await _discordClient.SetGameAsync($"{_botStatusVersion} - {selectedFact}", type: ActivityType.Playing);

                        await Task.Delay(TimeSpan.FromSeconds(ConstantHelper.GAME_ACTIVITY_COOLDOWN));
                    } while (loopFunFacts);
                });
        }

        private void SubscribeToEvents()
        {
            _discordClient.Log += ClientLog;
            //_discordClient.GuildAvailable += GuildAvailable;
            //_discordClient.GuildMembersDownloaded += GuildMembersDownloaded;
            _discordClient.ReactionAdded += ReactionAdded;
            _discordClient.Ready += Ready;

            _logger.LogInformation("Events successfully subscribed.");
        }

        private void UnsubsribeToEvents()
        {
            _discordClient.Log -= ClientLog;
            //_discordClient.GuildMembersDownloaded -= GuildMembersDownloaded;
            //_discordClient.GuildAvailable -= GuildAvailable;
            _discordClient.ReactionAdded -= ReactionAdded;
            _discordClient.Ready -= Ready;

            _logger.LogInformation("Events successfully unsubscribed.");
        }
    }
}
