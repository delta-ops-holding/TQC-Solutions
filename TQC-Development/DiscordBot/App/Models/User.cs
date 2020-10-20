using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBot.Models
{
    public class User
    {
        private ulong _userId;
        private DateTimeOffset _date;

        public ulong UserId { get => _userId; set => _userId = value; }
        public DateTimeOffset Date { get => _date; set => _date = value; }
    }
}
