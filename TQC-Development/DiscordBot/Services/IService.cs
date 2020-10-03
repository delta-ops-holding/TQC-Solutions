using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DiscordBot.Services
{
    public interface IService
    {
        Task ReactionAddedAsync(IUserMessage userMessage, ISocketMessageChannel messageChannel, SocketReaction reaction);
    }
}
