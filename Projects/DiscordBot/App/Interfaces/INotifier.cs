using DataClassLibrary.Enums;
using Discord;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    /// <summary>
    /// Provides the ability to notifiy.
    /// </summary>
    public interface INotifier
    {
        /// <summary>
        /// Notifies a user as direct message.
        /// </summary>
        /// <param name="discordUser">The user to direct message.</param>
        /// <param name="clanName">The name of the clan.</param>
        Task NotifyUserAsync(IUser discordUser, Clan clanName);

        /// <summary>
        /// Notifies an admin role.
        /// </summary>
        /// <param name="platformId">Used to identify the admin for a platform.</param>
        /// <param name="discordUser">The user which invoked the notification.</param>
        /// <param name="clanName">The name of the clan.</param>
        Task NotifyAdminAsync(byte platformId, IUser discordUser, Clan clanName);
    }
}
