namespace ObjectLibrary.Clan.Interfaces
{
    public interface IPlatform
    {
        string ImagePath { get; }
        string Name { get; }
        IApplicationChannel ApplicationChannel { get; }
    }
}