using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Commands.Modules
{
    [NamedArgumentType]
    public class ClanRosterArguments
    {
        public Clan Name { get; set; }
        public SocketGuildUser User { get; set; }
        public bool IsFounder { get; set; }
    }

    [Group("clan")]
    [Alias("c")]
    public class ClanModule : ModuleBase<SocketCommandContext>
    {
        [Group("roster")]
        [Alias("r")]
        public class RosterModule : ModuleBase<SocketCommandContext>
        {
            [Command("add")]
            [Summary("Adds a user to the clan roster, specified by a clan and the user to be added.")]
            public async Task AddUserAsync(SocketGuildUser user = null, string clanName = "", bool isFounder = false)
            {
                if (user == null)
                {
                    user = Context.User as SocketGuildUser;
                }

                string founder = (isFounder == true) ? "Founder" : "Admin";
                string nickname = (!string.IsNullOrEmpty(user.Nickname)) ? user.Nickname : user.Username;

                await ReplyAsync($"Added {nickname} to {clanName} roster as {founder}.");
            }
        }
    }
}
