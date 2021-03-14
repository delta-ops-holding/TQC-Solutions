using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBot.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiscordBot.Commands.Modules
{
    [Group("info")]
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Group("log")]
        [Summary("Handles command for logs.")]
        public class LogModule : ModuleBase<SocketCommandContext>
        {
            private readonly IDataService _dataService;

            public LogModule(IDataService dataService)
            {
                _dataService = dataService;
            }

            [Command("latest")]
            [Summary("Pulls the latest logs from the database within a day.")]
            public async Task LatestAsync()
            {
                var user = Context.User as SocketGuildUser;

                if (!user.Roles.Any(role => role.Name.Equals("Mushroom")))
                {
                    return;
                }

                var logs = await _dataService.GetLatestLogs();

                EmbedBuilder embed = new()
                {
                    Title = "Today's Logs",
                    Timestamp = DateTime.UtcNow,
                    Footer = new EmbedFooterBuilder
                    {
                        Text = $"Showing top {((logs.Count() <= 10) ? logs.Count() : 10)} logs."
                    }
                };

                List<EmbedFieldBuilder> embeds = new List<EmbedFieldBuilder>();

                for (int i = 0; i < logs.Count(); i++)
                {
                    embeds.Add(new EmbedFieldBuilder()
                    {
                        Name = logs.ElementAt(i).CreatedDate.ToString(),
                        Value = $"{logs.ElementAt(i).Source}\n{logs.ElementAt(i).CreatedBy}\n{logs.ElementAt(i).Message}",
                        IsInline = false
                    });
                }

                embed.Fields = embeds;

                await ReplyAsync(embed: embed.Build());
            }
        }
    }
}
