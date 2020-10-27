using Discord;
using Discord.WebSocket;
using DiscordBot.Interfaces;
using DiscordBot.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            try
            {
                // Configuration.
                _config = new DiscordSocketConfig()                                         // Set DiscordSocket Configuration.
                {
                    MessageCacheSize = 100,
                    LogLevel = LogSeverity.Info,
                    DefaultRetryMode = RetryMode.RetryTimeouts,
                    HandlerTimeout = 15
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
                _client.ReactionAdded += ReactionAdded;                                     // Bind ReactionAdded Event.
                _client.Log += _loggable.Log;                                               // Bind Log Event.
                _client.Ready += () =>                                                      // Bind ON Ready Event.
                {
                    return Task.CompletedTask;
                };

                await _client.LoginAsync(TokenType.Bot,                                     // Login with Bot.
                    DiscordToken);

                await _client.StartAsync();                                                 // Start Service.

                // Block this task until the program is closed.
                await Task.Delay(-1);                                                       // Await Task for Indefinitely. (Or program is closed)
            }
            catch (Exception ex)
            {
                await _loggable.Log(
                    new LogMessage(
                        severity: LogSeverity.Error,
                        source: "System Exception",
                        message: ex.ToString(),
                        exception: ex)
                    );
            }
        }

        private Task ReactionAdded(Cacheable<IUserMessage, ulong> userMessage, ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction)
        {
            _ = Task.Run(async () =>
            {
                var user = await socketReaction.Channel.GetUserAsync(socketReaction.UserId) as SocketGuildUser;

                // Check if the reaction is from the corrects channels. (e.g. Steam, Xbox, Psn)
                if (
                    socketReaction.Channel.Id == 765277945194348544 ||
                    socketReaction.Channel.Id == 765277969278042132 ||
                    socketReaction.Channel.Id == 765277993454534667)
                {
                    var message = await userMessage.GetOrDownloadAsync();

                    //Check if the admin tries to react.
                    if (user is SocketGuildUser test)
                    {
                        // Check if the user is from the leadership role, or is the owner of the guild.
                        if (test.Roles.Any(r => r.Name == "Leadership") || test.Id == 275816977631805451)
                            await _loggable.Log(
                                new LogMessage(
                                    LogSeverity.Info,
                                    "Clan Application", $"Admin <{test.Id}> added reaction emote <{socketReaction.Emote.Name}> to message"));
                        else
                        {
                            // Assign user to clan.
                            await _reactable.ClanApplicationAsync(
                                message,
                                socketReaction);
                        }
                    }
                }
            });
            return Task.CompletedTask;
        }
    }
}
