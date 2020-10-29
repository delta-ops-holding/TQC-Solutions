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

        public static void Main(string[] args)
            => new Program()
                .InitBotAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;
        private DiscordSocketConfig _config;
        private DataService _dataService;
        private IReactable _reactable;
        private INotifiable _notifiable;
        private ILoggable _loggable;

        public async Task InitBotAsync()
        {
            // Configuration.
            _config = new DiscordSocketConfig()                                         // Set DiscordSocket Configuration.
            {
                LogLevel = LogSeverity.Info,
                MessageCacheSize = 100
            };

            // Client assignment.
            _client = new DiscordSocketClient(_config);                                 // Initialize new Discord Client with Configuration.

            // Service Injections.
            _dataService = new DataService();                                           // Initialize new Data Service.
            _loggable = new LogService();                                               // Initialize new Log Service.
            _notifiable = new NotificationService(_client, _loggable, _dataService);    // Inject Notification Service.

            //_service = new ReactionService(_client);                                  // V1 - Reaction Service.
            _reactable = new ReactionServiceV2(_notifiable, _loggable, _dataService);   // V2 - Reaction Service with Dependencies.

            //_reactable = new TestService(_notifiable, _loggable);                     // Debugging for testing new service.

            // Events.       
            _client.Log += _loggable.Log;
            _client.Ready += () =>
            {
                Thread.Sleep(2000);
                DownloadGuildUsers();
                return Task.CompletedTask;
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
            Task.Run(async () =>
            {
                Console.WriteLine("Loading Guilds..");
                await Task.WhenAll(_client.Guilds.Select(g => g.DownloadUsersAsync()));
                int count = _client.Guilds.Sum(g => g.Users.Count);
                Console.WriteLine($"Done downloading. Cached -> {count} users.");
            });

            return Task.CompletedTask;
        }

        private async Task GuildAvailable(SocketGuild arg)
        {
            await _loggable.Log(new LogMessage(LogSeverity.Info, "Event", $"Guild is available {arg.Name}"));

            await Task.CompletedTask;
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> cacheUserMessage, ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction)
        {
            IUserMessage userMessage = cacheUserMessage.GetOrDownloadAsync().GetAwaiter().GetResult();

            //Check if the reaction is from the corrects channels. (e.g.Steam, Xbox, Psn)
            if (socketReaction.Channel.Id == 765277945194348544 || socketReaction.Channel.Id == 765277993454534667 || socketReaction.Channel.Id == 765277969278042132 || socketReaction.Channel.Id == 761687188341522492)
            {

                if (socketReaction.User.IsSpecified)
                {
                    SendClanApplication(userMessage, socketReaction);

                    return Task.CompletedTask;
                }
                else
                {
                    _loggable.Log(new LogMessage(LogSeverity.Info, "Reaction Added", $"User was not found i cache <{socketReaction.UserId}>"));
                }
            }
            return Task.CompletedTask;
        }

        private async void SendClanApplication(IUserMessage userMessage, SocketReaction socketReaction)
        {
            //Check if the admin tries to react.
            if (socketReaction.User.Value is SocketGuildUser user)
            {
                //Check if the user is from the leadership role, or is the owner of the guild.
                if (user.Roles.Any(r => r.Name == "Leadership"))
                    await _loggable.Log(
                        new LogMessage(
                            LogSeverity.Info,
                            "Clan Application", $"Admin <{user.Id}> added reaction emote <{socketReaction.Emote.Name}> to message"));
                else
                {
                    //Assign user to clan.
                    _reactable.SendClanApplication(
                        userMessage,
                        socketReaction);
                }
            }
        }
    }
}
