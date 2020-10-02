using Discord;
using Discord.WebSocket;
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
            => new Program().MainAsync().GetAwaiter().GetResult();

        private const string DiscordToken = "DiscordToken";
        private DiscordSocketClient _client;
        private DiscordSocketConfig _config;

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

        private async Task ReactionAdded(Cacheable<IUserMessage, ulong> arg1, ISocketMessageChannel arg2, SocketReaction arg3)
        {
            Console.WriteLine($"Reaction Fired!");

            var message = await arg1.GetOrDownloadAsync();

            if (arg3.MessageId == 761705280781811712)
            {
                if (arg3.Emote is Emote emote)
                {
                    switch (emote.Name)
                    {
                        case "PanWoah":
                            Console.WriteLine($"Test");
                            await arg3.User.GetValueOrDefault().SendMessageAsync($"Test");

                            ulong channelId = 761715122736463904;
                            var channel = _client.GetChannel(channelId) as IMessageChannel;
                            await channel.SendMessageAsync($"Test to server channel.");

                            await message.RemoveReactionAsync(emote, arg3.UserId);                            
                            break;
                        default:
                            Console.WriteLine($"No Work.");
                            break;
                    }
                }
            }

            await Task.CompletedTask;
        }

        private Task Log(LogMessage logMsg)
        {
            Console.WriteLine(logMsg.ToString());

            return Task.CompletedTask;
        }
    }
}
