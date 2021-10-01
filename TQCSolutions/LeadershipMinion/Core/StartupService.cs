using Discord;
using Discord.Net;
using Discord.WebSocket;
using LeadershipMinion.Core.Abstractions;
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
        private readonly IBotConfiguration _botConfiguration;
        private readonly ILogger<StartupService> _logger;
        private readonly DiscordSocketClient _discordClient;

        public StartupService(IBotConfiguration botConfiguration, ILogger<StartupService> logger, DiscordSocketClient discordClient)
        {
            _botConfiguration = botConfiguration;
            _logger = logger;
            _discordClient = discordClient;
        }

        public async Task InitializeBotAsync()
        {
            // Enable Logging.
            _discordClient.Log += ClientLog;
            _discordClient.Ready += Ready;
            _discordClient.GuildMembersDownloaded += DownloadedGuildMembers;
            _discordClient.GuildAvailable += GuildAvailable;
            _discordClient.ReactionAdded += ReactionAdded;

            await Task.Delay(-1);
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> arg1, Cacheable<IMessageChannel, ulong> arg2, SocketReaction arg3)
        {
            throw new NotImplementedException();
        }

        private Task GuildAvailable(SocketGuild guild)
        {
            _logger.LogInformation($"{guild.Name} is now available!");

            return Task.CompletedTask;
        }

        private Task DownloadedGuildMembers(SocketGuild guild)
        {
            _logger.LogInformation($"Finished Downloading Guild Members; Cached <{guild.Users.Count}> Members.");

            return Task.CompletedTask;
        }

        private Task Ready()
        {
            Task t = Task.Run(
                async () =>
                {
                    // Set Game Status on Ready.
                    _logger.LogInformation("Setting game as status..");
                    await _discordClient.SetGameAsync($"On {_botConfiguration.Version}", type: ActivityType.Playing);

                    // Download all Guild users on Ready.
                    _logger.LogInformation("Downloading Guild Members..");
                    await Task.WhenAll(_discordClient.Guilds.Select(g => g.DownloadUsersAsync()));
                });

            return t;
        }

        private Task ClientLog(LogMessage logMessage)
        {
            // Use Task to run background thread.
            Task t = Task.Run(
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

            return t;
        }

        private async Task RestartConnectionAsync()
        {
            await _discordClient.LogoutAsync();
            DisposeEvents();
            _logger.LogInformation("Logout Successfull.");

            await InitializeBotAsync();
        }

        private void DisposeEvents()
        {
            _discordClient.Log -= ClientLog;
        }
    }
}
