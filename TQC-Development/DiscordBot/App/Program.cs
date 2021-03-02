using Discord;
using Discord.Rest;
using Discord.WebSocket;
using DiscordBot.Interfaces;
using DiscordBot.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace DiscordBot
{
    public class Program
    {
        private const string DiscordToken = "Insert Token Here";

        private DiscordSocketClient _client;
        private DiscordSocketConfig _config;
        private DataService _dataService;
        private Interfaces.IApplication _applicationService;
        private INotifier _notifier;
        private ILogger _logger;

        public async Task Main(string[] args)
        {
            _config = new DiscordSocketConfig()
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100
            };

            _client = new DiscordSocketClient(_config);
            _dataService = new DataService();
            _logger = new LogService();

            _notifier = new NotificationService(_client, _logger, _dataService);
            _applicationService = new ClanApplicationService(_notifier, _logger, _dataService);

            await InitBotAsync();
        }

        public async Task InitBotAsync()
        {
            // Events.       
            _client.Log += _logger.Log;
            _client.Ready += async () =>
            {
                await DownloadGuildUsers();
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
                await _logger.Log(new LogMessage(LogSeverity.Verbose, "DownloadGuildUsers", "Loading Guilds.."));

                await Task.WhenAll(_client.Guilds.Select(g => g.DownloadUsersAsync()));
                int count = _client.Guilds.Sum(g => g.Users.Count);

                await _logger.Log(new LogMessage(LogSeverity.Verbose, "DownloadGuildUsers", $"Finished Download. Cached => {count} users."));
            });

            return Task.CompletedTask;
        }

        private Task GuildAvailable(SocketGuild arg)
        {
            _ = Task.Run(async () =>
           {
               await _logger.Log(new LogMessage(LogSeverity.Info, "Event", $"Guild is available {arg.Name}"));
           });

            return Task.CompletedTask;
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> cacheUserMessage, ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction)
        {
            _ = Task.Run(async () =>
            {
                IUserMessage userMessage = await cacheUserMessage.GetOrDownloadAsync();

                //Check if the reaction is from the corrects channels. (e.g.Steam, Xbox, Psn)
                if (!CheckChannelReaction(socketReaction)) return;

                await ValidateUserSpecifiedAsync(socketReaction, userMessage);
            });

            return Task.CompletedTask;
        }

        /// <summary>
        /// Validates if a user is specified in the reaction message.
        /// </summary>
        /// <param name="socketReaction"></param>
        /// <param name="userMessage"></param>
        /// <returns>An asyncronous process containing the Task.</returns>
        private async Task ValidateUserSpecifiedAsync(SocketReaction socketReaction, IUserMessage userMessage)
        {
            if (socketReaction.User.IsSpecified)
            {
                await CreateClanApplicationAsync(userMessage, socketReaction);

                return;
            }

            await _logger.Log(new LogMessage(LogSeverity.Info, "Reaction Added", $"User was not found i cache <{socketReaction.UserId}>"));
        }

        /// <summary>
        /// Checks if the reaction is in the right channel.
        /// </summary>
        /// <param name="socketReaction">The </param>
        /// <returns>True if the channel is correct.</returns>
        private static bool CheckChannelReaction(SocketReaction socketReaction)
        {
            return socketReaction.Channel.Id == 765277945194348544
                   || socketReaction.Channel.Id == 765277993454534667
                   || socketReaction.Channel.Id == 765277969278042132
                   || socketReaction.Channel.Id == 761687188341522492;
        }

        /// <summary>
        /// Sends a clan application.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="socketReaction"></param>
        /// <returns>An asyncronous process containing the Task.</returns>
        private async Task CreateClanApplicationAsync(IUserMessage userMessage, SocketReaction socketReaction)
        {
            //Check if the admin tries to react.
            if (socketReaction.User.Value is SocketGuildUser user)
            {
                //Check if the user is from the leadership role, or is the owner of the guild.
                await ValidateUserRoleAsync(userMessage, socketReaction, user);
            }
        }

        /// <summary>
        /// Check if the user is not an admin.
        /// </summary>
        /// <param name="userMessage"></param>
        /// <param name="socketReaction"></param>
        /// <param name="user"></param>
        /// <returns>An asyncronous process containing the Task.</returns>
        private async Task ValidateUserRoleAsync(IUserMessage userMessage, SocketReaction socketReaction, SocketGuildUser user)
        {
            // Check the user role.
            // If user is from leadership.
            if (user.Roles.Any(r => r.Name == "Leadership"))
            {
                await _logger
                    .Log(
                    new LogMessage(
                        LogSeverity.Info,
                        "Clan Application",
                        $"Admin <{user.Id}> added reaction emote <{socketReaction.Emote.Name}> to message"
                        ));

                return;
            }

            // Create clan application for user.
            await _applicationService.CreateClanApplicationAsync(userMessage, socketReaction);
        }
    }
}
