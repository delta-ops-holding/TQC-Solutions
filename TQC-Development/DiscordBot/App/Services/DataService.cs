using Discord;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Services
{
    public class DataService
    {
        /// <summary>
        /// Gets the clan name, by a given emote.
        /// </summary>
        /// <remarks>Returns Undefined, if no emotes is matched.</remarks>
        /// <param name="emote"></param>
        /// <returns>An enumerable of <see cref="Enums.ClanNames"/>.</returns>
        public Enums.ClanNames GetClanName(IEmote emote)
        {
            var e = emote as Emote;

            return e.Id switch
            {
                765283689957294121 => Enums.ClanNames.TRΔNSIENT,
                765283671455825962 => Enums.ClanNames.TENΔCITY,
                765283591579762718 => Enums.ClanNames.ΔEGIS,
                765283655727317032 => Enums.ClanNames.ETHEREΔL,
                765283608436408330 => Enums.ClanNames.CELESTIΔL,
                772951082544267284 => Enums.ClanNames.MΔJESTIC,
                765283621891604530 => Enums.ClanNames.DEFIΔNCE,
                765283749612355605 => Enums.ClanNames.VIGILΔNT,
                765283935558303794 => Enums.ClanNames.TRΔNQUILITY,
                765283645065134130 => Enums.ClanNames.ETERNΔL,
                772951082326163457 => Enums.ClanNames.IMMORTΔL,
                765283634247893002 => Enums.ClanNames.EPHEMERΔ,
                767090159211905054 => Enums.ClanNames.SHΔDOW,
                _ => Enums.ClanNames.Undefined,
            };
        }

        /// <summary>
        /// Gets the role to ping, by given clan name.
        /// </summary>
        /// <param name="clanName">Used to identify the pinged role.</param>
        /// <returns>A role to ping.</returns>
        public string GetPingRole(Enums.ClanNames clanName)
        {
            return clanName switch
            {
                Enums.ClanNames.TRΔNSIENT => "<@&690062392989843477>",
                Enums.ClanNames.TENΔCITY => "<@&690063656725250253>",
                Enums.ClanNames.ΔEGIS => "<@&690063768712904803>",
                Enums.ClanNames.ETHEREΔL => "<@&696163791083143168>",
                Enums.ClanNames.CELESTIΔL => "<@&725837651185369178>",
                Enums.ClanNames.MΔJESTIC => "<@&772933170781749258>",
                Enums.ClanNames.DEFIΔNCE => "<@&690062826144006186>",
                Enums.ClanNames.VIGILΔNT => "<@&690063134802837535>",
                Enums.ClanNames.TRΔNQUILITY => "<@&690274199046062270>",
                Enums.ClanNames.ETERNΔL => "<@&725837512139997325>",
                Enums.ClanNames.IMMORTΔL => "<@&772933700681859123>",
                Enums.ClanNames.EPHEMERΔ => "<@&694675786111647814>",
                Enums.ClanNames.SHΔDOW => "<@&725837886427234354>",
                _ => string.Empty,
            };
        }
    }
}
