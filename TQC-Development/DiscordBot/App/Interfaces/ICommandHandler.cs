using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public interface ICommandHandler
    {
        Task InitializeCommandsAsync();
    }
}