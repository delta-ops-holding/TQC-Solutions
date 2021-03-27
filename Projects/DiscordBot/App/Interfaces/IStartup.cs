using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public interface IStartup
    {
        Task InitBotAsync();
    }
}