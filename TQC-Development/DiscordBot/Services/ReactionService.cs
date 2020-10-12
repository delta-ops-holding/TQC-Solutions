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
        private readonly ulong _messageIdForSteamClanReactions = 0;
        private readonly ulong _messageIdForPsnClanReactions = 0;
        private readonly ulong _messageIdForXboxClanReactions = 0;

        private readonly ulong _channeIdForSteamClanRegistrations = 761715122736463904;
        private readonly ulong _channeIdForPsnClanRegistrations = 761715122736463904;
        private readonly ulong _channeIdForXboxClanRegistrations = 761715122736463904;

        private readonly DiscordSocketClient _client;

        public ReactionService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task ReactionAddedAsync(IUserMessage userMessage, SocketReaction reaction)
        {
            // Check for different message channels.
            switch (reaction.MessageId)
            {
                case 1: // Debug Message
                    await GetClanName(reaction, (Emote)reaction.Emote, 1);
                    break;
                case 2:
                    await GetClanName(reaction, (Emote)reaction.Emote, 2);
                    break;
                case 3:
                    await GetClanName(reaction, (Emote)reaction.Emote, 3);
                    break;
                default:
                    await Task.CompletedTask;
                    break;
            }

            await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
        }

        private async Task GetClanName(SocketReaction reaction, Emote emote, byte platformNumber)
        {
            // Define empty clan name.
            var clanName = string.Empty;

            // Switch on emotes being reacted on.
            switch (emote.Id)
            {
                case 653026889769943060: // Pika Emote
                    clanName = $"{ClanNames.TRΔNSIENT}";
                    break;
                case 2:
                    clanName = $"{ClanNames.TENΔCITY}";
                    break;
                case 3:
                    clanName = $"{ClanNames.ΔEGIS}";
                    break;
                case 4:
                    clanName = $"{ClanNames.ETHEREΔL}";
                    break;
                case 5:
                    clanName = $"{ClanNames.CELESTIΔL}";
                    break;
                case 6:
                    clanName = $"{ClanNames.DEFIΔNCE}";
                    break;
                case 7:
                    clanName = $"{ClanNames.VIGILΔNT}";
                    break;
                case 8:
                    clanName = $"{ClanNames.TRΔNQUILITY}";
                    break;
                case 9:
                    clanName = $"{ClanNames.ETERNΔL}";
                    break;
                case 10:
                    clanName = $"{ClanNames.EPHEMERΔ}";
                    break;
                case 11:
                    clanName = $"{ClanNames.SHΔDOW}";
                    break;
            }

            if (!String.IsNullOrEmpty(clanName))
            {
                await NotifyUserAsync(reaction.User.Value, clanName);
                await NotifyAdminAsync(platformNumber, reaction.User.Value, clanName);
            }
            else
                await reaction.User.Value.SendMessageAsync($"Could not find the Clan. Contact an admin or try again later.");
        }

        private async Task NotifyUserAsync(IUser user, string clanName)
        {
            await user.SendMessageAsync($"Hello Guardian. You're successfully signed up for the clan, {clanName}. Please await patiently for an admin to proceed your request.");
        }

        private async Task NotifyAdminAsync(byte platformId, IUser user, string clanName)
        {
            ulong channelId = 0;

            var embed = new EmbedBuilder()
            {
                Title = "Clan Registration!",
                Description = $"{user.Mention}, registered themself for joining {clanName}. Confirmation message has also been sent to the Guardian."
            };

            // Switch on platform id.
            switch (platformId)
            {
                case 1:
                    channelId = _channeIdForSteamClanRegistrations;
                    embed.Color = Color.LightGrey;
                    embed.WithFooter(clanName, "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/240px-Steam_icon_logo.svg.png").WithCurrentTimestamp();
                    break;
                case 2:
                    channelId = _channeIdForPsnClanRegistrations;
                    embed.Color = Color.Blue;
                    embed.WithFooter(clanName, "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/240px-Steam_icon_logo.svg.png").WithCurrentTimestamp();
                    break;
                case 3:
                    channelId = _channeIdForXboxClanRegistrations;
                    embed.Color = Color.Green;
                    embed.WithFooter(clanName, "https://upload.wikimedia.org/wikipedia/commons/thumb/8/83/Steam_icon_logo.svg/240px-Steam_icon_logo.svg.png").WithCurrentTimestamp();
                    break;
                default:
                    break;
            }

            var channel = _client.GetChannel(channelId) as IMessageChannel;
            await channel.SendMessageAsync(embed: embed.Build());
        }

        public enum ClanNames
        {
            TRΔNSIENT, TENΔCITY, ΔEGIS, ETHEREΔL, CELESTIΔL,
            DEFIΔNCE, VIGILΔNT, TRΔNQUILITY, ETERNΔL,
            EPHEMERΔ, SHΔDOW
        }
    }
}
