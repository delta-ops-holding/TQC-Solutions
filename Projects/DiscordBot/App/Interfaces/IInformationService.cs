using DataClassLibrary.Models;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public interface IInformationService
    {
        MinionInformationModel MinionInformation { get; }
    }
}