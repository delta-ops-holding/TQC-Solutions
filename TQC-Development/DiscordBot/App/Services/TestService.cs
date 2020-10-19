using Discord;
using Discord.WebSocket;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public class TestService : IService
    {
        private DiscordSocketClient _client;

        public TestService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task ReactionAddedAsync(IUserMessage userMessage, SocketReaction reaction)
        {
            var clanName = GetClanName(reaction.Emote);

            await NotifyUserAsync(reaction.User.Value, clanName);

            // Switch on different channels.
            switch (reaction.Channel.Id)
            {
                // Steam / PC | pc-clans
                case 765277945194348544:
                    await NotifyAdminAsync(1, reaction.User.Value, clanName);
                    await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
                    break;

                // Playstation | ps4-clans
                case 765277969278042132:
                    await NotifyAdminAsync(2, reaction.User.Value, clanName);
                    await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
                    break;

                // Xbox | xbox-clans
                case 765277993454534667:
                    await NotifyAdminAsync(3, reaction.User.Value, clanName);
                    await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
                    break;
            }
        }

        public Name.ClanNames GetClanName(IEmote emote)
        {
            return ((Emote)emote).Id switch
            {
                765283689957294121 => Name.ClanNames.TRΔNSIENT,
                765283671455825962 => Name.ClanNames.TENΔCITY,
                765283591579762718 => Name.ClanNames.ΔEGIS,
                765283655727317032 => Name.ClanNames.ETHEREΔL,
                765283608436408330 => Name.ClanNames.CELESTIΔL,
                765283621891604530 => Name.ClanNames.DEFIΔNCE,
                765283749612355605 => Name.ClanNames.VIGILΔNT,
                765283935558303794 => Name.ClanNames.TRΔNQUILITY,
                765283645065134130 => Name.ClanNames.ETERNΔL,
                765283634247893002 => Name.ClanNames.EPHEMERΔ,
                767090159211905054 => Name.ClanNames.SHΔDOW,
                _ => Name.ClanNames.Undefined,
            };
        }

        public string GetPingRole(Name.ClanNames clanName)
        {
            return clanName switch
            {
                Name.ClanNames.TRΔNSIENT => "<@&690062392989843477>",
                Name.ClanNames.TENΔCITY => "<@&690063656725250253>",
                Name.ClanNames.ΔEGIS => "<@&690063768712904803>",
                Name.ClanNames.ETHEREΔL => "<@&696163791083143168>",
                Name.ClanNames.CELESTIΔL => "<@&725837651185369178>",
                Name.ClanNames.DEFIΔNCE => "<@&690062826144006186>",
                Name.ClanNames.VIGILΔNT => "<@&690063134802837535>",
                Name.ClanNames.TRΔNQUILITY => "<@&690274199046062270>",
                Name.ClanNames.ETERNΔL => "<@&725837512139997325>",
                Name.ClanNames.EPHEMERΔ => "<@&694675786111647814>",
                Name.ClanNames.SHΔDOW => "<@&725837886427234354>",
                _ => string.Empty,
            };
        }

        public async Task NotifyUserAsync(IUser user, Name.ClanNames clanName)
        {
            await user.SendMessageAsync($"Hello Guardian. You're successfully signed up for the clan, {clanName}. Please await patiently for an admin to proceed your request.");
        }

        public async Task NotifyAdminAsync(byte platformId, IUser user, Name.ClanNames clanName)
        {
            // Channel to post to.
            ulong channelId = 0; // To give a default value to catch on.
            ulong mainChannelId = 767474913308835880; // Channel on the admin server to post in.
            ulong debugChannelId = 0; // Dev Server, to test bot.

            var pingRole = GetPingRole(clanName);
            var messageChannel = _client.GetChannel(debugChannelId) as IMessageChannel;

            var embedMessage = new EmbedBuilder()
            {
                Title = "New Clan Application Arrived!",
                Description = $"{user.Username}#{user.Discriminator}, registered themself for joining {clanName}. Confirmation message has also been sent to the Guardian."
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

            await messageChannel.SendMessageAsync(
                text: $"{pingRole}!",
                embed: embedMessage.Build());
        }
    }
}
