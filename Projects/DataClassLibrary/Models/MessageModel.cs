using DataClassLibrary.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataClassLibrary.Models
{
    public struct MessageModel
    {
        public MessageModel(string message, byte platformId, Clan clan, object discordUser)
        {
            Message = message;
            PlatformId = platformId;
            Clan = clan;
            DiscordUser = discordUser;
        }

        public string Message { get; set; }
        public byte PlatformId { get; }
        public Clan Clan { get; }
        public object DiscordUser { get; }
    }
}
