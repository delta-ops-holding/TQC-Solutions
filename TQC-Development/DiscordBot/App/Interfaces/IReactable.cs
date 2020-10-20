using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Interfaces
{
    public interface IReactable
    {
        Task ReactionAddedAsync(IUserMessage userMessage, SocketReaction reaction);
    }
}
