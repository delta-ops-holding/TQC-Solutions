using Discord;
using LeadershipMinion.Logical.Data.Abstractions;
using LeadershipMinion.Logical.Enums;
using LeadershipMinion.Logical.Models;
using System;

namespace LeadershipMinion.Logical.Data.Services
{
    /// <summary>
    /// A class that transforms messages into beautiful embeds.
    /// </summary>
    public class EmbedService : IEmbedService
    {
        public Embed BeautifyMessage(MessageModel model)
        {
            var embedMessage = new EmbedBuilder()
            {
                Title = "New Clan Application Arrived!",
                Description = model.Message,
                Color = GenerateCustomEmbedColor(CustomDiscordEmbedColor.Lavender),
                ThumbnailUrl = model.Application.ClanData.Icon,
                Timestamp = model.Application.RegistrationDate,
                Author = new EmbedAuthorBuilder()
                {
                    Name = $"{model.DiscordUser.Username}#{model.DiscordUser.Discriminator}",
                    IconUrl = model.DiscordUser.GetAvatarUrl()
                },
                Footer = new EmbedFooterBuilder()
                {
                    Text = model.Application.ClanData.Name,
                    IconUrl = GetThumbnailByPlatform(model.Application.ClanData.Platform)
                }
            };

            return embedMessage.Build();
        }

        private static Color GenerateCustomEmbedColor(CustomDiscordEmbedColor color) => color switch
        {
            CustomDiscordEmbedColor.Lavender => new Color(162, 94, 246),
            _ => throw new ArgumentException("Cannot generate custom color with defined argument.", nameof(color))
        };

        private static string GetThumbnailByPlatform(ClanPlatform platform) => platform switch
        {
            ClanPlatform.PC => "https://cdn.discordapp.com/emojis/641432631715561473.png?v=1",
            ClanPlatform.PSN => "https://cdn.discordapp.com/emojis/551501319177895958.png?v=1",
            ClanPlatform.XBOX => "https://cdn.discordapp.com/emojis/551501460202979328.png?v=1",
            ClanPlatform.CROSS => "https://cdn.discordapp.com/emojis/616051721008971786.png?size=96",
            _ => "https://cdn.discordapp.com/emojis/903676869973516308.png?size=96"
        };

        private static string GetClanNameAsReadable(Clan clan) => clan switch
        {
            Clan.TRΔNSIENT => nameof(Clan.TRΔNSIENT),
            Clan.TENΔCITY => nameof(Clan.TENΔCITY),
            Clan.ΔEGIS => nameof(Clan.ΔEGIS),
            Clan.ETHEREΔL => nameof(Clan.ETHEREΔL),
            Clan.CELESTIΔL => nameof(Clan.CELESTIΔL),
            Clan.MΔJESTIC => nameof(Clan.MΔJESTIC),
            Clan.DEFIΔNCE => nameof(Clan.DEFIΔNCE),
            Clan.VIGILΔNT => nameof(Clan.VIGILΔNT),
            Clan.TRΔNQUILITY => nameof(Clan.TRΔNQUILITY),
            Clan.ETERNΔL => nameof(Clan.ETERNΔL),
            Clan.IMMORTΔL => nameof(Clan.IMMORTΔL),
            Clan.EPHEMERΔ => nameof(Clan.EPHEMERΔ),
            Clan.SHΔDOW => nameof(Clan.SHΔDOW),
            Clan.CHΔOS => nameof(Clan.CHΔOS),
            Clan.QUΔNTUM => nameof(Clan.QUΔNTUM),
            Clan.ΔSTRAL => nameof(Clan.ΔSTRAL),
            _ => throw new ArgumentOutOfRangeException(nameof(clan), clan, null)
        };
    }
}
