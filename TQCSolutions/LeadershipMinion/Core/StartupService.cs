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
            InitializeEvents();

            await _discordClient.LoginAsync(TokenType.Bot, _botConfiguration.Token);
            await _discordClient.StartAsync();

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

                    // New Caller
                    await _applicationHandler.HandleApplicationAsync(socketReaction, currentUser);

                    // Cleanup Process.
                    // Get or download the user cache from the Server.
                    IUserMessage userMessage = await GetCachedEntityOrDownloadItAsync(cacheUserMessage);

                    if (userMessage is not null)
                    {
                        await userMessage.RemoveReactionAsync(socketReaction.Emote, socketReaction.UserId);
                        return;
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
                    await DownloadGuildMembersAsync();
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
                    try
                    {
                        switch (logMessage.Exception)
                        {
                            case GatewayReconnectException:
                                _logger.LogDebug("Discord requested a server reconnect.");
                                await RestartConnectionAsync();
                                break;
                            case WebSocketClosedException:
                                _logger.LogWarning("Discord closed my connection, attempting to restart system.");
                                await RestartConnectionAsync();
                                break;
                            default:
                                _logger.LogInformation(logMessage.Message);
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

        internal async Task RestartConnectionAsync()
        {
            _logger.LogInformation("Restarting Services...");
            await _discordClient.LogoutAsync();
            DisposeEvents();
            _logger.LogInformation("Logout Successfull.");

            await InitializeBotAsync();
        }

        internal async Task<IUserMessage> GetCachedEntityOrDownloadItAsync(Cacheable<IUserMessage, ulong> cacheUserMessage)
        {
            try
            {
                return await cacheUserMessage.GetOrDownloadAsync();
            }
            catch (HttpException httpEx)
            {
                _logger.LogError(httpEx, $"Cannot cache message {cacheUserMessage.Id} from a user account.");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"The message is deleted and is not in the cache.");
                return null;
            }
        }

        internal Task DownloadGuildMembersAsync()
        {
            _ = Task.Run(
                async () =>
                {
                    _logger.LogDebug("Downloading Guild Members..");

                    await Task.WhenAll(_discordClient.Guilds.Select(g => g.DownloadUsersAsync()));
                    int count = _discordClient.Guilds.Sum(g => g.Users.Count);

                    _logger.LogDebug($"Finished Download. Cached => {count} users.");
                });

            return Task.CompletedTask;
        }

        internal void RunFunFactsRoulette()
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

                        await Task.Delay(TimeSpan.FromSeconds(ConstantHelper.GAME_ACTIVITY_COOLDOWN_FROM_SECONDS));
                    } while (loopFunFacts);
                });
        }

        internal void InitializeEvents()
        {
            // Enable Client Logging.
            _discordClient.Log += ClientLog;

            // Observe available guilds.
            _discordClient.GuildAvailable += GuildAvailable;

            // Observe when offline Guild Members are downloaded.
            _discordClient.GuildMembersDownloaded += GuildMembersDownloaded;

            // Observe whenever a reaction is added to a message.
            _discordClient.ReactionAdded += ReactionAdded;

            // Observe whenever the client is ready.
            _discordClient.Ready += Ready;

            _logger.LogInformation("Events successfully initalized.");
        }

        internal void DisposeEvents()
        {
            _discordClient.Log -= ClientLog;
            _discordClient.Ready -= Ready;
            _discordClient.GuildMembersDownloaded -= GuildMembersDownloaded;
            _discordClient.GuildAvailable -= GuildAvailable;
            _discordClient.ReactionAdded -= ReactionAdded;

            _logger.LogInformation("Events successfully disposed.");
        }
    }
}
