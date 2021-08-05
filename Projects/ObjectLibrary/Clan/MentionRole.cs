using ObjectLibrary.Clan.Interfaces;

namespace ObjectLibrary.Clan
{
    public struct MentionRole : IMentionRole
    {
        private readonly int _clanId;
        private readonly long _discordSnowflakeRoleId;

        public MentionRole(int clanId, long discordSnowflakeRoleId)
        {
            _clanId = clanId;
            _discordSnowflakeRoleId = discordSnowflakeRoleId;
        }

        public int ClanId { get { return _clanId; } }
        public long DiscordSnowflakeRoleId { get { return _discordSnowflakeRoleId; } }
    }
}
