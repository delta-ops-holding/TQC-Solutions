using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Clan.Interfaces
{
    public interface IMentionRole
    {
        int ClanId { get; }
        long DiscordSnowflakeRoleId { get; }
    }
}
