using Discord;
using Discord.Net;
using Discord.WebSocket;
using LeadershipMinion.Core.Abstractions;
using LeadershipMinion.Core.Helpers;
using LeadershipMinion.Logical.Data.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            // Enable Logging.
            _discordClient.Log += ClientLog;
            _discordClient.GuildAvailable += GuildAvailable;
            _discordClient.GuildMembersDownloaded += GuildMembersDownloaded;
            _discordClient.ReactionAdded += ReactionAdded;
            _discordClient.Ready += Ready;

            await _discordClient.LoginAsync(TokenType.Bot, _botConfiguration.Token);
            await _discordClient.StartAsync();

            await Task.Delay(-1);
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> cacheUserMessage, Cacheable<IMessageChannel, ulong> socketMessageChannel, SocketReaction socketReaction)
        {
            _ = Task.Run(async () =>
            {
                // Get or download the user cache from the Server.
                IUserMessage userMessage = await cacheUserMessage.GetOrDownloadAsync();

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
                        _logger.LogInformation($"{message}");

                        return;
                    }

                    // New Caller
                    await _applicationHandler.CreateApplicationAsync(socketReaction, currentUser);

                    // Remove Reaction after process.
                    await userMessage.RemoveReactionAsync(socketReaction.Emote, socketReaction.UserId);
                }
            });

            return Task.CompletedTask;
        }

        private Task GuildAvailable(SocketGuild guild)
        {
            _logger.LogInformation($"{guild.Name} is now available!");

            return Task.CompletedTask;
        }

        private Task GuildMembersDownloaded(SocketGuild guild)
        {
            _logger.LogInformation($"Cached Offline Users from {guild.Name}!");

            return Task.CompletedTask;
        }

        private Task Ready()
        {
            _ = Task.Run(
                async () =>
                {
                    // Set Game Status on Ready.
                    _logger.LogInformation("Setting game as status..");
                    await _discordClient.SetGameAsync(_botStatusVersion, type: ActivityType.Playing);

                    // Run Funny Facts to be displayed as status.
                    //RunFunFactsRoulette();

                    // Download all Guild users on Ready.
                    await DownloadGuildMembersAsync();
                });

            return Task.CompletedTask;
        }        

        private Task ClientLog(LogMessage logMessage)
        {
            // Use Task to run background thread.
            _ = Task.Run(
                async () =>
                {
                    try
                    {
                        var log = logMessage.ToString();

                        switch (logMessage.Exception)
                        {
                            case GatewayReconnectException:
                                _logger.LogWarning(log);
                                await RestartConnectionAsync();
                                break;
                            case WebSocketClosedException:
                                _logger.LogWarning("Discord closed my connection, attempting to restart system.");
                                await RestartConnectionAsync();
                                break;
                            default:
                                _logger.LogInformation(log);
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

        private async Task RestartConnectionAsync()
        {
            await _discordClient.LogoutAsync();
            DisposeEvents();
            _logger.LogInformation("Logout Successfull.");

            await InitializeBotAsync();
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

                        await Task.Delay(TimeSpan.FromSeconds(ConstantHelper.GAME_ACTIVITY_COOLDOWN_FROM_SECONDS));
                    } while (loopFunFacts);
                });
        }

        private void DisposeEvents()
        {
            _discordClient.Log -= ClientLog;
            _discordClient.Ready -= Ready;
            _discordClient.GuildMembersDownloaded -= GuildMembersDownloaded;
            _discordClient.GuildAvailable -= GuildAvailable;
            _discordClient.ReactionAdded -= ReactionAdded;
        }

        internal Task DownloadGuildMembersAsync()
        {
            _ = Task.Run(
                async () =>
                {
                    _logger.LogInformation("Downloading Guild Members..");

                    await Task.WhenAll(_discordClient.Guilds.Select(g => g.DownloadUsersAsync()));
                    int count = _discordClient.Guilds.Sum(g => g.Users.Count);

                    _logger.LogInformation($"Finished Download. Cached => {count} users.");
                });

            return Task.CompletedTask;
        }
    }
}
