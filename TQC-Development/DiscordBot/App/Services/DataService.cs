using Discord;
using DiscordBot.Enums;

namespace DiscordBot.Services
{
    /// <summary>
    /// Represents a service, for handling data flow.
    /// </summary>
    public class DataService
    {
        /// <summary>
        /// Gets the clan name, by a given emote.
        /// </summary>
        /// <remarks>Returns Undefined, if no emotes is matched.</remarks>
        /// <param name="emote"></param>
        /// <returns>An enumerable of <see cref="Enums.Clan"/>.</returns>
        public Clan GetClanName(IEmote emote)
        {
            var e = emote as Emote;

            return e.Id switch
            {
                765283689957294121 => Clan.TRΔNSIENT,
                765283671455825962 => Clan.TENΔCITY,
                765283591579762718 => Clan.ΔEGIS,
                765283655727317032 => Clan.ETHEREΔL,
                765283608436408330 => Clan.CELESTIΔL,
                772951082544267284 => Clan.MΔJESTIC,
                765283621891604530 => Clan.DEFIΔNCE,
                765283749612355605 => Clan.VIGILΔNT,
                765283935558303794 => Clan.TRΔNQUILITY,
                765283645065134130 => Clan.ETERNΔL,
                772951082326163457 => Clan.IMMORTΔL,
                765283634247893002 => Clan.EPHEMERΔ,
                767090159211905054 => Clan.SHΔDOW,
                _ => Clan.Undefined,
            };
        }

        /// <summary>
        /// Gets the role to ping, by given clan name.
        /// </summary>
        /// <param name="clanName">Used to identify the pinged role.</param>
        /// <returns>A role to ping.</returns>
        public string GetRoleByClan(Clan clanName)
        {
            return clanName switch
            {
                Clan.TRΔNSIENT => "<@&690062392989843477>",
                Clan.TENΔCITY => "<@&690063656725250253>",
                Clan.ΔEGIS => "<@&690063768712904803>",
                Clan.ETHEREΔL => "<@&696163791083143168>",
                Clan.CELESTIΔL => "<@&725837651185369178>",
                Clan.MΔJESTIC => "<@&772933170781749258>",
                Clan.DEFIΔNCE => "<@&690062826144006186>",
                Clan.VIGILΔNT => "<@&690063134802837535>",
                Clan.TRΔNQUILITY => "<@&690274199046062270>",
                Clan.ETERNΔL => "<@&725837512139997325>",
                Clan.IMMORTΔL => "<@&772933700681859123>",
                Clan.EPHEMERΔ => "<@&694675786111647814>",
                Clan.SHΔDOW => "<@&725837886427234354>",
                _ => Clan.Undefined.ToString(),
            };
        }
    }
}
