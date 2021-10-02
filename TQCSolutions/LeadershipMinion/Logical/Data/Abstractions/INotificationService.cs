using Discord;
using LeadershipMinion.Logical.Enums;
using LeadershipMinion.Logical.Models;
using System.Threading.Tasks;

namespace LeadershipMinion.Logical.Data.Abstractions
{
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
    }
}