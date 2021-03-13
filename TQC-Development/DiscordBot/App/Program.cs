using DatabaseAccess.Database;
using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Repositories;
using DatabaseAccess.Repositories.Interfaces;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Interfaces;
using DiscordBot.Services;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Program
    {
        private const string DiscordToken = "NzY1Mjk3Mzk0NjQ4MjE5Njc4.X4SwvQ.xgWBRkSezMDkRCJ989ujvQBHYAE";
        private IConfiguration _configuration;

        private DiscordSocketClient _client;
        private DiscordSocketConfig _config;
        private DataService _dataService;
        private ILogRepository _logRepository;
        private IDatabase _database;
        private IClanApplication _clanApplication;
        private INotifier _notifier;
        private ILogger _logger;

        private IEnumerable<ulong> _clanApplicationChannels;

        private static Task Main(string[] args) => new Program().InitServicesAsync();

        private async Task InitServicesAsync()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile(Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json"), optional: false).Build();
            _config = new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100,
                ExclusiveBulkDelete = false,
                AlwaysDownloadUsers = true
            };

            _client = new DiscordSocketClient(_config);
            _database = new SqlDatabase(_configuration);
            _logRepository = new DatabaseLogRepository(_database);
            _logger = new LogService(_logRepository);
            _dataService = new DataService();

            _notifier = new NotificationService(_client, _logger, _dataService);
            _clanApplication = new ClanApplicationService(_notifier, _logger, _dataService);

            await InitBotAsync();
        }

        private async Task InitBotAsync()
        {
            // Events.
            _client.Log += ClientLogging;
            _client.GuildAvailable += GuildAvailable;
            _client.GuildMembersDownloaded += GuildMembersDownloaded;
            _client.ReactionAdded += ReactionAdded;

            _client.Ready += async () =>
            {
                await DownloadGuildUsersAsync();

                await _client.SetGameAsync("v2.5.0", type: ActivityType.Playing);

                // Load data from db at some point.
                _clanApplicationChannels = new List<ulong> { 765277945194348544, 765277993454534667, 765277969278042132 };

                ValidateConfiguration();
            };

            await InitializeConnectionWithDiscordAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private void ValidateConfiguration()
        {
            string validationMessage;
            if (_configuration != null && !string.IsNullOrEmpty(_configuration.GetConnectionString("ApiDb")))
            {
                validationMessage = "Configuration is Valid.";
            }
            else
            {
                validationMessage = "Configuration is Invalid.";
            }

            _logger.ConsoleLog(new LogMessage(LogSeverity.Verbose, "Configuration", $"{validationMessage}"));
        }

        private Task GuildMembersDownloaded(SocketGuild arg)
        {
            _logger.ConsoleLog(new LogMessage(LogSeverity.Info, "Guild Members Downloaded", $"Cached Offline Users from {arg.Name}!"));

            return Task.CompletedTask;
        }

        private Task ClientLogging(LogMessage arg)
        {
            _ = Task.Run(async () =>
              {
                  var log = new LogMessage();

                  switch (arg.Exception)
                  {
                      case GatewayReconnectException:
                          log = new LogMessage(LogSeverity.Critical, "Gateway", "Restarting Services.");

                          await _logger.DatabaseLogAsync(
                                LogSeverity.Critical,
                                "Gateway",
                                $"Restarting Services.",
                                $"Discord",
                                DateTime.UtcNow);

                          await RestartConnectionWithDiscordAsync();
                          break;
                      case WebSocketClosedException:
                          log = new LogMessage(LogSeverity.Critical, "Discord", "WebSocket connection was closed. Establishing..");
                          break;
                      default:
                          log = arg;
                          break;
                  }

                  _logger.ConsoleLog(log);
              });

            return Task.CompletedTask;
        }

        private Task DownloadGuildUsersAsync()
        {
            _ = Task.Run(async () =>
            {
                _logger.ConsoleLog(new LogMessage(LogSeverity.Verbose, "DownloadGuildUsers", "Loading Guilds.."));

                await Task.WhenAll(_client.Guilds.Select(g => g.DownloadUsersAsync()));
                int count = _client.Guilds.Sum(g => g.Users.Count);

                _logger.ConsoleLog(new LogMessage(LogSeverity.Verbose, "DownloadGuildUsers", $"Finished Download. Cached => {count} users."));
            });

            return Task.CompletedTask;
        }

        private Task GuildAvailable(SocketGuild arg)
        {
            _logger.ConsoleLog(new LogMessage(LogSeverity.Info, "Guild Available", $"{arg.Name} is now available!"));

            return Task.CompletedTask;
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> cacheUserMessage, ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction)
        {
            try
            {
                _ = Task.Run(async () =>
                {
                    // Debug Mode:
                    if (socketReaction.Channel.Id.Equals(761687188341522492))
                    {
                        _logger.ConsoleLog(new LogMessage(LogSeverity.Debug, "Debugging", "Working as intentional."));
                        return;
                    }

                    // Get or download the user cache from the Server.
                    IUserMessage userMessage = await cacheUserMessage.GetOrDownloadAsync();

                    // If the socket reaction, is from any of the filtered channels.
                    if (_clanApplicationChannels.Contains(socketReaction.Channel.Id))
                    {
                        // Get the value from the socket reaction as a Socket Guild User.
                        var guildUser = socketReaction.User.Value as SocketGuildUser;

                        // If the user has any roles from the filter.
                        if (guildUser.Roles.Any(r => r.Id.Equals(414618518554673152)))
                        {
                            // Log the message to the console.
                            _logger.ConsoleLog(
                                  logMessage: new LogMessage(
                                      severity: LogSeverity.Info,
                                      source: "Clan Application", $"Leadership <{guildUser.Nickname}:{guildUser.Id}> assigned reaction <{socketReaction.Emote.Name}> to message."));

                            await _logger.DatabaseLogAsync(
                                LogSeverity.Info,
                                "Reaction Added",
                                $"Leadership <{guildUser.Nickname}:{guildUser.Id}> assigned reaction <{socketReaction.Emote.Name}> to message.",
                                $"{socketReaction.User.Value.Username}",
                                DateTime.UtcNow);

                            return;
                        }

                        // Process a new clan application.
                        await _clanApplication.ProcessClanApplicationAsync(socketReaction);

                        // Remove Reaction after process.
                        await userMessage.RemoveReactionAsync(socketReaction.Emote, socketReaction.UserId);

                        return;
                    }
                });
            }
            catch (Exception)
            {
                _ = Task.Run(async () =>
                {
                    await _logger.DatabaseLogAsync(LogSeverity.Error, "Reaction Added", "Uncaught Error when processing Clan Application.", "TQC Minion", DateTime.UtcNow);
                });
            }
            return Task.CompletedTask;
        }

        private async Task InitializeConnectionWithDiscordAsync()
        {
            await LogInToDiscordAsync();
            await _client.StartAsync();
        }

        private async Task RestartConnectionWithDiscordAsync()
        {
            await _client.StopAsync();
            await LogInToDiscordAsync();
            await _client.StartAsync();
        }

        private async Task LogInToDiscordAsync()
        {
            await _client.LoginAsync(TokenType.Bot, DiscordToken);
        }
    }
}
