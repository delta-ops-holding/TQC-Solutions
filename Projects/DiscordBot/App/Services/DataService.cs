using Discord;
using DiscordBot.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataClassLibrary.Enums;
using DatabaseAccess.Repositories.Interfaces;
using DataClassLibrary.Models;
using System;

namespace DiscordBot.Services
{
    /// <summary>
    /// Represents a service, for handling data flow.
    /// </summary>
    public class DataService : IDataService
    {
        private readonly ILogRepository _logRepository;

        public DataService(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

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
                783506275057008691 => Clan.TRΔNSIENT,
                783506209914880000 => Clan.TENΔCITY,
                783505835170857041 => Clan.ΔEGIS,
                783506077228597248 => Clan.ETHEREΔL,
                783505861691310121 => Clan.CELESTIΔL,
                783506132681228298 => Clan.MΔJESTIC,
                783505883267072012 => Clan.DEFIΔNCE,
                783506295104864286 => Clan.VIGILΔNT,
                783506251904581672 => Clan.TRΔNQUILITY,
                783506052388749382 => Clan.ETERNΔL,
                783506106869743636 => Clan.IMMORTΔL,
                783506022370246726 => Clan.EPHEMERΔ,
                783506171499774005 => Clan.SHΔDOW,
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

        public Task<IEnumerable<LogModel>> GetLatestLogs()
        {
            throw new NotImplementedException();
            //return await _logRepository.GetLatest();
        }
    }
}
