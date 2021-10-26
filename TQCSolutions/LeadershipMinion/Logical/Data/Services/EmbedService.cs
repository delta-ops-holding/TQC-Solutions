using Discord;
using LeadershipMinion.Logical.Data.Abstractions;
using LeadershipMinion.Logical.Enums;
using LeadershipMinion.Logical.Models;
using System;

namespace LeadershipMinion.Logical.Data.Services
{
    public class EmbedService : IEmbedService
    {
        public Embed BeautifyMessage(MessageModel model)
        {
            //_logger.LogDebug($"Creating message embed for <{model.DiscordUser.Id}>.");

            // Create new Embed Builder.
            var embedMessage = new EmbedBuilder()
            {
                Title = "New Clan Application Arrived!",
                Description = model.Message,
                Color = GenerateCustomEmbedColor(CustomDiscordEmbedColor.Lavender),
            };

            embedMessage.WithFooter(nameof(model.Application.ClanData.Name), model.Application.ClanData.Icon);

            //var clan = model.Application.AppliedToClan;

            // Switch on provided platform identifier.
            //switch (model.Application.ClanAssociatedWithPlatform)
            //{
            //    case ClanPlatform.PC:
            //        embedMessage.Color = Color.Red;
            //        embedMessage.WithFooter($"{clan}", "https://cdn.discordapp.com/emojis/641432631715561473.png?v=1").WithCurrentTimestamp();
            //        break;
            //    case ClanPlatform.PSN:
            //        embedMessage.Color = Color.Blue;
            //        embedMessage.WithFooter($"{clan}", "https://cdn.discordapp.com/emojis/551501319177895958.png?v=1").WithCurrentTimestamp();
            //        break;
            //    case ClanPlatform.XBOX:
            //        embedMessage.Color = Color.Green;
            //        embedMessage.WithFooter($"{clan}", "https://cdn.discordapp.com/emojis/551501460202979328.png?v=1").WithCurrentTimestamp();
            //        break;
            //}

            return embedMessage.Build();
        }

        private static Color GenerateCustomEmbedColor(CustomDiscordEmbedColor color)
        {
            return color switch
            {
                CustomDiscordEmbedColor.Lavender => new Color(162, 94, 246),
                _ => throw new ArgumentException("Cannot generate custom color with defined argument.", nameof(color))
            };
        }
    }
}
