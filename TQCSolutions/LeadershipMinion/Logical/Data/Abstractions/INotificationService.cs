using LeadershipMinion.Logical.Models;
using System.Threading.Tasks;

namespace LeadershipMinion.Logical.Data.Abstractions
{
    public interface INotificationService
    {
        Task SendApplicationAsync(MessageModel model);
        Task SendDirectMessageToUserAsync(MessageModel model);
    }
}