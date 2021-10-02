using Discord.WebSocket;
using LeadershipMinion.Logical.Enums;

namespace LeadershipMinion.Logical.Data.Abstractions
{
    public interface IClanService
    {
        Clan GetClanNameByEmoteId(ulong id);
        string GetMentionRoleByClanName(Clan clanName);
        ClanPlatform GetClanPlatformByChannelId(ulong id);
    }
}