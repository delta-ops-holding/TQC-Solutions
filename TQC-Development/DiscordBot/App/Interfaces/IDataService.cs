using Discord;
using DiscordBot.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public interface IDataService
    {
        Clan GetClanName(IEmote emote);
        Task<IEnumerable<DatabaseAccess.Models.LogMessage>> GetLatestLogs();
        string GetRoleByClan(Clan clanName);
    }
}