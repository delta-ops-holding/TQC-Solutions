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
        private readonly Guid _id;
        private readonly ulong _discordId;
        private readonly DateTimeOffset _date;
        private readonly Clan _clanApplication;

        /// <summary>
        /// Create a User Model.
        /// </summary>
        /// <param name="id">A GUID representing the unique identify of the user.</param>
        /// <param name="discordId">Defines the identify specified by Discord.</param>
        /// <param name="date">Datetime of which the user was created.</param>
        /// <param name="clanApplication">A Clan of which the user has applied to.</param>
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
        public readonly Guid Id { get { return _id; } }

        /// <summary>
        /// Identifier of the discord user's unique id.
        /// </summary>
        public readonly ulong DiscordId { get { return _discordId; } }

        /// <summary>
        /// Date of when user was added.
        /// </summary>
        public readonly DateTimeOffset Date { get { return _date; } }

        /// <summary>
        /// Get the clan of which the user has applied to.
        /// </summary>        
        public readonly Clan ClanApplication { get { return _clanApplication; } }
    }
}
