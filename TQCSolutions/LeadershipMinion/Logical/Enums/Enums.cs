namespace LeadershipMinion.Logical.Enums
{
    /// <summary>
    /// Represents an environment that systems runs in.
    /// </summary>
    public enum SystemEnvironment
    {
        Debug,
        Test,
        Development,
        Production
    }

    /// <summary>
    /// Defines a list of predefined custom discord embed colors.
    /// </summary>
    public enum CustomDiscordEmbedColor
    {
        Lavender
    }

    /// <summary>
    /// Represents the current clan.
    /// </summary>
    public enum Clan
    {
        Undefined,
        TRΔNSIENT, TENΔCITY, ΔEGIS, ETHEREΔL, CELESTIΔL, MΔJESTIC,
        DEFIΔNCE, VIGILΔNT, TRΔNQUILITY, ETERNΔL, IMMORTΔL,
        EPHEMERΔ, SHΔDOW, QUΔNTUM
    }

    /// <summary>
    /// Represents a unique platform for a unique object.
    /// </summary>
    public enum ClanPlatform
    {
        Undefined,
        PC,
        PSN,
        XBOX,
        CROSS
    }
}