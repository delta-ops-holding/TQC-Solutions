using ObjectLibrary.Clan.Interfaces;

namespace ObjectLibrary.Clan
{
    public struct ApplicationChannel : IApplicationChannel
    {
        private readonly int _clanPlatformId;
        private readonly long _discordSnowflakeChannelId;

        public ApplicationChannel(int clanPlatformId, long discordSnowflakeChannelId)
        {
            _clanPlatformId = clanPlatformId;
            _discordSnowflakeChannelId = discordSnowflakeChannelId;
        }

        public int ClanPlatformId { get { return _clanPlatformId; } }
        public long DiscordSnowflakeChannelId { get { return _discordSnowflakeChannelId; } }
    }
}
