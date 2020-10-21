using Discord;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Interfaces
{
    public class DataService
    {
        /// <summary>
        /// Gets the clan name, by a given emote.
        /// </summary>
        /// <remarks>Returns Undefined, if no emotes is matched.</remarks>
        /// <param name="emote"></param>
        /// <returns>An enumerable of <see cref="Name.ClanNames"/>.</returns>
        public Name.ClanNames GetClanName(IEmote emote)
        {
            var e = emote as Emote;

            return e.Id switch
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

        /// <summary>
        /// Gets the role to ping, by given clan name.
        /// </summary>
        /// <param name="clanName">Used to identify the pinged role.</param>
        /// <returns>A role to ping.</returns>
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
    }
}
