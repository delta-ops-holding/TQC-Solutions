namespace ObjectLibrary.Clan.Interfaces
{
    public interface IApplicationChannel
    {
        int ClanPlatformId { get; }
        long DiscordSnowflakeChannelId { get; }
    }
}