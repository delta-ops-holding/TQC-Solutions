namespace LeadershipMinion.Core.Abstractions
{
    /// <summary>
    /// Represents a generic Bot Configuration.
    /// </summary>
    public interface IBotConfiguration
    {
        string CommandPrefix { get; }
        string Version { get; }
        string Status { get; }
        string Token { get; }
    }
}