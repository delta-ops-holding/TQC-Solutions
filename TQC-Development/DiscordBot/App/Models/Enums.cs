using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    public abstract class Enums
    {
        /// <summary>
        /// Supported clans available.
        /// </summary>
        public enum ClanNames
        {
            Undefined,
            TRΔNSIENT, TENΔCITY, ΔEGIS, ETHEREΔL, CELESTIΔL, MΔJESTIC,
            DEFIΔNCE, VIGILΔNT, TRΔNQUILITY, ETERNΔL, IMMORTΔL,
            EPHEMERΔ, SHΔDOW
        }

        public enum ClanPlatforms
        {
            Undefined,
            PC,
            PSN,
            XBOX
        }
    }
}
