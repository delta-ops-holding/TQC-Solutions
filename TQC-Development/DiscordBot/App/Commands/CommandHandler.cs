using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Commands.Modules;
using DiscordBot.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using System.Threading.Tasks;
using static DiscordBot.Commands.Modules.InfoModule;

namespace DiscordBot.Commands
{
    public class CommandHandler : ICommandHandler
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _services;
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;

        public CommandHandler(DiscordSocketClient client, CommandService commandService, IConfiguration configuration, IServiceProvider services)
        {
            _client = client;
            _commandService = commandService;
            _configuration = configuration;
            _services = services;
        }

        public async Task InitializeCommandsAsync()
        {
            _client.MessageReceived += HandleCommandAsync;

            await _commandService.AddModulesAsync(assembly: Assembly.GetEntryAssembly(), services: _services);
        }

        private async Task HandleCommandAsync(SocketMessage socketMessage)
        {
            if (socketMessage is not SocketUserMessage message)
            {
                return;
            }

            int argPos = 0;

            // Determine if the message is a command, based on the prefix and make sure no bots trigger commands.
            if (!(message.HasStringPrefix(_configuration.GetValue<string>("Configuration:DiscordBot:CommandPrefix"), ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos)) ||
                message.Author.IsBot)
            {
                return;
            }

            var context = new SocketCommandContext(_client, message);

            await _commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: _services);
        }
    }
}
