using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class EventService : IService
    {
        private readonly ulong _channelId = 761715122736463904;
        private readonly DiscordSocketClient _client;

        public EventService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task ReactionAddedAsync(IUserMessage userMessage, ISocketMessageChannel messageChannel, SocketReaction reaction)
        {
            Console.WriteLine($"Service Works!");

            if (reaction.MessageId == 761705280781811712)
            {
                if (reaction.Emote is Emote reactedEmote)
                {
                    await SendConfirmationMessages(reaction, reactedEmote);
                    await userMessage.RemoveReactionAsync(reactedEmote, reaction.UserId);
                }
            }

            await Task.CompletedTask;
        }

        private async Task SendConfirmationMessages(SocketReaction reaction, Emote emote)
        {
            var channel = _client.GetChannel(_channelId) as IMessageChannel;
            var message = string.Empty;

            switch (emote.Id)
            {
                case 653026889769943060:
                    message = $"Pika Emoji Confirmed.";
                    break;
                default:
                    break;
            }

            await reaction.User.Value.SendMessageAsync(message);
            await channel.SendMessageAsync($"{message} by <@{reaction.User.Value.Id}>");

            await Task.CompletedTask;
        }
    }
}
