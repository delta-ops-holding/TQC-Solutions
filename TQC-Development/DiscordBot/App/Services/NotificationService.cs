using Discord;
using Discord.Net;
using Discord.WebSocket;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public class NotificationService : INotifiable
    {
        private readonly DiscordSocketClient _client;
        private readonly ILoggable _loggable;

        public NotificationService(DiscordSocketClient client, ILoggable loggable)
        {
            _client = client;
            _loggable = loggable;
        }

        public async Task NotifyAdminAsync(byte platformId, IUser discordUser, Name.ClanNames clanName)
        {
            // Channel to post to.
            ulong channelId = 0; // To give a default value to catch on.
            ulong mainChannelId = 767474913308835880; // Channel on the admin server to post in.
            ulong debugChannelId = 768014874289766402; // Dev Server, to test bot.

            var pingRole = DataService.GetPingRole(clanName);
            var messageChannel = _client.GetChannel(mainChannelId) as IMessageChannel;

            var embedMessage = new EmbedBuilder()
            {
                Title = "New Clan Application Arrived!",
                Description = $"{discordUser.Username}#{discordUser.Discriminator}, registered themself for joining {clanName}. Confirmation message has also been sent to the Guardian."
            };

            switch (platformId)
            {
                case 1:
                    embedMessage.Color = Color.LighterGrey;
                    embedMessage.WithFooter($"{clanName}", "https://cdn.discordapp.com/emojis/641432631715561473.png?v=1").WithCurrentTimestamp();
                    break;
                case 2:
                    embedMessage.Color = Color.Blue;
                    embedMessage.WithFooter($"{clanName}", "https://cdn.discordapp.com/emojis/551501319177895958.png?v=1").WithCurrentTimestamp();
                    break;
                case 3:
                    embedMessage.Color = Color.Green;
                    embedMessage.WithFooter($"{clanName}", "https://cdn.discordapp.com/emojis/551501460202979328.png?v=1").WithCurrentTimestamp();
                    break;
            }

            pingRole = string.IsNullOrEmpty(pingRole) ? "Debugging" : pingRole;

            await messageChannel.SendMessageAsync(
                text: $"{pingRole}!",
                embed: embedMessage.Build());
        }

        public async Task NotifyUserAsync(IUser discordUser, Name.ClanNames clanName)
        {
            try
            {
                await discordUser.SendMessageAsync($"Hello Guardian. You're successfully signed up for the clan, {clanName}. Please await patiently for an admin to proceed your request.");
            }
            catch (HttpException)
            {
                await _loggable.Log(new LogMessage(LogSeverity.Error, "Reaction Added", "Couldn't DM Guardian. [Privacy is on or sender is blocked]"));
            }
        }
    }
}
