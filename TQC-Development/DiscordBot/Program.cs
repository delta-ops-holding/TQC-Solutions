using Discord;
using Discord.WebSocket;
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
        private IService _service;

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
            _service = new ReactionService(_client);

            // Events.
            _client.ReactionAdded += ReactionAdded;
            _client.Log += Log;
            _client.Ready += () =>
            {
                return Task.CompletedTask;
            };

            await _client.LoginAsync(
                TokenType.Bot,
                Environment.GetEnvironmentVariable(DiscordToken));

            await _client.StartAsync();



            // Block this task until the program is closed.
            await Task.Delay(-1);
        }

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> userMessage, ISocketMessageChannel socketMessageChannel, SocketReaction socketReaction)
        {
            var message = await userMessage.GetOrDownloadAsync();

            await _service.ReactionAddedAsync(message, socketReaction);
        }

        private Task Log(LogMessage logMsg)
        {
            Console.WriteLine(logMsg.ToString());

            return Task.CompletedTask;
        }
    }
}
