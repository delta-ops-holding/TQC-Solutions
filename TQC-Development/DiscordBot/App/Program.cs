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
        public static void Main(string[] args)
            => new Program()
            .MainAsync()
            .GetAwaiter()
            .GetResult();

        private const string DiscordToken = "DiscordToken";
        private DiscordSocketClient _client;
        private DiscordSocketConfig _config;
        private IReactable _reactable;
        private INotifiable _notifiable;
        private ILoggable _loggable;

        public async Task MainAsync()
        {
            // Configuration.
            _config = new DiscordSocketConfig()
            {
                MessageCacheSize = 100,
                LogLevel = LogSeverity.Info
            };

            // Client assignment.
            _client = new DiscordSocketClient(_config);

            // Service Injections.
            //_service = new ReactionService(_client);
            _loggable = new LogService();
            _notifiable = new NotificationService(_client);
            _reactable = new TestService(_notifiable, _loggable);

            // Events.
            _client.ReactionAdded += ReactionAdded;
            _client.Log += _loggable.Log;
            _client.Ready += () =>
            {
                return Task.CompletedTask;
            };

            await _client.LoginAsync(
                TokenType.Bot,
                "Insert Token Here");

            await _client.StartAsync();


            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> userMessage, ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction)
        {
            // Check if the admin tries to react.
            if (socketReaction.User.Value is SocketGuildUser user)
            {
                if (user.Roles.Any(r => r.Name == "Leadership") || user.Id == 275816977631805451)
                    await _loggable.Log(new LogMessage(LogSeverity.Info, "Clan Application", $"Admin <{user.Id}> added reaction emote <{socketReaction.Emote.Name}> to message"));
                else
                {
                    var message = await userMessage.GetOrDownloadAsync();

                    await _reactable.ReactionAddedAsync(message, socketReaction);
                }
            }
        }
    }
}
