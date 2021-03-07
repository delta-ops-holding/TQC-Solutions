using Discord;
using Discord.WebSocket;
using DiscordBot.Interfaces;
using DiscordBot.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Program
    {
        private const string DiscordToken = "";

        private DiscordSocketClient _client;
        private DiscordSocketConfig _config;
        private DataService _dataService;
        private IClanApplication _clanApplication;
        private INotifier _notifier;
        private ILogger _logger;

        private IEnumerable<ulong> _clanApplicationChannels;

        private static Task Main(string[] args) => new Program().InitServicesAsync();

        private async Task InitServicesAsync()
        {
            _config = new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100
            };

            _client = new DiscordSocketClient(_config);
            _dataService = new DataService();
            _logger = new LogService(_client);

            _notifier = new NotificationService(_client, _logger, _dataService);
            _clanApplication = new ClanApplicationService(_notifier, _logger, _dataService);

            await InitBotAsync();
        }

        private async Task InitBotAsync()
        {
            // Events.       
            _client.Log += _logger.ConsoleLog;
            _client.Ready += async () =>
            {
                await DownloadGuildUsers();

                // Load data from db at some point.
                _clanApplicationChannels = new List<ulong> { 765277945194348544, 765277993454534667, 765277969278042132 };
            };

            _client.GuildAvailable += GuildAvailable;
            _client.ReactionAdded += ReactionAdded;

            await _client.LoginAsync(TokenType.Bot, DiscordToken);
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task DownloadGuildUsers()
        {
            _ = Task.Run(async () =>
            {
                await _logger.ConsoleLog(new LogMessage(LogSeverity.Verbose, "DownloadGuildUsers", "Loading Guilds.."));

                await Task.WhenAll(_client.Guilds.Select(g => g.DownloadUsersAsync()));
                int count = _client.Guilds.Sum(g => g.Users.Count);

                await _logger.ConsoleLog(new LogMessage(LogSeverity.Verbose, "DownloadGuildUsers", $"Finished Download. Cached => {count} users."));
            });

            return Task.CompletedTask;
        }

        private Task GuildAvailable(SocketGuild arg)
        {
            Task.Run(async () =>
            {
                await _logger.ConsoleLog(new LogMessage(LogSeverity.Info, "Event", $"Guild is available {arg.Name}"));
            });

            return Task.CompletedTask;
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> cacheUserMessage, ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction)
        {
            Task.Run(async () =>
            {
                // Debug Mode:
                if (socketReaction.Channel.Id.Equals(761687188341522492))
                {
                    await _logger.ConsoleLog(new LogMessage(LogSeverity.Debug, "Debugging", "Working as intentional."));
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
                        await _logger.ConsoleLog(
                            logMessage: new LogMessage(
                                severity: LogSeverity.Info,
                                source: "Clan Application", $"Leadership <{guildUser.Nickname}:{guildUser.Id}> assigned reaction <{socketReaction.Emote.Name}> to message."));

                        return;
                    }

                    // Process a new clan application.
                    await _clanApplication.ProcessClanApplicationAsync(userMessage, socketReaction);

                    return;
                }
            });

            return Task.CompletedTask;
        }
    }
}
