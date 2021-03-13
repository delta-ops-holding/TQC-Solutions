using Discord;
using DiscordBot.Enums;

namespace DiscordBot.Interfaces
{
    public interface IDataService
    {
        Clan GetClanName(IEmote emote);
        string GetRoleByClan(Clan clanName);
    }
}