using DataClassLibrary.Enums;
using DataClassLibrary.Models;
using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class MinionStartup : IStartup
    {
        private readonly DiscordSocketClient _client;
        private readonly ICommandHandler _commandHandler;
        private readonly IDataService _dataService;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IClanApplication _clanApplication;
        private IEnumerable<ulong> _clanApplicationChannels;

        public MinionStartup(
            DiscordSocketClient client,
            IConfiguration configuration,
            ILogger logger,
            IClanApplication clanApplication,
            ICommandHandler commandHandler,
            IDataService dataService)
        {
            _configuration = configuration;
            _client = client;
            _logger = logger;
            _clanApplication = clanApplication;
            _commandHandler = commandHandler;
            _dataService = dataService;
        }

        public async Task InitBotAsync()
        {
            // Hook Events.
            _client.Log += ClientLogging;
            _client.GuildAvailable += GuildAvailable;
            _client.GuildMembersDownloaded += GuildMembersDownloaded;
            _client.ReactionAdded += ReactionAdded;
            _client.Ready += OnClientReady;

            await _client.LoginAsync(TokenType.Bot, _configuration.GetValue<string>("Configuration:DiscordBot:Token"));
            await _client.StartAsync();

            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private Task OnClientReady()
        {
            _ = Task.Run(async () =>
            {
                await ValidateConfiguration();
                // Load data from db at some point.
                _clanApplicationChannels = new List<ulong> { 765277945194348544, 765277993454534667, 765277969278042132 };
                await _client.SetGameAsync($"On {_configuration.GetValue<string>("Configuration:DiscordBot:Version")}", type: ActivityType.Playing);
                await DownloadGuildUsersAsync();
            });

            return Task.CompletedTask;
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
                try
                {
                    var log = new LogMessage();

                    switch (arg.Exception)
                    {
                        case GatewayReconnectException:
                            await _logger.LogAsync(new(
                                LoggingSeverity.Debug,
                                "Client Log",
                                "Restarting Client",
                                "Discord",
                                DateTime.UtcNow));

                            log = new LogMessage(LogSeverity.Debug, "Gateway", "Restarting Services.");

                            await _client.StartAsync();
                            break;
                        case WebSocketClosedException:
                            await _logger.LogAsync(new(
                                LoggingSeverity.Debug,
                                "Client Log",
                                "Client connection was closed.",
                                "Discord",
                                DateTime.UtcNow));

                            log = new LogMessage(LogSeverity.Critical, "Discord", "WebSocket connection was closed. Establishing..");
                            break;
                        default:
                            log = arg;
                            break;
                    }

                    _logger.ConsoleLog(log);
                }
                catch (Exception)
                {
                    await _logger.LogAsync(new LogModel(LoggingSeverity.Debug, "Client Log", "Error while handling client logging.", "TQC Minion", DateTime.UtcNow));
                }
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
            _ = Task.Run(async () =>
            {
                // Get or download the user cache from the Server.
                IUserMessage userMessage = await cacheUserMessage.GetOrDownloadAsync();

                if (socketReaction.User.GetValueOrDefault() is not SocketGuildUser currentUser)
                {
                    currentUser = _client.GetUser(socketReaction.UserId) as SocketGuildUser;
                }

                // Debug Mode:
                if (socketReaction.Channel.Id.Equals(761687188341522492))
                {
                    _logger.ConsoleLog(new LogMessage(LogSeverity.Debug, "Debugging", "Working as intentional."));
                    return;
                }

                // If the socket reaction, is from any of the filtered channels.
                if (_clanApplicationChannels.Contains(socketReaction.Channel.Id))
                {
                    // If the user has any roles from the filter.
                    if (currentUser.Roles.Any(r => r.Id.Equals(414618518554673152)))
                    {
                        string message = $"Leadership assigned reaction <{socketReaction.Emote.Name}> to message.";
                        string createdBy = currentUser.Id.ToString();

                        await _logger.LogAsync(new LogModel(LoggingSeverity.Info, "Reaction Added", message, createdBy, DateTime.UtcNow));

                        return;
                    }

                    // Process a new clan application.
                    // Old Caller.
                    //await _clanApplication.ProcessClanApplicationAsync(currentReaction, currentUser);

                    // New Caller
                    await _clanApplication.ApplyToClanAsync(socketReaction, currentUser);

                    // Remove Reaction after process.
                    await userMessage.RemoveReactionAsync(socketReaction.Emote, socketReaction.UserId);
                }
            });

            return Task.CompletedTask;
        }

        private Task ValidateConfiguration()
        {
            _ = Task.Run(() =>
           {
               try
               {
                   string validationMessage;
                   if (_configuration != null && !string.IsNullOrEmpty(_configuration.GetConnectionString("ApiDb")))
                   {
                       validationMessage = "Configuration validation succeeded!";
                   }
                   else
                   {
                       validationMessage = "Configuration validation failed!";
                   }

                   _logger.ConsoleLog(new LogMessage(LogSeverity.Debug, "Configuration", $"{validationMessage}"));
               }
               catch (Exception)
               {
                   _logger.ConsoleLog(new LogMessage(LogSeverity.Error, "Configuration", $"Error validating configuration."));
               }
           });

            return Task.CompletedTask;
        }
    }
}