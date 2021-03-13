using DiscordBot.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    /// <summary>
    /// Represents a model of a User.
    /// </summary>
    public struct UserModel
    {
        private Guid _id;
        private ulong _discordId;
        private DateTimeOffset _date;
        private Clan _clanApplication;

        public UserModel(Guid id, ulong discordId, DateTimeOffset date, Clan clanApplication)
        {
            _id = id;
            _discordId = discordId;
            _date = date;
            _clanApplication = clanApplication;
        }

        /// <summary>
        /// Identifier of the user.
        /// </summary>
        public readonly Guid Id
        {
            get
            {
                return _id;
            }
        }

        /// <summary>
        /// Identifier of the discord user's unique id.
        /// </summary>
        public readonly ulong DiscordId { get { return _discordId; } }

        /// <summary>
        /// Date of when user was added.
        /// </summary>
        public readonly DateTimeOffset Date { get { return _date; } }

        // Get the clan of which the user has applied to.
        public readonly Clan ClanApplication { get { return _clanApplication; } }
    }
}
