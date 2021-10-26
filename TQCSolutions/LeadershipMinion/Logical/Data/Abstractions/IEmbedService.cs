using Discord;
using LeadershipMinion.Logical.Models;

namespace LeadershipMinion.Logical.Data.Abstractions
{
    public interface IEmbedService
    {
        /// <summary>
        /// Turns a message into an <see cref="Embed"/> object.
        /// </summary>
        /// <param name="model">A message to beautify.</param>
        /// <returns>An <see cref="Embed"/> object.</returns>
        Embed BeautifyMessage(MessageModel model);
    }
}