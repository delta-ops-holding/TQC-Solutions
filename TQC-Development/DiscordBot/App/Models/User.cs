using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    public class User
    {
        private ulong _userId;
        private DateTimeOffset _date;

        /// <summary>
        /// Identifier of the discord user.
        /// </summary>
        public ulong UserId { get => _userId; set => _userId = value; }

        /// <summary>
        /// Date of when user was added.
        /// </summary>
        public DateTimeOffset Date { get => _date; set => _date = value; }
    }
}
