namespace ObjectLibrary.Clan.Interfaces
{
    public interface IClanEmote
    {
        int ClanId { get; }
        long DiscordSnowflakeEmoteId { get; }
    }
}