using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace LeadershipMinion.Logical.Data.Abstractions
{
    public interface IApplicationHandler
    {
        Task CreateApplicationAsync(SocketReaction appliedBy, IUser userWhoReacted);
    }
}