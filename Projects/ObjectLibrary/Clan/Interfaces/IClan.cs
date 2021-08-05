namespace ObjectLibrary.Clan.Interfaces
{
    public interface IClan
    {
        string About { get; }
        IClanEmote ClanEmote { get; }
        IMentionRole MentionRole { get; }
        string Name { get; }
        IPlatform Platform { get; }
    }
}