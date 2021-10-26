using System.Threading.Tasks;

namespace LeadershipMinion.Core.Abstractions
{
    /// <summary>
    /// Represents a generic Startup Service, to handle specific startup scenarios.
    /// </summary>
    public interface IStartupService
    {
        /// <summary>
        /// Starts a connection with Discord Service asynchronous.
        /// </summary>
        /// <remarks>
        /// awaits <see cref="Task.Delay(int)"/> indefinitely.
        /// </remarks>
        /// <returns>A Task representing the asynchronous process.</returns>
        Task InitializeBotAsync();
    }
}