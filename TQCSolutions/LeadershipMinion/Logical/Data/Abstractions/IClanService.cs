using Discord.WebSocket;
using LeadershipMinion.Logical.Enums;

namespace LeadershipMinion.Logical.Data.Abstractions
{
    /// <summary>
    /// Represents a generic Clan Service, to retrieve clan specific information.
    /// </summary>
    public interface IClanService
    {
        /// <summary>
        /// Attempts to get a <see cref="Clan"/> by <see cref="Discord.Emote.Id"/>.
        /// </summary>
        /// <param name="id">A unique indetifier of the emote.</param>
        /// <returns>A <see cref="Clan"/> if found, otherwise <see cref="Clan.Undefined"/>.</returns>
        Clan GetClanNameByEmoteId(ulong id);

        /// <summary>
        /// Attempts to get a mention role command by <see cref="Clan"/>.
        /// </summary>
        /// <param name="clanName">Defines the name of the clan with it's associated mention role.</param>
        /// <returns>A string containing the mention role if found, otherwise nameof(<see cref="Clan.Undefined)"/>).</returns>
        string GetMentionRoleByClanName(Clan clanName);

        /// <summary>
        /// Attempts to get <see cref="ClanPlatform"/> by the <see cref="SocketReaction.Channel"/> id.
        /// </summary>
        /// <param name="id">A unique identifier of  the channel id.</param>
        /// <returns>A <see cref="ClanPlatform"/> if found, otherwise <see cref="ClanPlatform.Undefined"/>.</returns>
        ClanPlatform GetClanPlatformByChannelId(ulong id);
    }
}