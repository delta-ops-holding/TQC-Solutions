using LeadershipMinion.Logical.Data.Abstractions;
using LeadershipMinion.Logical.Enums;

namespace LeadershipMinion.Logical.Data.Services
{
    public class ClanService : IClanService
    {
        public Clan GetClanNameByEmoteId(ulong id)
        {
            return id switch
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
                783506171499774005 => Clan.SHΔDOW,
                849017066739859520 => Clan.QUΔNTUM,
                _ => Clan.Undefined,
            };
        }

        public ClanPlatform GetClanPlatformByChannelId(ulong id)
        {
            return id switch
            {
                765277945194348544 => ClanPlatform.PC,
                765277969278042132 => ClanPlatform.PSN,
                765277993454534667 => ClanPlatform.XBOX,
                _ => ClanPlatform.Undefined
            };
        }

        public string GetMentionRoleByClanName(Clan clanName)
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
                Clan.SHΔDOW => "<@&725837886427234354>",
                Clan.QUΔNTUM => "<@&848288604341272621>",
                _ => nameof(Clan.Undefined),
            };
        }
    }
}
