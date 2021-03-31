using DatabaseAccess.Database;
using DatabaseAccess.Database.Interfaces;
using DatabaseAccess.Repositories;
using DatabaseAccess.Repositories.Interfaces;
using DataClassLibrary.Enums;
using DataClassLibrary.Models;
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
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class MinionStartup : IStartup
    {
        private readonly DiscordSocketClient _client;
        private readonly ICommandHandler _commandHandler;
        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;
        private readonly IClanApplication _clanApplication;
        private IEnumerable<ulong> _clanApplicationChannels;

        public MinionStartup(
            DiscordSocketClient client,
            IConfiguration configuration,
            ILogger logger,
            IClanApplication clanApplication,
            ICommandHandler commandHandler)
        {
            _configuration = configuration;
            _client = client;
            _logger = logger;
            _clanApplication = clanApplication;
            _commandHandler = commandHandler;
        }

        public async Task InitBotAsync()
        {
            // Events.
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
                await _client.SetGameAsync(_configuration.GetValue<string>("Configuration:DiscordBot:Version"), type: ActivityType.Playing);
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
                try
                {
                    // Get or download the user cache from the Server.
                    IUserMessage userMessage = await cacheUserMessage.GetOrDownloadAsync();
                    SocketReaction currentReaction = socketReaction;

                    if (currentReaction.User.GetValueOrDefault() is not SocketGuildUser currentUser)
                    {
                        currentUser = _client.GetUser(currentReaction.UserId) as SocketGuildUser;
                    }

                    // Debug Mode:
                    if (currentReaction.Channel.Id.Equals(761687188341522492))
                    {
                        _logger.ConsoleLog(new LogMessage(LogSeverity.Debug, "Debugging", "Working as intentional."));
                        return;
                    }

                    // If the socket reaction, is from any of the filtered channels.
                    if (_clanApplicationChannels.Contains(currentReaction.Channel.Id))
                    {
                        // If the user has any roles from the filter.
                        if (currentUser.Roles.Any(r => r.Id.Equals(414618518554673152)))
                        {
                            var message = $"Leadership assigned reaction <{currentReaction.Emote.Name}> to message.";
                            var createdBy = currentUser.Id.ToString();

                            await _logger.LogAsync(new LogModel(LoggingSeverity.Info, "Reaction Added", message, createdBy, DateTime.UtcNow));

                            return;
                        }

                        // Process a new clan application.
                        await _clanApplication.ProcessClanApplicationAsync(currentReaction, currentUser);

                        // Remove Reaction after process.
                        await userMessage.RemoveReactionAsync(currentReaction.Emote, currentReaction.UserId);
                    }
                }
                catch (Exception)
                {
                    await _logger.LogAsync(
                        new LogModel(
                            LoggingSeverity.Error,
                            "Reaction Added",
                            "Error, could not handle reaction on clan application.",
                            "TQC Minion",
                            DateTime.UtcNow));
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