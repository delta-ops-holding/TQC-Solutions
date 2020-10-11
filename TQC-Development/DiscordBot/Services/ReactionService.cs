using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class ReactionService : IService
    {
        private readonly ulong _channelId = 761715122736463904;
        private readonly DiscordSocketClient _client;

        public ReactionService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task ReactionAddedAsync(IUserMessage userMessage, ISocketMessageChannel messageChannel, SocketReaction reaction)
        {
            Console.WriteLine($"Service Works!");

            // Get Correct Message.
            if (reaction.MessageId == 761705280781811712)
            {
                // Make sure it's an emote.
                if (reaction.Emote is Emote reactedEmote)
                {
                    // Send Messages.
                    await SendConfirmationMessages(reaction, reactedEmote);

                    // Remove User Reaction after confirmed.
                    await userMessage.RemoveReactionAsync(reactedEmote, reaction.UserId);
                }
            }

            await Task.CompletedTask;
        }

        private async Task SendConfirmationMessages(SocketReaction reaction, Emote emote)
        {
            // Get the Channel for the admin server.
            var channel = _client.GetChannel(_channelId) as IMessageChannel;

            // Define empty message.
            var message = string.Empty;

            // Switch on emotes being reacted on.
            switch (emote.Id)
            {
                case 653026889769943060: // Pike Emote
                    message = $"Pika Emoji Confirmed.";
                    break;
                default:
                    break;
            }

            // Send Confirmation message to user who reacted.
            await reaction.User.Value.SendMessageAsync(message);

            // Send Confirmation message to admin channel, about what user reacted and to what clan.
            await channel.SendMessageAsync($"{message} by <@{reaction.User.Value.Id}>");

            await Task.CompletedTask;
        }
    }
}
