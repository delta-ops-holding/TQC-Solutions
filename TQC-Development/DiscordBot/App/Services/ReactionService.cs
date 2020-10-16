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
        private readonly DiscordSocketClient _client;

        public ReactionService(DiscordSocketClient client)
        {
            _client = client;
        }

        public async Task ReactionAddedAsync(IUserMessage userMessage, SocketReaction reaction)
        {
            // Switch on different channels.
            switch (reaction.Channel.Id)
            {
                // Steam / PC | pc-clans
                case 765277945194348544:
                    await GetClanName(reaction, (Emote)reaction.Emote, 1);
                    await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
                    break;

                // Playstation | ps4-clans
                case 765277969278042132:
                    await GetClanName(reaction, (Emote)reaction.Emote, 2);
                    await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
                    break;

                // Xbox | xbox-clans
                case 765277993454534667:
                    await GetClanName(reaction, (Emote)reaction.Emote, 3);
                    await userMessage.RemoveReactionAsync(reaction.Emote, reaction.UserId);
                    break;
            }
        }

        private async Task GetClanName(SocketReaction reaction, Emote emote, byte platformNumber)
        {
            // Define empty clan name.
            ClanNames clanName = ClanNames.Undefined;

            // Switch on emotes being reacted on.
            switch (emote.Id)
            {
                case 765283689957294121:
                    clanName = ClanNames.TRΔNSIENT;
                    break;
                case 765283671455825962:
                    clanName = ClanNames.TENΔCITY;
                    break;
                case 765283591579762718:
                    clanName = ClanNames.ΔEGIS;
                    break;
                case 765283655727317032:
                    clanName = ClanNames.ETHEREΔL;
                    break;
                case 765283608436408330:
                    clanName = ClanNames.CELESTIΔL;
                    break;
                case 765283621891604530:
                    clanName = ClanNames.DEFIΔNCE;
                    break;
                case 765283749612355605:
                    clanName = ClanNames.VIGILΔNT;
                    break;
                case 765283935558303794:
                    clanName = ClanNames.TRΔNQUILITY;
                    break;
                case 765283645065134130:
                    clanName = ClanNames.ETERNΔL;
                    break;
                case 765283634247893002:
                    clanName = ClanNames.EPHEMERΔ;
                    break;
                case 0:
                    clanName = ClanNames.SHΔDOW;
                    break;
            }

            if (ClanNames.Undefined != clanName)
            {
                await NotifyUserAsync(reaction.User.Value, clanName);
                await NotifyAdminAsync(platformNumber, reaction.User.Value, clanName);
            }
            else
                await reaction.User.Value.SendMessageAsync($"Could not find the Clan. Contact an admin or try again later.");
        }

        private async Task NotifyUserAsync(IUser user, ClanNames clanName)
        {
            await user.SendMessageAsync($"Hello Guardian. You're successfully signed up for the clan, {clanName}. Please await patiently for an admin to proceed your request.");
        }

        private async Task NotifyAdminAsync(byte platformId, IUser user, ClanNames clanName)
        {
            ulong channelId = 0;
            string pingMessage = string.Empty;

            var embed = new EmbedBuilder()
            {
                Title = "New Clan Application Arrived!",
                Description = $"<@{user.Id}>, registered themself for joining {clanName}. Confirmation message has also been sent to the Guardian."
            };

            // Switch on platform id.
            switch (platformId)
            {
                case 1:

                    switch (clanName)
                    {
                        case ClanNames.TRΔNSIENT:
                            pingMessage = "<@&690062392989843477>!";
                            channelId = 690082003290292235;
                            break;
                        case ClanNames.TENΔCITY:
                            pingMessage = "<@&690063656725250253>!";
                            channelId = 690082295448600607;
                            break;
                        case ClanNames.ΔEGIS:
                            pingMessage = "<@&690063768712904803>!";
                            channelId = 690082616732680227;
                            break;
                        case ClanNames.ETHEREΔL:
                            pingMessage = "<@&696163791083143168>!";
                            channelId = 696155917359513630;
                            break;
                        case ClanNames.CELESTIΔL:
                            pingMessage = "<@&725837651185369178>!";
                            channelId = 725833921379696660;
                            break;
                    }

                    embed.Color = Color.LightGrey;
                    embed.WithFooter($"{clanName}", "https://cdn.discordapp.com/emojis/641432631715561473.png?v=1").WithCurrentTimestamp();
                    break;
                case 2:

                    switch (clanName)
                    {
                        case ClanNames.DEFIΔNCE:
                            pingMessage = "<@&690062826144006186>!";
                            channelId = 690082108139503649;
                            break;
                        case ClanNames.VIGILΔNT:
                            pingMessage = "<@&690063134802837535>!";
                            channelId = 690082215572275201;
                            break;
                        case ClanNames.TRΔNQUILITY:
                            pingMessage = "<@&690274199046062270>!";
                            channelId = 690275298930983224;
                            break;
                        case ClanNames.ETERNΔL:
                            pingMessage = "<@&725837512139997325>!";
                            channelId = 725838252606619740;
                            break;
                    }

                    embed.Color = Color.Blue;
                    embed.WithFooter($"{clanName}", "https://cdn.discordapp.com/emojis/551501319177895958.png?v=1").WithCurrentTimestamp();
                    break;
                case 3:

                    switch (clanName)
                    {
                        case ClanNames.EPHEMERΔ:
                            pingMessage = "<@&694675786111647814>!";
                            channelId = 694238798892236800;
                            break;
                    }

                    embed.Color = Color.Green;
                    embed.WithFooter($"{clanName}", "https://cdn.discordapp.com/emojis/551501460202979328.png?v=1").WithCurrentTimestamp();
                    break;
                default:
                    break;
            }

            if (channelId != 0)
            {
                var channel = _client.GetChannel(channelId) as IMessageChannel;
                await channel.SendMessageAsync(text: pingMessage, embed: embed.Build());
            }
        }

        public enum ClanNames
        {
            Undefined,
            TRΔNSIENT, TENΔCITY, ΔEGIS, ETHEREΔL, CELESTIΔL,
            DEFIΔNCE, VIGILΔNT, TRΔNQUILITY, ETERNΔL,
            EPHEMERΔ, SHΔDOW
        }
    }
}
