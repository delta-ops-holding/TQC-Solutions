using DataClassLibrary.Enums;
using DataClassLibrary.Models;
using Discord;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public interface IDataService
    {
        Clan GetClanName(IEmote emote);
        Task<IEnumerable<LogModel>> GetLatestLogs();
        string GetRoleByClan(Clan clanName);
    }
}