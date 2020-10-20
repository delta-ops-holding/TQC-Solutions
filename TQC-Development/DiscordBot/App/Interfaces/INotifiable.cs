using Discord;
using DiscordBot.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public interface INotifiable
    {
        Task NotifyUserAsync(IUser discordUser, Name.ClanNames clanName);
        Task NotifyAdminAsync(byte platformId, IUser discordUser, Name.ClanNames clanName);
    }
}
