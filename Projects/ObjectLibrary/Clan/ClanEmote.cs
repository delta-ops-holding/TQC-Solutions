using ObjectLibrary.Clan.Interfaces;

namespace ObjectLibrary.Clan
{
    public struct ClanEmote : IClanEmote
    {
        private readonly int _clanId;
        private readonly long _discordSnowflakeEmoteId;

        public ClanEmote(int clanId, long discordSnowflakeEmoteId)
        {
            _clanId = clanId;
            _discordSnowflakeEmoteId = discordSnowflakeEmoteId;
        }

        public int ClanId { get { return _clanId; } }
        public long DiscordSnowflakeEmoteId { get { return _discordSnowflakeEmoteId; } }
    }
}
