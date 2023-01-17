using LeadershipMinion.Logical.Models;
using System.Threading.Tasks;
using Discord.WebSocket;

namespace LeadershipMinion.Logical.Data.Abstractions
{
    /// <summary>
    /// Represents a generic Notification Service.
    /// </summary>
    public interface INotificationService
    {
        /// <summary>
        /// Notifies an admin by role, in a specific channel.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A Task representing the asynchrounous process.</returns>
        Task NotifyStaffsAsync(MessageModel model);

        /// <summary>
        /// Notifies a user in DM's.
        /// </summary>
        /// <param name="model"></param>
        /// <returns>True if user has privacy settings on, otherwise false.</returns>
        Task<bool> NotifyUserAsync(MessageModel model);
        Task CalBotGhostPingAsync(SocketMessage message, string PingStr);
    }
}