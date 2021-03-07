using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    /// <summary>
    /// Represents a model of a User.
    /// </summary>
    public class UserModel
    {
        private Guid _id;
        private ulong _discordId;
        private DateTimeOffset _date;

        /// <summary>
        /// Identifier of the user.
        /// </summary>
        public Guid Id { get => _id; set => _id = value; }

        /// <summary>
        /// Identifier of the discord user's unique id.
        /// </summary>
        public ulong DiscordId { get => _discordId; set => _discordId = value; }

        /// <summary>
        /// Date of when user was added.
        /// </summary>
        public DateTimeOffset Date { get => _date; set => _date = value; }
    }
}
